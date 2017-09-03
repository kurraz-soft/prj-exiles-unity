using System.Collections.Generic;
using System.Linq;
using buildings;
using buildings.living;
using items;
using UI;
using UnityEngine;

namespace peasants
{
    public enum Profession
    {
        None,
        Builder,
        Miner,
        Lumberjack,
        Fisherman,
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class Peasant : MonoBehaviour
    {
        public string personName;
        public int age;

        private float _energy = 100.0f;
        public ItemList items = new ItemList();

        private NavMeshAgent nav;
        private const float PATH_COMPLETE_MIN_DISTANCE = 1f;

        public State state = State.Free;
        private Profession profession = Profession.None;
        public WorkBuilding workplace;

        public Building home;

        [Header("Ramble params")]
        public float RambleStepRadius = 5.0f;

        public float energy
        {
            get { return _energy; }
            set
            {
                _energy = value;
                if (_energy <= 0)
                {
                    //Take a break
                    _energy = 0;
                    GoHome();
                }
                else if(_energy >= 100)
                {
                    //Energy restored go to work
                    _energy = 100;
                    MoveToWork(workplace);
                }
            }
        }

        public Profession Profession
        {
            get { return profession; }
            set
            {
                if (value == Profession.None)
                {
                    state = State.Free;
                    var behavours = GetComponents<PeasantBehavior>();
                    if (behavours != null && behavours.Length > 0)
                    {
                        foreach (var b in behavours)
                        {
                            Destroy(b);
                        }
                    }
                }
                else
                {
                    workplace =
                        GameManager.Instance.BuildingManager.GetClosestAvailableProfBuilding(transform.position, value);
                    if(workplace != null)
                        MoveToWork(workplace);
                }
                profession = value;
            }

        }
        

        public enum State
        {
            Free,
            MoveToWork,
            MoveToHome,
            AtWork,
        }

        void Awake()
        {
            nav = GetComponent<NavMeshAgent>();

            var names = ReadNamesFile();

            personName = names[Random.Range(0, names.Length)];

            age = Random.Range(16, 50);
        }

        string[] ReadNamesFile()
        {
            TextAsset file = Resources.Load<TextAsset>("names");

            var names = file.text.Split('\n');

            return names;
        }

        void Start()
        {
            //TODO temporary
            home = GameManager.Instance.BuildingManager.buildings.OfType<LivingBuilding>().First();
        }

        public void MoveTo(Vector3 dest)
        {
            nav.destination = dest;
        }

        public void MoveTo(Building b)
        {
            MoveTo(b.entryPoint.transform.position);
        }

        public void MoveToWork(WorkBuilding b)
        {
            MoveTo(b);
            state = State.MoveToWork;
        }

        public void GoHome()
        {
            if(workplace != null)
                workplace.WorkerLeft(this);

            state = State.MoveToHome;
            if(home != null)
                MoveTo(home);
        }

        public bool PathComplete()
        {
            if (Vector3.Distance(nav.destination, nav.transform.position) <= nav.stoppingDistance + PATH_COMPLETE_MIN_DISTANCE)
            {
                if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f || nav.remainingDistance <= PATH_COMPLETE_MIN_DISTANCE)
                {
                    return true;
                }
            }

            return false;
        }

        void Update()
        {
            CheckState();
        }

        void CheckState()
        {
            if (workplace == null)
                state = State.Free;

            if (state == State.AtWork) return;

            if (PathComplete())
            {
                switch (state)
                {
                    case State.Free:
                        Ramble();
                        break;
                    case State.MoveToHome:
                        //Restore energy
                        //TODO think about restore value
                        energy += 15.0f * Time.deltaTime;
                        break;
                    case State.MoveToWork:
                        state = State.AtWork;
                        workplace.WorkerArrived(this);
                        break;
                }
            }
        }

        public float DecreaseEnergy()
        {
            var spend = 5.0f*Time.deltaTime;
            energy -= spend;
            return spend;
        }

        void OnMouseDown()
        {
            UIController.Instance.peasantModal.Show(this);
        }

        private void Ramble()
        {
            //Recalc destination

            var center = GameManager.Instance.WorkManager.peasantAppearancePoint.position;
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

                var step = Random.Range(-RambleStepRadius , RambleStepRadius);

                dest.x = Random.Range(pos.x - step, pos.x + step);
                dest.z = Mathf.Sqrt(Mathf.Abs(Mathf.Pow(dest.x - pos.x, 2) - Mathf.Pow(step, 2)));
                dest.z = Random.value > 0.5 ? dest.z : -dest.z;
                dest.z += pos.z;

                nav.destination = new Vector3(dest.x, pos.y, dest.z);
            }
        }
    }
}
