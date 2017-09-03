using UnityEngine;
using UnityEngine.UI;

namespace UI.StorageModal
{
    class DataRow : MonoBehaviour
    {
        public Text Name = null;
        public Text Value = null;

        public void Set(string name, string value)
        {
            Name.text = name;
            Value.text = value;
        }
    }
}
