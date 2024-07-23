using UnityEngine;

public class ChemicalMolecule : MonoBehaviour
{
    public string chemicalName { get; private set; }

    private void Start()
    {
        Transform parent = transform.parent;
        // Get the chemical name from the GameObject's name.
        chemicalName = parent.name;
    }
}
