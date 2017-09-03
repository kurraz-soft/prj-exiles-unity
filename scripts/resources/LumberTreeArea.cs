using UnityEngine;

namespace scripts.resources
{
    class LumberTreeArea : MonoBehaviour
    {
        public float scopeRadius = 30;
        [Range(0,1)]
        public float apperanceChance = 0.8f;
        public float stepRadiusFrom = 5.0f;
        public float stepRadiusTo = 10.0f;

        public GameObject treePrefab = null;

        void Start()
        {
            float startX = transform.position.x - scopeRadius;
            float startZ = transform.position.z + scopeRadius;

            for (float z = startZ; z > startZ - scopeRadius * 2; z -= Random.Range(stepRadiusFrom, stepRadiusTo))
            {
                for (float x = startX; x < startX + scopeRadius * 2; x += Random.Range(stepRadiusFrom, stepRadiusTo))
                {
                    if (Random.value <= apperanceChance)
                    {
                        var tree = Instantiate(treePrefab, new Vector3(x, transform.position.y, z), Quaternion.Euler(new Vector3(0,Random.Range(0,360),0))) as GameObject;
                        tree.transform.parent = transform;
                    }
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(transform.position, scopeRadius);
            Gizmos.DrawWireCube(transform.position, new Vector3(scopeRadius * 2, 0, scopeRadius * 2));
        }
    }
}
