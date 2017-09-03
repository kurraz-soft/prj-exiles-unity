using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using items;
using peasants;
using peasants.workers;
using UnityEngine;

namespace buildings
{
    public class Construct : WorkBuilding
    {
        public List<GameObject> stages = null;
        public BuildMark buildMark = null;
        public bool isConstructed = false;

        private float _progressConsumedEnergy = 0;
        public float consumeWorkEnergy = 1000;

        public float progressConsumedEnergy
        {
            get { return _progressConsumedEnergy; }
            set
            {
                _progressConsumedEnergy = value;
                UpdateBuildStage();
            }
        }

        public int Progress
        {
            get { return (int)((progressConsumedEnergy/consumeWorkEnergy)*100); }
        }

        public override int WorkersCount
        {
            get { return workers.Count; }
        }

        public ItemList resourcesCost = new ItemList();

        public List<Builder> workers = new List<Builder>();

        public List<Builder.RequiredResource> alreadyAssigned; //TODO private

        public State state = State.WaitingForWorkers;

        public enum State
        {
            NoResources,
            NoRequired,
            WaitingForWorkers,
        }

        public override void WorkerArrived(Peasant p)
        {
            var w = p.gameObject.GetComponent<Builder>() ?? p.gameObject.AddComponent<Builder>();
            w.workplace = this;
            if(!workers.Contains(w))
                workers.Add(w);
        }

        public override void WorkerLeft(Peasant p)
        {
            var worker = workers.First(w => w.Peasant == p);
            workers.Remove(worker);
            Destroy(worker);
        }

        public override void ReleaseWorkers()
        {
            foreach (var w in workers)
            {
                w.Peasant.workplace = null;
                w.Peasant.GoHome();
                Destroy(w);
            }
            workers = new List<Builder>();
        }

        public void UpdateBuildStage()
        {
            if (Progress >= 100)
            {
                isConstructed = true;

                var controller = GetComponentInParent<ConstructController>();
                controller.Init();
                controller.SetCompleteState();
            }
            else if(Progress > 0)
            {
                var stageStep = 100.0f / stages.Count;
                var stageN = (int)Math.Ceiling(Progress / stageStep) - 1;
                var stage = stages[stageN];
                stages.ForEach(s => s.SetActive(false));
                stage.SetActive(true);
            }
        }

        void Update()
        {
            switch (state)
            {
                case State.WaitingForWorkers:
                    foreach (var w in workers.Where(w => w.state == Builder.State.Free))
                        AssignResources(w);
                    UpdateAssignedResources();
                    break;
                case State.NoResources:
                    //TODO Neeed some event to check newly arrived resources to storages
                    break;
                case State.NoRequired:
                    foreach (var w in workers)
                    {
                        if(w.state == Builder.State.Free)
                            w.state = Builder.State.Build;
                    }
                    break;
            }
        }

        void StartBuild()
        {
            foreach (var worker in workers)
            {
                worker.state = Builder.State.Build;

                //Get them to workplace
                worker.Peasant.MoveToWork(this);
            }
        }

        private void AssignResources(Builder worker)
        {
            var resType = GetRequiredResourceType();

            if (resType == null)
            {
                StartBuild();
                state = State.NoRequired;
                return;
            }

            var assignedByStorages = new Dictionary<Storage, int>();
            //Initialize for each storage
            foreach (var s in GameManager.Instance.BuildingManager.buildings.OfType<Storage>()) assignedByStorages[s] = 0;

            foreach (var res in alreadyAssigned)
            {
                if (res.type != resType) continue;

                if (!assignedByStorages.ContainsKey(res.storage))
                    assignedByStorages[res.storage] = 0;

                assignedByStorages[res.storage] += res.num;
            }

            var num = Math.Min(resourcesCost[resType], worker.MaxCanCarry);

            Storage storage;

            try
            {
                storage = GameManager.Instance.BuildingManager.buildings.OfType<Storage>().OrderBy(s => Vector3.Distance(s.transform.position, worker.transform.position)).First(s => s.items.ContainsKey(resType) && s.items[resType] >= assignedByStorages[s]);
            }
            catch (Exception)
            {
                //Not found
                state = State.NoResources;
                return;
            }

            var availableInStorage = storage.items[resType] - assignedByStorages[storage];

            num = Math.Min(availableInStorage, num);

            worker.SendForResources(new Builder.RequiredResource(resType, num, storage));
        }

        public void UpdateAssignedResources()
        {
            alreadyAssigned = (from w in workers where !w.requiredResource.IsEmpty() select w.requiredResource).ToList();
        }

        private string GetRequiredResourceType()
        {
            if (resourcesCost.Keys.Count == 0) return null;

            UpdateAssignedResources();

            var resType = "";

            foreach (var kv in resourcesCost)
            {
                var cnt = alreadyAssigned.Where(res => res.type == kv.Key).Sum(res => res.num);

                if (cnt < kv.Value)
                {
                    resType = kv.Key;
                    break;
                }
            }

            return resType.Length == 0 ? null : resType;
        }


        private void Start()
        {
            //Debug.Log(resourcesCost);
        }

        private void OnDestroy()
        {
            GameManager.Instance.BuildingManager.buildings.Remove(this);
        }
    }
}
