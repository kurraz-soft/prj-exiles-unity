using buildings;
using UnityEngine;

public class BuilderController : MonoBehaviour
{
    public ConstructController MinePrefab = null;
    public ConstructController LumberjackHutPrefab = null;
    public ConstructController FishermanHutPrefab = null;
    public ConstructController StoragePrefab = null;
    public ConstructController CropFieldPrefab = null;

    /// <summary>
    /// Grid Cell Size
    /// </summary>
    public int cellSize = 2;

    private ConstructController _markConstruct;
    private ConstructController _selectedPrefab;

    public void BuildMine()
    {
        _selectedPrefab = MinePrefab;
        Build();
    }
    
    public void BuildLumberjackHut()
    {
        _selectedPrefab = LumberjackHutPrefab;
        Build();
    }
    
    public void BuildFishermanHut()
    {
        _selectedPrefab = FishermanHutPrefab;
        Build();
    }

    public void BuildCropField()
    {
        _selectedPrefab = CropFieldPrefab;
        Build();
    }

    private void Build()
    {
        _markConstruct = Instantiate(_selectedPrefab);
        _markConstruct.SetBuildState();
    }

    void Update()
    {
        if(!_markConstruct) return;

        ObjectFollowCursor();
        if (Input.GetMouseButton(1) || Input.GetAxis("Cancel") > 0)
        {
            Destroy(_markConstruct.gameObject);
        }
        else if (Input.GetMouseButton(0))
        {
            if (_markConstruct.construct.buildMark.CanPlace)
            {
                PlaceBuilding();

                Destroy(_markConstruct.gameObject);
            }
            else
            {
                UIController.Instance.ShowAlert("You can't build here");
            }
        }    
    }

    /// <summary>
    /// Sets buildplace for a selected building
    /// </summary>
    private void PlaceBuilding()
    {
        var b = Instantiate(_selectedPrefab);
        b.transform.parent = DynamicObjectsManager.Instance.transform;
        b.transform.position = _markConstruct.transform.position;
        b.construct.isConstructed = false;
        GameManager.Instance.BuildingManager.AddBuilding(b.construct);

        b.Init();
    }

    private void ObjectFollowCursor()
    {
        var ray  = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground", "Water")))
        {
            //var r = _markBuilding.Construct.BuildMark.GetComponent<Renderer>();

            var point = new Vector3(Mathf.Round(hit.point.x / cellSize) * cellSize, hit.point.y, Mathf.Round(hit.point.z / cellSize) * cellSize);
            point.y = 30;

            _markConstruct.transform.position = point;
        }
    }
}