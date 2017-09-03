using System.Collections.Generic;
using System.Linq;
using peasants;
using peasants.workers;
using scripts.resources;
using UnityEngine;

namespace buildings.resources
{
    class LumberjackHut : WorkBuilding
    {
        public List<Lumberjack> workers;

        public float scopeRadius = 30.0f;

        public int maxResourceDropped = 5;
        public int resourceCollected = 0;

        private void Awake()
        {
            
        }

        public bool CanDrop()
        {
            return resourceCollected >= maxResourceDropped;
        }

        //-----------------OVERRIDE---------------------
        public override int WorkersCount
        {
            get { return workers.Count; }
        }
        
        public override void WorkerArrived(Peasant p)
        {
            var w = p.gameObject.GetComponent<Lumberjack>() ?? p.gameObject.AddComponent<Lumberjack>();
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
            workers = new List<Lumberjack>();
        }
        //----------------\OVERRIDE---------------------

        public LumberTree GetTree()
        {
            var trees = new List<LumberTree>();

            Collider[] hits = Physics.OverlapSphere(transform.position, scopeRadius);
            foreach (var hit in hits)
            {
                var tree = hit.GetComponentInParent<LumberTree>();
                if(tree != null) trees.Add(tree);
            }

            if (trees.Count == 0)
            {
                return null;
            }

            var rand = Random.Range(0, trees.Count);

            return trees[rand];
        }

        /*
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, scopeRadius);
        }*/
    }
}
