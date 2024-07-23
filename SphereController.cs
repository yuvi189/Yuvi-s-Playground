using UnityEngine;
using UnityEngine.UI;

public class SphereController : MonoBehaviour
{
    private bool isMouseDragging;
    private float spherePlaneDistance;
    private Vector3 offset;
    public Text messageText;
    private int sphereLayerMask;

    public ParticleSystem particleSystem;
    public AudioSource soundEffect;
    public AudioSource soundEffect2;
    private void Start()
    {
        sphereLayerMask = 1 << LayerMask.NameToLayer("Cradle");
    }

    private void OnMouseDown()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, sphereLayerMask))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isMouseDragging = true;
                spherePlaneDistance = Vector3.Dot(transform.position - Camera.main.transform.position, Camera.main.transform.forward);
                offset = transform.position - GetMouseWorldPos();
            }
        }
    }

    private void OnMouseUp()
    {
        isMouseDragging = false;
        HideMessage();
    }

    private void DisplayMessage(string text)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = text;
    }

    private void HideMessage()
    {
        messageText.gameObject.SetActive(false);
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = spherePlaneDistance;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void Update()
    {
        if (isMouseDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    private void OnCollisionEnter(Collision collision)
{
    // Check if the collision involves a sphere or a plank
    if (collision.gameObject.CompareTag("Sphere") || collision.gameObject.CompareTag("Plank"))
    {
        // Play collision sound effect
        soundEffect.Play();
    }
    else if(collision.gameObject.CompareTag("Reward"))
    {
        soundEffect2.Play();
    }
}

}
