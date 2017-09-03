using UnityEngine;

namespace scripts.resources
{
    public class LumberTree : MonoBehaviour
    {
        public float hitpoints;
        public int value;

        void Awake()
        {
            hitpoints = 30;
            value = Random.Range(1, 4);
        }

        public int Cut()
        {
            Destroy(gameObject);

            return value;
        }
    }
}
