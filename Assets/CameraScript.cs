using UnityEngine;
public class CameraScript : MonoBehaviour
{
    public bool firstPersonView = false; // Toggle in Inspector or via code
    public Transform shipTransform;      // Assign the ship's transform in Inspector

    Vector3 defaultLocation = new Vector3(0, 26, 3.10483e-06f);
    Vector3 firstPersonOffset = new Vector3(0, 1.5f, 0.5f); // Adjust as needed

    void Start()
    {
        // Optionally, auto-find the ship if not set
        if (shipTransform == null)
        {
            var ship = GameObject.FindWithTag("Player");
            if (ship != null)
                shipTransform = ship.transform;
        }
    }

    void Update()
    {
        if (firstPersonView && shipTransform != null)
        {
            // Set camera to ship's position + offset, and match rotation
            transform.position = shipTransform.position + shipTransform.TransformVector(firstPersonOffset);
            transform.rotation = shipTransform.rotation;
        }
        else
        {
            // Set to default location and rotation
            transform.position = defaultLocation;
            transform.rotation = Quaternion.Euler(90, 0, 0); // Overhead view, adjust as needed
        }
    }
}
