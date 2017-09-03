using System.Collections;
using UnityEngine;

namespace animals
{
    class Horse : MonoBehaviour
    {
        public float RambleStepRadius = 20.0f;

        private Animator anim;
        private NavMeshAgent nav;
        private Vector3 startPos;

        void Awake()
        {
            anim = GetComponent<Animator>();
            nav = GetComponent<NavMeshAgent>();
            startPos = transform.position;
        }

        void Start()
        {
            
        }

        void Update()
        {
            if (PathComplete())
            {
                nav.ResetPath();
                anim.SetBool("isWalk", false);
                StartCoroutine(Ramble());
            }
            else
            {
                anim.SetBool("isWalk", true);
            }
        }

        public bool PathComplete()
        {
            if (Vector3.Distance(nav.destination, nav.transform.position) <= nav.stoppingDistance + 2.0f)
            {
                if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f || nav.remainingDistance <= 2.0f)
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerator Ramble()
        {
            yield return new WaitForSeconds(2);   

            //Recalc destination

            var center = startPos;
            var radius = 30f;

            var pos = transform.position;

            if (Mathf.Pow(pos.x - center.x, 2) + Mathf.Pow(pos.z - center.z, 2) > Mathf.Pow(radius, 2))
            {
                //Pos is outside ramble borders
                //Need to move in

                Ray ray = new Ray(pos, center - pos);

                nav.destination = ray.GetPoint(RambleStepRadius);
            }
            else
            {
                Vector3 dest = new Vector3();

                //var step = Random.Range(-RambleStepRadius, RambleStepRadius);
                var step = Random.value > 0.5 ? -RambleStepRadius : RambleStepRadius;

                dest.x = Random.Range(pos.x - step, pos.x + step);
                dest.z = Mathf.Sqrt(Mathf.Abs(Mathf.Pow(dest.x - pos.x, 2) - Mathf.Pow(step, 2)));
                dest.z = Random.value > 0.5 ? dest.z : -dest.z;
                dest.z += pos.z;

                nav.destination = new Vector3(dest.x, pos.y, dest.z);
            }
        }
    }
}
