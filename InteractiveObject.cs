using UnityEngine;
using UnityEngine.UI;

public class InteractiveObject : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public Text messageText;
    public AudioSource soundEffect;
    public float interactionDistance = 2f;

    private bool isParticlePlaying = false;

    void Update()
    {
        // Check if the player is looking at the object
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                // Display start or stop message based on particle system state
                messageText.text = isParticlePlaying ? "Press Space Bar to Stop" : "Press Space Bar to Start";

                // Check for space bar input
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Toggle particle system
                    ToggleParticleSystem();
                }
            }
            else
            {
                // Player is not looking at the object, reset the text
                messageText.text = "";
            }
        }
        else
        {
            // Player is not looking at the object, reset the text
            messageText.text = "";
        }
    }

    void ToggleParticleSystem()
    {
        if (isParticlePlaying)
        {
            // Stop particle system
            particleSystem.Stop();
            soundEffect.Stop(); // Stop sound effect
        }
        else
        {
            // Start particle system
            particleSystem.Play();
            soundEffect.Play(); // Play sound effect
        }

        // Toggle the state
        isParticlePlaying = !isParticlePlaying;
    }
}
