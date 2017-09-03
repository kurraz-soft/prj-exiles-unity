using items;
using UnityEngine;

namespace buildings
{
    public class Storage : Building
    {
        public ItemList items = new ItemList();
        /// <summary>
        ///  Upload cargo time in seconds
        /// </summary>
        public float uploadTime = 1;

        void OnMouseDown()
        {
            StorageModal.Instance.Show(this);
        }
    }
}
