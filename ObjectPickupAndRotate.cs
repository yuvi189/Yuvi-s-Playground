using UnityEngine;
using UnityEngine.UI;

public class ObjectPickupAndRotate : MonoBehaviour
{
    public Camera playerCamera;
    public LayerMask interactableObjectsLayer;
    public float interactionDistance = 2.0f;

    private Transform pickedContainer;
    private bool isCarrying = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isCarrying)
        {
            PickUpContainer();
        }
        else if (Input.GetKeyDown(KeyCode.E) && isCarrying)
        {
            DropContainer();
        }

        if (isCarrying)
        {
            RotatePickedContainer();
        }
    }

    private void PickUpContainer()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableObjectsLayer))
        {
            pickedContainer = hit.collider.transform; // Assumes the container's Collider is the one being hit.
            isCarrying = true;

            // Attach the container to the player.
            pickedContainer.SetParent(transform);
        }
    }

  private void DropContainer()
{
    if (pickedContainer != null)
    {
        // Disable the renderer and collider to make the container vanish.
        Renderer containerRenderer = pickedContainer.GetComponent<Renderer>();
        Collider containerCollider = pickedContainer.GetComponent<Collider>();

        containerRenderer.enabled = false;
        containerCollider.enabled = false;
        pickedContainer.SetParent(null);
        foreach (Transform child in pickedContainer)
            {
                child.SetParent(null);
            }
        pickedContainer.gameObject.layer = 31;

        // Reset the parent to remove it from the player's hierarchy.
        
        pickedContainer = null;
        isCarrying = false;
    }
}



    private void RotatePickedContainer()
    {
        float rotationSpeed = 50.0f; // Adjust the rotation speed as needed.

        // Rotate the picked container using arrow keys.
        float horizontalInput = Input.GetAxis("Horizontal");
        pickedContainer.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
    }
}
