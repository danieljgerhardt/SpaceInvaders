using UnityEngine;
public class AlienScript : MonoBehaviour
{
    public float speed = 2f;           // Movement speed
    public float moveDistance = 3f;    // How far from the start position to move left/right

    private float startX;
    private float leftLimit;
    private float rightLimit;
    public int direction = 1;         // 1 = right, -1 = left
    private bool isHit = false;

    void Start()
    {
        startX = transform.position.x;
        // Calculate relative limits
        leftLimit = startX - moveDistance;
        rightLimit = startX + moveDistance;
    }

    void FixedUpdate()
    {
        if (isHit)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                //rb.linearVelocity += new Vector3(0, 0, -9.68f * Time.fixedDeltaTime);

                rb.AddForce(new Vector3(0, 0, -9.68f), ForceMode.Acceleration);
            }
        }
        else
        {
            transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
        }
    }
    public void Shoot(GameObject bulletPrefab)
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(
                bulletPrefab,
                transform.position + new Vector3(0, 0, -1f), // Adjust direction as needed
                Quaternion.identity
            );
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector3(0, 0, -10f); // Shoot downward
            }
        }
    }

    public void setHit()
    {
        isHit = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.rotation = Quaternion.identity;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            rb.AddForce(new Vector3(0, 0, -9.68f), ForceMode.Acceleration);
        }
    }

    public bool getIsHit() { return isHit; }
}
