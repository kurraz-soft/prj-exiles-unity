using System;
using System.Linq;
using buildings;
using items;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    const int MAX_TIME_SCALE = 5;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if(!_instance)
                    Debug.LogError("Can't find a GameController");
            }
            return _instance;
        }
    }

    public WorkManager WorkManager
    {
        get { return GetComponent<WorkManager>(); }
    }

    private buildings.Manager _buildingManager;
    private peasants.Manager _peasantManager;

    public buildings.Manager BuildingManager
    {
        get { return _buildingManager; }
    }
    public peasants.Manager PeasantManager
    {
        get { return _peasantManager; }
    }

    public void TimeScaleUp()
    {
        Time.timeScale = Math.Min(Time.timeScale + 1, MAX_TIME_SCALE);

        GameObject.Find("TimeScaleLabel").GetComponent<Text>().text = "x" + Time.timeScale;
    }

    public void TimeScaleDown()
    {
        Time.timeScale = Math.Max(Time.timeScale - 1, 1);

        GameObject.Find("TimeScaleLabel").GetComponent<Text>().text = "x" + Time.timeScale;
    }

    public ItemList GetAllStorageResources()
    {
        var items = new ItemList();

        foreach (var s in BuildingManager.buildings.OfType<Storage>())
        {
            items.Merge(s.items);
        }

        return items;
    }

    void Awake()
    {
        _buildingManager = GetComponent<buildings.Manager>();
        _peasantManager = GetComponent<peasants.Manager>();
    }
}
