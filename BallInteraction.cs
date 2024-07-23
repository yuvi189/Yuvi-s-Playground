using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BallInteraction : MonoBehaviour
{
    // Projectile
    public Rigidbody ball;
    public Transform target;
    public float h = 25;
    public float gravity = -18;
    public bool debugPath = true;

    // Interaction
    public Camera playerCamera;
    public LayerMask chalkLayer;
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.red;
    public Text pickupText;
    public Image crosshairImage;
    public GameObject trajectoryMarkerPrefab;
    public AudioSource throwSound; // Added AudioSource for sound effect
    private float markerLifeTime = 2.0f;
    private Transform currentMarker;

    private GameObject chalkObject;
    private bool isChalkPickedUp = false;

    private void Start()
    {
        ball.useGravity = false;
        crosshairImage.color = defaultColor;
        pickupText.gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);

        if (!isChalkPickedUp)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, chalkLayer))
            {
                chalkObject = hit.collider.gameObject;
                crosshairImage.color = highlightColor;
                pickupText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpChalk();
                }
            }
            else
            {
                crosshairImage.color = defaultColor;
                pickupText.gameObject.SetActive(false);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DropChalk();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Launch();
            }
        }
    }

    private void PickUpChalk()
    {
        Rigidbody chalkRigidbody = chalkObject.GetComponent<Rigidbody>();
        chalkObject.transform.SetParent(playerCamera.transform);
        chalkRigidbody.isKinematic = true; // Disable isKinematic to move along with the camera.
        pickupText.text = "Press [E] to Drop";
        isChalkPickedUp = true;
    }

    private void DropChalk()
    {
        Rigidbody chalkRigidbody = chalkObject.GetComponent<Rigidbody>();
        chalkObject.transform.SetParent(null);
        chalkRigidbody.isKinematic = false;
        pickupText.text = "Press [E] to Pick Up";
        isChalkPickedUp = false;
    }

    private void Launch()
    {
        if (isChalkPickedUp)
        {
            DropChalk();
        }
        Physics.gravity = Vector3.up * gravity;
        ball.useGravity = true;
        ball.velocity = CalculateLaunchData().initialVelocity;
        StartCoroutine(CreateTrajectoryMarkers());

        // Play the throw sound effect
        if (throwSound != null)
        {
            throwSound.Play();
        }
    }

    private LaunchData CalculateLaunchData()
    {
        float displacementY = target.position.y - ball.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - ball.position.x, 0, target.position.z - ball.position.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    private struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }

    IEnumerator CreateTrajectoryMarkers()
    {
        LaunchData launchData = CalculateLaunchData();
        Vector3 previousDrawPoint = ball.position;

        int resolution = 30;
        for (int i = 1; i <= resolution; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = ball.position + displacement;

            // Instantiate the trajectory marker and set its position.
            currentMarker = Instantiate(trajectoryMarkerPrefab, previousDrawPoint, Quaternion.identity).transform;

            // Schedule the marker to be destroyed after markerLifeTime.
            Destroy(currentMarker.gameObject, markerLifeTime);

            previousDrawPoint = drawPoint;

            // Wait for a short time before creating the next marker to create the "temporary" effect.
            yield return new WaitForSeconds(markerLifeTime / resolution);
        }
    }
}
