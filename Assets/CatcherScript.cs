using UnityEngine;

public class Catchercript : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Destroy the colliding object
        Destroy(collision.gameObject);

        // Find the ship and update resources
        var ship = GameObject.FindWithTag("Player");
        if (ship != null)
        {
            var shipScript = ship.GetComponent<ShipScript>();
            if (shipScript != null)
            {
                shipScript.CollectResources(1);
            }
        }
    }
}
