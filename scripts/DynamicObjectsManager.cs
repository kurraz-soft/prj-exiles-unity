using UnityEngine;

public class DynamicObjectsManager : MonoBehaviour
{
    private static DynamicObjectsManager _instance;

    public static DynamicObjectsManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<DynamicObjectsManager>();
                if(!_instance)
                    Debug.LogError("Can't find DynamicObjectsManager");
            }

            return _instance;
        }
    }
}