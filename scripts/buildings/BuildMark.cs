using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace buildings
{
    public class BuildMark : MonoBehaviour
    {
        public GameObject mesh = null;
        protected List<BuildMarkPlatformCell> platformCells;

        public virtual bool CanPlace
        {
            get { return platformCells.FirstOrDefault(c => !c.CanPlace) == null; }
        }

        protected virtual void Awake()
        {
            platformCells = GetComponentsInChildren<BuildMarkPlatformCell>().ToList();
        }

        private void Update()
        {
            MouseHandler();
        }

        protected virtual void MouseHandler()
        {
            
        }
    }
}
