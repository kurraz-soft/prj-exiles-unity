using System;
using System.Linq;
using UnityEngine;

namespace buildings
{
    public class BuildMarkPlatformCell : MonoBehaviour
    {
        public GroundType matchGroundType = GroundType.Ground;
        private string matchColliderTag;

        private Color _markColor;
        private Color _markDeniedColor;
        private Renderer _renderer;
        private GameObject _collider;
        private Vector3 lastPos = Vector3.zero;

        public virtual bool CanPlace
        {
            get
            {
                return _collider != null && _collider.tag == matchColliderTag;
            }
        }

        void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _markColor = _renderer.material.color;
            _markDeniedColor = new Color(255, 0, 0, 255);

            matchColliderTag = Enum.GetName(typeof(GroundType), matchGroundType);
        }

        void OnTriggerEnter(Collider collider)
        {
            if(collider.gameObject.name.Contains("BuildMarkPlatformCell")) return;
            if (Enum.GetNames(typeof(GroundType)).Any(ground_type => ground_type == collider.tag)) return;

            _collider = collider.gameObject;

            _renderer.material.color = CanPlace ? _markColor : _markDeniedColor;
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.name.Contains("BuildMarkPlatformCell")) return;
            if (Enum.GetNames(typeof(GroundType)).Any(ground_type => ground_type == collider.tag)) return;

            lastPos = Vector3.zero;
            _collider = null;

            _renderer.material.color = CanPlace ? _markColor : _markDeniedColor;
        }

        void Update()
        {
            if(lastPos == transform.position) return;

            lastPos = transform.position;

            Ray ray = new Ray(transform.position,Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1.0f))
            {
                _collider = hit.collider.gameObject;
            }

            _renderer.material.color = CanPlace ? _markColor : _markDeniedColor;
        }
    }
}
