using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamReaction : MonoBehaviour
{
    private List<Transform> enteredMoleculeParents = new List<Transform>();
    private List<string> chemicalsInBeaker = new List<string>();
    private bool isBuffering = false;
    public Color changeColor = Color.yellow;

    // Reference to the independent empty object with invisible children
    public GameObject independentEmptyObject;

    // Reference to the particle system prefab
    public ParticleSystem foamParticleSystemPrefab;

    // Reference to the audio clip
    public AudioClip reactionSoundClip;

    // AudioSource for playing the sound
    private AudioSource audioSource;

    private void Start()
    {
        // Initialize the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = reactionSoundClip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isBuffering)
            return;

        ChemicalMolecule molecule = other.GetComponent<ChemicalMolecule>();
        if (molecule != null)
        {
            string chemicalName = molecule.chemicalName;

            // Check if the chemical is not already in the beaker.
            if (!chemicalsInBeaker.Contains(chemicalName))
            {
                chemicalsInBeaker.Add(chemicalName);
                enteredMoleculeParents.Add(molecule.transform.parent);

                // Play the particle system and sound at the beginning of the reaction
                

                // Start buffering for the reaction.
                StartCoroutine(BufferTimer());

                // Output a message when a new chemical is added.
                Debug.Log("New chemical added: " + chemicalName);
            }
        }
    }

    private IEnumerator BufferTimer()
    {
        isBuffering = true;
        yield return new WaitForSeconds(5f); // Adjust the time as needed.
        PerformReaction();
        isBuffering = false;

        // Reset the beaker for the next reaction
        ResetBeaker();
    }

    private void PerformReaction()
    {
        // List of required chemicals for the reaction.
        List<string> requiredChemicals = new List<string>
        {
            "h2o2"
            // Add other required chemical names as needed.
        };

        bool allRequiredChemicalsPresent = true;

        foreach (string requiredChemical in requiredChemicals)
        {
            if (!chemicalsInBeaker.Contains(requiredChemical))
            {
                allRequiredChemicalsPresent = false;
                break;
            }
        }

        if (allRequiredChemicalsPresent)
        {
            PlayParticleSystem();
            PlayReactionSound();
            // Make the independent empty object's children visible with a delay
            if (independentEmptyObject != null)
            {
                StartCoroutine(MakeChildrenVisibleThenInvisible(independentEmptyObject));
            }
            DeleteSphericalMolecules(enteredMoleculeParents);
            Debug.Log("Flaming Reaction!");
        }
    }

    private IEnumerator MakeChildrenVisibleThenInvisible(GameObject parentObject)
    {
        Renderer[] childRenderers = parentObject.GetComponentsInChildren<Renderer>();
        float delay = 0.05f; // Adjust the delay as needed (0.25 seconds in this case).

        // Make children visible
        foreach (Renderer childRenderer in childRenderers)
        {
            childRenderer.enabled = true;
            yield return new WaitForSeconds(delay);
        }

        // Wait for 3 seconds after all children became visible
        yield return new WaitForSeconds(3f);

        // Make children invisible
        foreach (Renderer childRenderer in childRenderers)
        {
            childRenderer.enabled = false;
        }
    }

    private void DeleteSphericalMolecules(List<Transform> moleculeParents)
    {
        foreach (var parent in enteredMoleculeParents)
        {
            // Find all spherical molecules (children) of the parent and destroy them.
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }

            // Optionally, you can also destroy the parent object itself.
            Destroy(parent.gameObject);
        }
    }

    private void PlayParticleSystem()
    {
        // Play the beaker particle system at the beginning of the reaction
        if (foamParticleSystemPrefab != null)
        {
            // Instantiate the particle system and get its main module
            ParticleSystem particleSystemInstance = Instantiate(foamParticleSystemPrefab, transform.position, Quaternion.identity);
            ParticleSystem.MainModule mainModule = particleSystemInstance.main;

            // Ensure the particle system will destroy itself after playing
            mainModule.stopAction = ParticleSystemStopAction.Destroy;

            // Play the particle system
            particleSystemInstance.Play();
        }
    }

    private void PlayReactionSound()
    {
        // Play the reaction sound
        if (audioSource != null && reactionSoundClip != null)
        {
            audioSource.PlayOneShot(reactionSoundClip);
        }
    }

    private void ResetBeaker()
    {
        // Clear the entered chemicals for the next reaction
        chemicalsInBeaker.Clear();
        enteredMoleculeParents.Clear();
    }
}
