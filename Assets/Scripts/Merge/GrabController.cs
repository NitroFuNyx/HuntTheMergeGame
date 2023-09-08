using UnityEngine;

public class GrabController : MonoBehaviour
{
    [Header("Internal references")]
    [Space]
    [SerializeField]
    private Camera mainCamera;

    [Header("External references")] 
    [Space] 
    [SerializeField]
    private TileInteractionHandler tileInteractionHandler;

    [SerializeField] private MergeManager mergeManager;

    [SerializeField] private Vector3 selectedObjectLastPosition;
    [SerializeField] private TileHolder selectedObjectLastTile;

    private Animal selectedObject;

    [Header("Parameters")]
    [Space] 
    [SerializeField]
    private float offsetY = .25f;

    [SerializeField] private LayerMask animalLayer;
    [SerializeField] private LayerMask dropZoneLayer;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        animalLayer = LayerMask.GetMask("Animals");
        dropZoneLayer = LayerMask.GetMask("Drop Zone");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GrabAnimal();
            InteractWithTile();
        }

        if (selectedObject != null)
        {
            var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            var worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, offsetY, worldPosition.z);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedObject == null) return;
            var dropZoneHit = CastRay(dropZoneLayer);

            if (dropZoneHit.collider != null)
                if (dropZoneHit.collider.TryGetComponent(out TileHolder item))
                    PutAnimal(item, dropZoneHit.transform.position);

            if (selectedObjectLastPosition != Vector3.zero && selectedObject != null)
            {
                selectedObject.transform.position = selectedObjectLastPosition;
                selectedObject.ActivateCollider(true);
                selectedObject.ActivateOutline(false);
                tileInteractionHandler.OccupyTile(selectedObjectLastTile, true, selectedObject.AnimalData.level);
                selectedObject = null;
                selectedObjectLastPosition = Vector3.zero;
            }
        }
    }

    private void GrabAnimal()
    {
        if (selectedObject != null) return;
        var hit = CastRay(animalLayer);
        var dropZoneHit = CastRay(dropZoneLayer);

        if (hit.collider != null)
        {
            if (!hit.collider.CompareTag("Drag")) return;

            if (hit.collider.TryGetComponent(out Animal item))
            {
                selectedObject = item;
                selectedObject.ActivateOutline(true);
                item.ActivateCollider(false);
            }

            if (dropZoneHit.collider != null)
                if (dropZoneHit.collider.TryGetComponent(out TileHolder tileItem))
                {
                    tileInteractionHandler.OccupyTile(tileItem, false);
                    selectedObjectLastTile = tileItem;
                }

            selectedObjectLastPosition = hit.collider.transform.position;
        }
    }

    private void PutAnimal(TileHolder item, Vector3 hit)
    {
        if (item.MViewModel.IsLocked)
        {
            tileInteractionHandler.OccupyTile(selectedObjectLastTile, true, selectedObject.AnimalData.level);

            return;
        }

        if (!item.MViewModel.IsOccupied)
        {
            selectedObject.transform.position = new Vector3(hit.x, hit.y, hit.z);
            selectedObject.ActivateOutline(false);
            selectedObject.ActivateCollider(true);

            tileInteractionHandler.OccupyTile(item, true, selectedObject.AnimalData.level);
            selectedObject = null;
        }
        else
        {
            var hitAnimal = CastRay(animalLayer);

            if (hitAnimal.collider != null)
            {
                if (!hitAnimal.collider.CompareTag("Drag")) return;

                if (hitAnimal.collider.TryGetComponent(out Animal animalItem))
                {
                    if (mergeManager.MergeAnimals(item, selectedObject, animalItem))
                    {
                        selectedObject.transform.position = selectedObjectLastPosition;
                        tileInteractionHandler.OccupyTile(selectedObjectLastTile, true,
                            selectedObject.AnimalData.level);
                    }
                    else
                    {
                        tileInteractionHandler.OccupyTile(selectedObjectLastTile, false);
                        selectedObjectLastTile = null;
                    }
                }
            }
        }
    }

    private void InteractWithTile()
    {
        if (selectedObject != null) return;
        var hit = CastRay(dropZoneLayer);

        if (hit.collider != null)
        {
            if (!hit.collider.CompareTag("DropZone")) return;

            if (hit.collider.TryGetComponent(out TileHolder item)) tileInteractionHandler.BuyTile(item);
        }
    }

    private RaycastHit CastRay(LayerMask layer)
    {
        var screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        var screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        var worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        var worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, 10f, layer);
        Debug.DrawRay(worldMousePosNear, worldMousePosFar - worldMousePosNear, Color.green, 10f);

        return hit;
    }
}