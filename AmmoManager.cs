using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    // declares a public static property named Instance of type AmmoManager. 
    // Static properties belong to the class itself rather than to instances of the class. 
    // This property is designed to hold a reference to the single instance of the AmmoManager class throughout the game.
    public static AmmoManager Instance { get; set; }

    // Hold a reference of UI Elements from Inspector.
    public Text ammoDisplay;
    public Text totalAmmoDisplay;

    // Unity method that is called when the script instance is being loaded.
    private void Awake()
    {
        // (Singleton pattern) enforce that only one instance of a class exists at any given time.
        if (Instance != null && Instance != this)
        {
            // prevents duplicate instances of the AmmoManager from coexisting in the scene.
            Destroy(gameObject);
        }
        else
        { 
            // sets the Instance property to the current instance of the AmmoManager class.
            Instance = this;
        }
    }
}
