using UnityEngine;
using UnityEngine.UI;

public class ChalkInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public LayerMask chalkLayer;
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.red;
    public Text pickupText;
    public Image crosshairImage; // Reference to the crosshair image.
 
    private GameObject chalkObject;
    private bool isChalkPickedUp = false;

    private Vector3 chalkLocalPosition; // Store the relative position of the chalk.

    private void Start()
    {
        crosshairImage.color = defaultColor;
        pickupText.gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);

        if (!isChalkPickedUp)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, chalkLayer))
            {
                chalkObject = hit.collider.gameObject;
                crosshairImage.color = highlightColor;
                pickupText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpChalk();
                }
            }
            else
            {
                crosshairImage.color = defaultColor;
                pickupText.gameObject.SetActive(false);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DropChalk();
            }
        }
    }

    private void PickUpChalk()
    {
        chalkLocalPosition = chalkObject.transform.position - playerCamera.transform.position;
        chalkObject.transform.SetParent(playerCamera.transform);
        isChalkPickedUp = true;
        pickupText.text = "Press [E] to Drop";
    }

    private void DropChalk()
    {
        chalkObject.transform.SetParent(null);
        chalkObject.transform.position = playerCamera.transform.position + chalkLocalPosition;
        isChalkPickedUp = false;
        pickupText.text = "Press [E] to Pick Up";
    }
}
