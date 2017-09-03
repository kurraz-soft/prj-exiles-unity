using System.Collections.Generic;
using System.Linq;
using items;
using peasants;
using UnityEngine;

namespace buildings
{
    public class Manager : MonoBehaviour
    {
        public List<Building> buildings;

        private Settlement _settlement;

        public Settlement settlement
        {
            get
            {
                if (_settlement == null)
                {
                    _settlement = new Settlement(this);
                }
                return _settlement;
            }
        }

        void Awake()
        {
            
        }

        void Start()
        {
            UpdateBuildingsList();
            var storage = buildings.OfType<Storage>().First();
            storage.items[ItemType.WOOD] = 10;
        }

        public void UpdateBuildingsList()
        {
            buildings = FindObjectsOfType<Building>().ToList();
            GameManager.Instance.WorkManager.CalcRequiredCounters();
            GameManager.Instance.WorkManager.ApplyCounters();
        }

        public Dictionary<Profession, int> GetRequiredWorkers()
        {
            var ret = new Dictionary<Profession, int>();

            foreach (var b in buildings)
            {
                if (b is WorkBuilding)
                {
                    var wb = (WorkBuilding) b;
                    if (!ret.ContainsKey(wb.profession)) ret[wb.profession] = 0;
                    ret[wb.profession] += wb.workplaces;
                }
            }

            return ret;
        }

        public WorkBuilding GetClosestAvailableProfBuilding(Vector3 from,Profession prof)
        {
            float minDistance = -1;
            WorkBuilding closestBuilding = null;

            foreach (var b in buildings)
            {
                if (b is WorkBuilding)
                {
                    var wb = (WorkBuilding)b;
                    if (wb.profession == prof)
                    {
                        if (wb.WorkersCount < wb.workplaces)
                        {
                            var dist = Vector3.Distance(from, b.transform.position);
                            if (minDistance < 0 || minDistance > dist)
                            {
                                minDistance = dist;
                                closestBuilding = wb;
                            }
                        }
                    }
                }
            }

            return closestBuilding;
        }

        public Storage GetClosestStorage(Vector3 from)
        {
            var b = (Storage)buildings.Where(t => t is Storage)
                .OrderBy(t => Vector3.Distance(from,t.transform.position))
                .FirstOrDefault();

            return b;
        }

        public void AddBuilding(Building b)
        {
            buildings.Add(b);
            if(b is WorkBuilding)
                GameManager.Instance.WorkManager.ReAssignByWorkplace((WorkBuilding)b);
            settlement.IsDirty = true;
        }

        public class Settlement
        {
            private readonly Manager _manager;
            private Vector3 _center;
            private float _radius;

            public bool IsDirty
            {
                get { return _isDirtyCenter || _isDirtyRadius; }
                set 
                { 
                    _isDirtyCenter = value;
                    _isDirtyRadius = value;
                }
            }

            private bool _isDirtyCenter = true;
            private bool _isDirtyRadius = true;

            public Settlement(Manager bManager)
            {
                _manager = bManager;
            }

            public Vector3 Center
            {
                get
                {
                    if (_isDirtyCenter)
                    {
                        var buildings = _manager.buildings;

                        float minX = buildings[0].transform.position.x,
                        minY = buildings[0].transform.position.y,
                        maxX = buildings[0].transform.position.x,
                        maxY = buildings[0].transform.position.y;

                        foreach (var building in buildings)
                        {
                            if (building.transform.position.x < minX) minX = building.transform.position.x;
                            if (building.transform.position.x > maxX) maxX = building.transform.position.x;
                            if (building.transform.position.y < minY) minY = building.transform.position.y;
                            if (building.transform.position.y > maxY) maxY = building.transform.position.y;
                        }

                        _center = new Vector3((maxX - minX) / 2 + minX, (maxY - minY) / 2 + minY);
                        _isDirtyCenter = false;
                    }

                    return _center;
                }
            }

            public float Radius
            {
                get
                {
                    if (_isDirtyRadius)
                    {
                        var buildings = _manager.buildings;

                        float minX = buildings[0].transform.position.x,
                        minY = buildings[0].transform.position.y,
                        maxX = buildings[0].transform.position.x,
                        maxY = buildings[0].transform.position.y;

                        foreach (var building in buildings)
                        {
                            if (building.transform.position.x < minX) minX = building.transform.position.x;
                            if (building.transform.position.x > maxX) maxX = building.transform.position.x;
                            if (building.transform.position.y < minY) minY = building.transform.position.y;
                            if (building.transform.position.y > maxY) maxY = building.transform.position.y;
                        }

                        _radius = Mathf.Max(maxX - minY, maxY - minY);
                        _isDirtyRadius = false;
                    }
                    return _radius;
                }
            }
        }
    }
}
