using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon; // to access weapon script
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource Pistol_Shot;
    public AudioSource Pistol_Reload;
    public AudioSource Empty_Magzine;
    public AudioSource Rifle_Shot;
    public AudioSource Rifle_Reload;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
        Destroy(gameObject);
        }
        else
        { 
            Instance = this;
        }
    }

    public void PlayShootingSound(weaponModel weapon)
    {
        switch (weapon)
        {
            case weaponModel.Pistol:
            Pistol_Shot.Play();
            break;

            case weaponModel.Rifle:
            Rifle_Shot.Play();
            break;
        }
    }

    public void PlayReloadingSound(weaponModel weapon)
    {
        switch (weapon)
        {
            case weaponModel.Pistol:
            Pistol_Reload.Play();
            break;

            case weaponModel.Rifle:
            Rifle_Reload.Play();
            break;
        }
    }

}
