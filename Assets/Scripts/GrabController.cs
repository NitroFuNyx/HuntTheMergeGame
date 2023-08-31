using UnityEngine;

public class GrabController : MonoBehaviour
{
    [Header("Internal references")]
    [Space]
    [SerializeField] private Camera mainCamera;

    [SerializeField]private Vector3 selectedObjectLastPosition;

    private GameObject selectedObject;

    [Header("Parameters")] 
    [Space] 
    [SerializeField] private float offsetY = .25f;

    [SerializeField] private LayerMask rayLayer ;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        rayLayer = LayerMask.GetMask("Animals");
    }
    
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if(selectedObject == null) {
                RaycastHit hit = CastRay();

                if(hit.collider != null) {
                    Debug.Log(hit.collider.gameObject.name,hit.collider.gameObject);

                    if (!hit.collider.CompareTag("Drag")) {
                        return;
                    }

                    selectedObjectLastPosition = hit.collider.transform.position;
                    selectedObject = hit.collider.gameObject;
                    rayLayer = LayerMask.GetMask("Drop Zone");
                }
            }
           
        }

        if(selectedObject != null) {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, offsetY, worldPosition.z);

        }
        if (Input.GetMouseButtonUp(0))
        {
            if (selectedObject == null) return;
            RaycastHit dropZoneHit = CastRay();

            if (dropZoneHit.collider!= null)
            {
                Debug.Log(dropZoneHit.collider.gameObject.name,dropZoneHit.collider.gameObject);
                
                selectedObject.transform.position = new Vector3(dropZoneHit.transform.position.x, dropZoneHit.transform.position.y, dropZoneHit.transform.position.z);

                selectedObject = null;
            }
            else
            {
                selectedObject.transform.position = selectedObjectLastPosition;
                selectedObject = null;


            }
            rayLayer = LayerMask.GetMask("Animals");

        }

        
    }

    private RaycastHit CastRay() {
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
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit,10f,rayLayer);
        Debug.DrawRay(worldMousePosNear, worldMousePosFar - worldMousePosNear,Color.green,10f);

        return hit;
    }
}