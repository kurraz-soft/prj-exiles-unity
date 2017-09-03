using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace peasants
{
    public class Manager : MonoBehaviour
    {
        public Peasant prefab = null;

        public List<Peasant> peasants;
        

        void Awake()
        {
            peasants = FindObjectsOfType<Peasant>().ToList();
        }

        public Peasant Create(Vector3 position)
        {
            var p = (Peasant)Instantiate(prefab, position, Quaternion.identity);

            p.transform.parent = DynamicObjectsManager.Instance.transform;

            peasants.Add(p);

            return p;
        }

        /// <summary>
        /// Assign peasant to work
        /// </summary>
        /// <param name="prof"></param>
        /// <param name="count"></param>
        public void AssignWork(Profession prof, int count)
        {
            var profPeasants = new List<Peasant>();
            var available = new List<Peasant>();

            foreach (var p in peasants)
            {
                if (p.Profession == prof)
                {
                    profPeasants.Add(p);
                }
                else if(p.Profession == Profession.None)
                {
                    available.Add(p);
                }
                
            }

            if (count > profPeasants.Count && available.Count > 0)
            {
                var arr = available.Take(count - profPeasants.Count);
                foreach (var p in arr)
                {
                    //see property __set__
                    p.Profession = prof;
                }
            }
            else if(count < profPeasants.Count)
            {
                var arr = profPeasants.Take(profPeasants.Count - count);
                foreach (var p in arr)
                {
                    p.Profession = Profession.None;
                }
            }
        }
    }
}
