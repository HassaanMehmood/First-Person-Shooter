using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    // Default Value of Ammo
    public int ammoAmount = 200;

    // Declaring Enumerator/Struct List containing 2 constants 
    public AmmoType ammoType;
    public enum AmmoType
    {
        RifleAmmo,
        PistolAmmo
    }
}