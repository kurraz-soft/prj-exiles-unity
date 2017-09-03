using UnityEngine;

namespace buildings.resources
{
    [RequireComponent(typeof(AreaMarkerBehavior))]
    class LumberjackHutBuildMark : BuildMark
    {
        protected override void Awake()
        {
            base.Awake();
            var b = GetComponentInParent<ConstructController>().building as LumberjackHut;
            GetComponent<AreaMarkerBehavior>().radius = b.scopeRadius;
        }
    }
}
