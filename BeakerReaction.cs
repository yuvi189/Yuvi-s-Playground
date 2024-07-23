using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakerReaction : MonoBehaviour
{
    private List<string> chemicalsInBeaker = new List<string>();
    private bool isBuffering = false;
    public Color changeColor = Color.yellow;
    private List<Transform> enteredMoleculeParents = new List<Transform>();

    // Reference to the particle system prefab
    public ParticleSystem beakerParticleSystemPrefab;
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
    }

    private void PerformReaction()
    {
        // List of required parent objects for the reaction.
        List<string> requiredChemicals = new List<string>
        {
            "Na(Sodium Metal)",
            "H2o"
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
            StartCoroutine(ChangeChildrenColorsWithDelay(enteredMoleculeParents, changeColor, 0.1f));
        }
    }

    private IEnumerator ChangeChildrenColorsWithDelay(List<Transform> moleculeParents, Color newColor, float delay)
    {
        // Create a copy of the list to avoid modification during iteration
        List<Transform> parentsCopy = new List<Transform>(moleculeParents);

        foreach (var parent in parentsCopy)
        {
            // Check if the parent object is null or destroyed
            if (parent == null)
                continue;

            // Change the color of all child spheres of the parent.
            foreach (Renderer childRenderer in parent.GetComponentsInChildren<Renderer>())
            {
                // Check if the childRenderer is null or destroyed
                if (childRenderer == null)
                    continue;

                Material newMaterial = new Material(childRenderer.material);
                newMaterial.color = newColor;
                childRenderer.material = newMaterial;

                // Add a delay before changing the next child's color.
                yield return new WaitForSeconds(delay);
            }
        }

        // After changing colors, delete the spherical molecules
        StartCoroutine(DeleteSphericalMolecules(enteredMoleculeParents));
    }

    private IEnumerator DeleteSphericalMolecules(List<Transform> moleculeParents)
    {
        // Create a copy of the list to avoid modification during iteration
        List<Transform> parentsCopy = new List<Transform>(moleculeParents);

        yield return new WaitForSeconds(1f); // Adjust the time as needed.

        foreach (var parent in parentsCopy)
        {
            // Check if the parent object is null or destroyed
            if (parent == null)
                continue;

            // Find all spherical molecules (children) of the parent and destroy them.
            List<Transform> childrenCopy = new List<Transform>();
            foreach (Transform child in parent)
            {
                if (child != null)
                {
                    childrenCopy.Add(child);
                }
            }

            foreach (Transform child in childrenCopy)
            {
                Destroy(child.gameObject);
            }

            // Optionally, you can also destroy the parent object itself.
            if (parent != null)
            {
                Destroy(parent.gameObject);
            }
        }

        // Reset the beaker for the next reaction
        ResetBeaker();
    }

    private void PlayParticleSystem()
    {
        // Play the beaker particle system at the beginning of the reaction
        if (beakerParticleSystemPrefab != null)
        {
            // Instantiate the particle system and get its main module
            ParticleSystem particleSystemInstance = Instantiate(beakerParticleSystemPrefab, transform.position, Quaternion.identity);
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
