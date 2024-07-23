using UnityEngine;
using UnityEngine.UI;

public class NewtonsCradleHighlight : MonoBehaviour
{
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.red;
    public Text interactionText; // Reference to the interaction text.
    public LayerMask interactableObjectsLayer; // Define the interactable objects layer.

    private Image crosshairImage;

    private void Start()
    {
        crosshairImage = GetComponent<Image>();
    }

    private void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableObjectsLayer))
        {
            GameObject interactedObject = hit.collider.gameObject;
            
            if (interactedObject.CompareTag("NewtonsCradle"))
            {
                crosshairImage.color = highlightColor;
                interactionText.text = "Click the balls to begin simulation";
            }
            else
            {
                crosshairImage.color = defaultColor;
                interactionText.text = "";
            }
        }
        else
        {
            crosshairImage.color = defaultColor;
            interactionText.text = "";
        }
    }
}
