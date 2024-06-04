using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NUnit.Framework;

public class WeaponManager : MonoBehaviour
{
    // declares a public static property named Instance of type WeaponManager. 
    // Static properties belong to the class itself rather than to instances of the class. 
    // This property is designed to hold a reference to the single instance of the WeaponManager class throughout the game.
    public static WeaponManager Instance { get; set; }  
    
    [Header("Ammo")] // For Inspector Heading
    public int totalPistolAmmo = 0;
    public int totalRifleAmmo = 0;
    
        // Unity method that is called when the script instance is being loaded.
        private void Awake()
        {
            // (Singleton pattern) enforce that only one instance of a class exists at any given time.
            if (Instance != null && Instance != this)
            {
                // prevents duplicate instances of the WeaponManager from coexisting in the scene.
                Destroy(gameObject);
            }
            else
            { 
                // sets the Instance property to the current instance of the WeaponManager class.
                Instance = this;
            }
        }

        // Increasing Ammo ---------------------------------------------------------------------------------- 
        // contain one argument
        // AmmoBox, object (ammo) as an argument, which contains information about the type  of ammo in the pack.
        // The internal like public keyword is an access modifier 
        // allows you to restrict access to certain methods or variables within your project or assembly, 
        // providing encapsulation and controlling visibility as needed.
        internal void PickupAmmoPack(AmmoBox ammo)
        {
            switch (ammo.ammoType)
            {
                // Pistol Ammo
                case AmmoBox.AmmoType.PistolAmmo:
                    totalPistolAmmo += ammo.ammoAmount;
                    break;
                // Rifle Ammo
                case AmmoBox.AmmoType.RifleAmmo:
                    totalRifleAmmo += ammo.ammoAmount;
                    break;
                // Add More Weapons here
            }
            // Implement the logic to pick up ammo here
            Debug.Log("Picked up ammo!"); // Print a debug message
        }

        // Decreasing Ammo ----------------------------------------------------------------------------------
        // Contains 2 arguments 
        // bulletsToDecrease, which specifies the number of bullets to subtract from the total ammo count
        // currentWeaponModel from weapon script where weapon mode is enumeration,
        // which indicates the type of weapon currently being used. 
        internal void decreaseTotalAmmo(int bulletsToDecrease, Weapon.weaponModel currentWeaponModel)
        {
            switch (currentWeaponModel)
            {
                // Pistol Ammo
                case Weapon.weaponModel.Pistol:
                    totalPistolAmmo -= bulletsToDecrease;
                    break;
                // Rifle Ammo
                case Weapon.weaponModel.Rifle:
                    totalRifleAmmo -= bulletsToDecrease;
                    break;
                // Add More Weapons here
            }
        }

}
