using UnityEngine;
using UnityEngine.UI;

public class RocketLaunch : MonoBehaviour
{
    public Rigidbody rocketRigidbody;
    public ParticleSystem smokeParticleSystem;
    public Text launchText;
    public float acceleration = 10f;
    public float rotationForce = 5f;
    public float sidewaysForce = 1f; // Adjust the sideways force
    public float disappearanceHeight = 50f;
    public AudioSource launchSound; // Added AudioSource for sound effect

    private bool isLaunched = false;

    private void Start()
    {
        // Disable gravity for the initial state
        if (rocketRigidbody != null)
        {
            rocketRigidbody.useGravity = false;
        }
    }

    private void Update()
    {
        if (!isLaunched)
        {
            // Check if the player is looking at the rocket
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // Display launch text
                    if (launchText != null)
                    {
                        launchText.text = "Press SpaceBar to Launch";
                        launchText.gameObject.SetActive(true);
                    }

                    // Check for space bar press to launch the rocket and play sound effect
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        LaunchRocket();
                        PlayLaunchSound();
                    }
                }
                else
                {
                    // Hide launch text if not looking at the rocket
                    if (launchText != null)
                    {
                        launchText.gameObject.SetActive(false);
                    }
                }
            }
        }

        if (isLaunched && transform.position.y >= disappearanceHeight)
        {
            // Disappear when the rocket reaches the specified height
            Disappear();
        }

        // Move the smoke particle system along with the rocket
        if (smokeParticleSystem != null)
        {
            smokeParticleSystem.transform.position = rocketRigidbody.transform.position;
        }
    }

    private void LaunchRocket()
    {
        isLaunched = true;

        // Enable the smoke particle system
        if (smokeParticleSystem != null)
        {
            smokeParticleSystem.Play();
        }

        // Enable gravity and apply initial force and rotation to the rocket
        if (rocketRigidbody != null)
        {
            rocketRigidbody.useGravity = true;
            rocketRigidbody.AddForce(Vector3.up * acceleration, ForceMode.Impulse);
            rocketRigidbody.AddTorque(Vector3.forward * rotationForce, ForceMode.Impulse);
            rocketRigidbody.AddForce(Vector3.right * sidewaysForce, ForceMode.Impulse); // Apply sideways force
        }

        // Hide launch text
        if (launchText != null)
        {
            launchText.gameObject.SetActive(false);
        }
    }

    private void Disappear()
    {
        // Disable the rocket and particle system
        if (rocketRigidbody != null)
        {
            rocketRigidbody.gameObject.SetActive(false);
        }

        if (smokeParticleSystem != null)
        {
            smokeParticleSystem.gameObject.SetActive(false);
        }
    }

    private void PlayLaunchSound()
    {
        // Play the launch sound effect
        if (launchSound != null)
        {
            launchSound.Play();
        }
    }
}
