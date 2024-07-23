using UnityEngine;

public class ChalkDrawing : MonoBehaviour
{
    public GameObject chalkMarkerPrefab;
    public Camera playerCamera;
    public LayerMask chalkboardLayer;
    public AudioSource chalkSound;

    private bool isDrawing = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = true;
        }

        if (isDrawing)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, chalkboardLayer))
                {
                    DrawOnChalkboard(hit.point);

                    // Play sound effect only when drawing on the chalkboard layer
                    PlayChalkSound();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDrawing = false;
            }
        }
    }

    private void DrawOnChalkboard(Vector3 position)
    {
        Instantiate(chalkMarkerPrefab, position, Quaternion.identity);
    }

    private void PlayChalkSound()
    {
        // Play the chalk sound effect
        if (chalkSound != null)
        {
            chalkSound.Play();
        }
    }
}
