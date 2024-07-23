using UnityEngine;
using UnityEngine.UI;

public class CrosshairAndInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public LayerMask interactableObjectsLayer;
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.green;
    public float interactionDistance = 2.0f;

    private Image crosshairImage;
    private GameObject carriedObject;
    private bool isCarrying = false;

    private void Start()
    {
        crosshairImage = GetComponent<Image>();
    }

    private void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableObjectsLayer))
        {
            GameObject interactedObject = hit.collider.gameObject;
            crosshairImage.color = highlightColor;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isCarrying)
                {
                    carriedObject = interactedObject;
                    isCarrying = true;
                    carriedObject.transform.SetParent(transform);
                    carriedObject.GetComponent<Rigidbody>().isKinematic = true;
                }
                else if (carriedObject == interactedObject)
                {
                    carriedObject.transform.SetParent(null);
                    carriedObject.GetComponent<Rigidbody>().isKinematic = false;
                    carriedObject = null;
                    isCarrying = false;
                }
            }
        }
        else
        {
            crosshairImage.color = defaultColor;
        }

        if (isCarrying)
        {
            float rotationSpeed = 30.0f;
            float horizontalInput = Input.GetAxis("Horizontal");

            carriedObject.transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        }
    }
}
