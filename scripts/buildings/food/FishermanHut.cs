using System.Collections.Generic;
using System.Linq;
using peasants;
using peasants.workers;

namespace buildings.food
{
    class FishermanHut : WorkBuilding
    {
        public List<Fisherman> workers;

        //-----------------OVERRIDE---------------------
        public override int WorkersCount
        {
            get { return workers.Count; }
        }

        public override void WorkerArrived(Peasant p)
        {
            var w = p.gameObject.GetComponent<Fisherman>() ?? p.gameObject.AddComponent<Fisherman>();
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
            workers = new List<Fisherman>();
        }
        //----------------\OVERRIDE---------------------
    }
}
