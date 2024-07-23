using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Camera playerCamera;
    public LayerMask interactableObjectsLayer;
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.red;
    public Text chemicalNameText;
    public Text pickupText; // Reference to the "Press E to Pick Up" text.

    private Image crosshairImage;
    private bool isObjectDetected = false;

    private void Start()
    {
        crosshairImage = GetComponent<Image>();
    }

    private void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableObjectsLayer))
        {
            GameObject interactedObject = hit.collider.gameObject;
            crosshairImage.color = highlightColor;

            // Get the name of the detected child object providing the collision.
            string childObjectName = interactedObject.transform.GetChild(0).name;
            chemicalNameText.text = "Chemical Name: " + childObjectName;
            Debug.Log("Detected Object Name: " + childObjectName);
            isObjectDetected = true;
        }
        else
        {
            crosshairImage.color = defaultColor;
            chemicalNameText.text = "";
            isObjectDetected = false;
        }

        // Show/hide the "Press E to Pick Up" text based on object detection.
        pickupText.gameObject.SetActive(isObjectDetected);
    }
}
