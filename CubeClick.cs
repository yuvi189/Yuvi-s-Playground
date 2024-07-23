using UnityEngine;

public class CubeClick : MonoBehaviour
{
    public AudioClip soundClip;  // Drag your audio clip here in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundClip;
    }

    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast to check if the mouse is over the cube
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                // If the cube is clicked, play the sound
                audioSource.Play();
            }
        }
    }
}
