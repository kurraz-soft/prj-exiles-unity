using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace buildings
{
    public class ConstructController : MonoBehaviour
    {
        public Construct construct = null;
        public Building building = null;

        public List<ResourceCost> resourceCost;

        [Serializable]
        public struct ResourceCost
        {
            public string type;
            public int num;
        }

        /// <summary>
        /// Prepare for BuildMark
        /// </summary>
        public void SetBuildState()
        {
            construct.gameObject.SetActive(true);
            construct.stages.ForEach(stage => stage.SetActive(false));
            building.gameObject.SetActive(false);
            construct.buildMark.gameObject.SetActive(true);
        }

        public void SetCompleteState()
        {
            GameManager.Instance.BuildingManager.UpdateBuildingsList();
        }

        public void Init()
        {
            construct.stages.ForEach(stage => stage.SetActive(false));
            if (construct.isConstructed)
            {
                building.gameObject.SetActive(true);
                GameManager.Instance.WorkManager.ReAssignByWorkplace(construct);
                construct.gameObject.SetActive(false);
            }
            else
            {
                building.gameObject.SetActive(false);
                construct.gameObject.SetActive(true);
                construct.stages.First().SetActive(true);

                //Set resource cost from editor to construct
                foreach (var cost in resourceCost)
                {
                    construct.resourcesCost[cost.type] = cost.num;
                }
            }
        }

        void Awake()
        {
            Init();
        }
    }
}
