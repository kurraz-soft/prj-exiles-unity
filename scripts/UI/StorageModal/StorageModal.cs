using System.Collections.Generic;
using buildings;
using UnityEngine;
using UnityEngine.UI;
using UI.StorageModal;

class StorageModal : MonoBehaviour
{
    public Button CloseButton = null;
    public GameObject Panel = null;
    public GameObject DataTablePanel = null;
    public DataRow DataRowPrefab = null;

    private static StorageModal _instance;

    public static StorageModal Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(StorageModal)) as StorageModal;
                if(!_instance)
                    Debug.LogError("Can't find modal");
            }
            return _instance;
        }
    }

    public void OnClickCloseBtn()
    {
        Panel.SetActive(false);
    }

    public void Show(Storage storage)
    {
        var children = new List<Transform>();
        foreach (Transform child in DataTablePanel.transform) children.Add(child);
        children.ForEach(child => Destroy(child.gameObject));

        foreach (var itemPack in storage.items)
        {
            var row = Instantiate(DataRowPrefab);
            row.transform.SetParent(DataTablePanel.transform);
            row.Set(itemPack.Key, itemPack.Value.ToString());
        }
        
        
        Panel.SetActive(true);
    }
}
