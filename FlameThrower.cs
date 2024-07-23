using UnityEngine;
using UnityEngine.UI;

public class FlameThrower : MonoBehaviour
{
    public Camera playerCamera;
    public LayerMask interactableLayer;
    public Text interactionText;
    public ParticleSystem primaryParticleSystem;
    public ParticleSystem secondaryParticleSystem;
    public ParticleSystem tertiaryParticleSystem;

    private GameObject currentInteractable;
    private bool isObjectPickedUp = false;

    private void Start()
    {
        interactionText.gameObject.SetActive(false);
        StopParticleSystems();
    }

    private void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (!isObjectPickedUp)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
            {
                currentInteractable = hit.collider.gameObject;
                interactionText.text = "Press [E] to Pick Up";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpObject();
                }
            }
            else
            {
                currentInteractable = null;
                interactionText.gameObject.SetActive(false);
            }
        }
        else
        {
            interactionText.text = "Press [E] to Drop";

            if (Input.GetKeyDown(KeyCode.E))
            {
                DropObject();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ToggleParticleSystems();
            }
        }
    }

    private void PickUpObject()
    {
        Rigidbody objectRigidbody = currentInteractable.GetComponent<Rigidbody>();
        currentInteractable.transform.SetParent(playerCamera.transform);
        objectRigidbody.isKinematic = true;
        isObjectPickedUp = true;
    }

    private void DropObject()
    {
        Rigidbody objectRigidbody = currentInteractable.GetComponent<Rigidbody>();
        currentInteractable.transform.SetParent(null);
        objectRigidbody.isKinematic = false;
        isObjectPickedUp = false;
    }

    private void ToggleParticleSystems()
    {
        if (primaryParticleSystem.isPlaying || secondaryParticleSystem.isPlaying)
        {
            StopParticleSystems();
        }
        else
        {
            PlayParticleSystems();
        }
    }

    private void PlayParticleSystems()
    {
        var mainModulePrimary = primaryParticleSystem.main;
        mainModulePrimary.stopAction = ParticleSystemStopAction.None;
        primaryParticleSystem.Play();

        var mainModuleSecondary = secondaryParticleSystem.main;
        mainModuleSecondary.stopAction = ParticleSystemStopAction.None;
        secondaryParticleSystem.Play();

        var mainModuleTertiary = tertiaryParticleSystem.main;
        mainModuleTertiary.stopAction = ParticleSystemStopAction.Callback;
        tertiaryParticleSystem.Clear();
        tertiaryParticleSystem.Stop();
    }

    private void StopParticleSystems()
    {
        var mainModulePrimary = primaryParticleSystem.main;
        mainModulePrimary.stopAction = ParticleSystemStopAction.Callback;
        primaryParticleSystem.Clear();
        primaryParticleSystem.Stop();

        var mainModuleSecondary = secondaryParticleSystem.main;
        mainModuleSecondary.stopAction = ParticleSystemStopAction.Callback;
        secondaryParticleSystem.Clear();
        secondaryParticleSystem.Stop();

        var mainModuleTertiary = tertiaryParticleSystem.main;
        mainModuleTertiary.stopAction = ParticleSystemStopAction.None;
        tertiaryParticleSystem.Play();
    }
}
