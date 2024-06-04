using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    // declares a public static property named Instance of type InteractionManager. 
    // Static properties belong to the class itself rather than to instances of the class. 
    // This property is designed to hold a reference to the single instance of the InteractionManager class throughout the game.
    public static InteractionManager Instance { get; set; }  

    public AmmoBox hoveredAmmoPack = null;

        // Unity method that is called when the script instance is being loaded.
        private void Awake()
        {
            // (Singleton pattern) enforce that only one instance of a class exists at any given time.
            if (Instance != null && Instance != this)
            {
                // prevents duplicate instances of the InteractionManager from coexisting in the scene.
                Destroy(gameObject);
            }
            else
            { 
                // sets the Instance property to the current instance of the InteractionManager class.
                Instance = this;
            }
        }

        private void Update()
        {
            // Check if the human character is within a specific range of the ammo box
            float maxRange = 3f; // Adjust the range as needed
            // Assuming your character is a GameObject named "Player"
            GameObject Player = GameObject.Find("Player"); 
            Vector3 playerPosition = Player.transform.position;
            // Creates a ray originating from the center of the screen
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            // checks if the ray intersects with any colliders in the scene 
            // If it does, the information about the intersection is stored in the hit variable.
            if (Physics.Raycast(ray, out hit))
            {
                // retrieves the GameObject that was hit by the raycast.
                GameObject objectHitByRaycast = hit.transform.gameObject;
                // retrieves the position of the object hit by the raycast i-e of ammobox
                Vector3 ammoBoxPosition = objectHitByRaycast.transform.position;
                float distanceToAmmoBox = Vector3.Distance(playerPosition, ammoBoxPosition);

                // Proceed only if the player is within the specified range
                if (distanceToAmmoBox <= maxRange)
                {
                    // Get components of object which is hit by ray cast
                    if (objectHitByRaycast.GetComponent<AmmoBox>())
                    {
                        // Enable the outline when the raycast hits the AmmoBox object
                        hoveredAmmoPack = objectHitByRaycast.GetComponent<AmmoBox>();
                        hoveredAmmoPack.GetComponent<Outline>().enabled = true;
                        // If f is pressed
                        if (Input.GetKeyDown(KeyCode.F)) 
                        {
                            // called a method named PickupAmmoPack() from the WeaponManager class through its Instance.
                            WeaponManager.Instance.PickupAmmoPack(hoveredAmmoPack);
                            Destroy(objectHitByRaycast);
                        }
                    }
                    else
                    {
                        // Disable the outline when the raycast doesn't hit the AmmoBox object
                        if (hoveredAmmoPack)
                        {                
                            hoveredAmmoPack.GetComponent<Outline>().enabled = false;
                        }
                    }
                }
            }
        }

}
