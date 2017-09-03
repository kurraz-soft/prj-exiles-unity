using System.Collections.Generic;
using System.Linq;
using items;
using peasants;
using peasants.workers;
using UnityEngine;

namespace buildings
{
    public class Mine : WorkBuilding
    {
        public int oreCapacity = 10000;
        public float energyPerPoint = 100f;
        public float oreMined = 0;
        public int maxDropOrePoints = 5;
        public string oreType = ItemType.COAL;

        public List<Miner> workers;

        public int Mining(float energy)
        {
            oreMined += energy/energyPerPoint;
            if (oreMined >= maxDropOrePoints)
            {
                oreMined -= maxDropOrePoints;
                oreCapacity -= maxDropOrePoints;

                return maxDropOrePoints;
            }

            return 0;
        }

        //-----------------OVERRIDE---------------------

        public override int WorkersCount
        {
            get { return workers.Count; }
        }

        public override void WorkerArrived(Peasant p)
        {
            var w = p.gameObject.GetComponent<Miner>() ?? p.gameObject.AddComponent<Miner>();
            w.workplace = this;
            if (!workers.Contains(w))
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
            workers = new List<Miner>();
        }

        //----------------\OVERRIDE---------------------
    }


}
