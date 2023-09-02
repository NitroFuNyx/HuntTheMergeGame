using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class GrabController : MonoBehaviour
{
    [Header("Internal references")]
    [Space]
    [SerializeField] private Camera mainCamera;

    [Header("External references")]
    [Space]
    [SerializeField] private TileInteractionHandler tileInteractionHandler;
    
    [SerializeField]private Vector3 selectedObjectLastPosition;
    [SerializeField] private TileHolder selectedObjectLastTile;

    private Animal selectedObject;

    [Header("Parameters")] 
    [Space] 
    [SerializeField] private float offsetY = .25f;

    [SerializeField] private LayerMask animalLayer;
    [SerializeField] private LayerMask dropZoneLayer  ;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        animalLayer  = LayerMask.GetMask("Animals");     
        dropZoneLayer  = LayerMask.GetMask("Drop Zone"); 
    }
    
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
                GrabAnimal();
                    InteractWithTile();
        }

        if(selectedObject != null) {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, offsetY, worldPosition.z);
        
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (selectedObject == null) return;
            RaycastHit dropZoneHit = CastRay(dropZoneLayer);

            if (dropZoneHit.collider!= null)
            {
                if (dropZoneHit.collider.TryGetComponent(out TileHolder item))
                {
                   PutAnimal(item,dropZoneHit.transform.position);
                }

            }
            if(selectedObjectLastPosition!=Vector3.zero&&selectedObject!=null)
            {
                selectedObject.transform.position = selectedObjectLastPosition;
                tileInteractionHandler.OccupyTile(selectedObjectLastTile,true);
                selectedObject.ActivateOutline(false);
                selectedObjectLastTile = null;
                selectedObject = null;
                selectedObjectLastPosition=Vector3.zero;
            }



        }

        
    }
    
    private void GrabAnimal()
    {
        if (selectedObject != null) return;
        RaycastHit hit = CastRay(animalLayer);
        RaycastHit dropZoneHit = CastRay(dropZoneLayer);

        if (hit.collider != null)
        {
            if (!hit.collider.CompareTag("Drag"))
            {
                return ;
            }
            if (hit.collider.TryGetComponent(out Animal item))
            {
                selectedObject = item;
                selectedObject.ActivateOutline(true);
            }
            selectedObjectLastPosition = hit.collider.transform.position;
        }

        if (dropZoneHit.collider != null)
        {
            if (dropZoneHit.collider.TryGetComponent(out TileHolder item))
            {
                tileInteractionHandler.OccupyTile(item,false);
                selectedObjectLastTile = item;
            }
        }

    }private void InteractWithTile()
    {
        if (selectedObject != null)return ;
        RaycastHit hit = CastRay(dropZoneLayer);

        if (hit.collider != null)
        {

            if (!hit.collider.CompareTag("DropZone"))
            {
                return;
            }

            if (hit.collider.TryGetComponent(out TileHolder item))
            {
                tileInteractionHandler.BuyTile(item);

            }
        }

    }
    private void PutAnimal(TileHolder item, Vector3 hit)
    {
        if (item.MViewModel.IsLocked) return;
        if (!item.MViewModel.IsOccupied)
        {
            selectedObject.transform.position = new Vector3(hit.x, hit.y, hit.z);
            selectedObject.ActivateOutline(false);
            selectedObject = null; 
            tileInteractionHandler.OccupyTile(item,true);
                        
        }
    }
    private RaycastHit CastRay(LayerMask layer) {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit,10f,layer);
        Debug.DrawRay(worldMousePosNear, worldMousePosFar - worldMousePosNear,Color.green,10f);

        return hit;
    }
}