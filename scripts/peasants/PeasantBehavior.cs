using UnityEngine;

namespace peasants
{
    [RequireComponent(typeof(Peasant))]
    abstract public class PeasantBehavior : MonoBehaviour
    {
        protected Peasant peasant;

        public Peasant Peasant
        {
            get { return peasant; }
        }

        protected virtual void Awake()
        {
            peasant = GetComponent<Peasant>();
        }
    }
}
