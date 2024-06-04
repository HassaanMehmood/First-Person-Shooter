using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    // Bullet (Initial Settings) -----------------------------------------------------------------------------
    public GameObject bulletPrefab; // assigning bullet prefab
    public Transform bulletSpawn; // position where bullet will going to instantiate // assign bullet spawn game object
    public float bulletVelocity = 30; // Bullet Speed
    public float bulletPrefabLife = 3f; // 3 sec lifetime for enhancement

    // Shooting Limitations ----------------------------------------------------------------------------------
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 0.5f; // shoot delay after one shot

    // For Ray casting 
    // public Camera playerCamera; // assign the First person character

    // Spread / Edit Bullet Accuracy ------------------------------------------------------------------------
    public float spreadIntensity = 0;

    // Shooting Mode ----------------------------------------------------------------------------------------
    public enum shootingMode
    {
        Single,
        Auto
    }

    public shootingMode currentShootingMode;

    // It initialize tasks that need to be performed before the script's Start method is called. -------------
    private void Awake()
    {
        readyToShoot = true;
        animator = GetComponent<Animator>();
        bulletsLeft = magzineSize;
    }

    // Making Gun More Relastic -------------------------------------------------------------------------------
    public GameObject muzzleEffect;
    public Animator animator;

    // Bullet Reloading , Magzine Size and Bullets Left 
    public float reloadTime;
    public int magzineSize , bulletsLeft;
    public bool isReloading;

    // Seperate Every Weapon for having its seperate functionality -----------------------------------------
    public enum weaponModel
    {
        Pistol,
        Rifle
    }
    public weaponModel currentWeaponModel; // storing current gun name and for providing in sound methods 

    // Fixing the weapon Spawn Position ---------------------------------------------------------------------
    // public Vector3 weaponSpawnPosition;
    // public Vector3 weaponSpawnRotation;
    
    // Declare the initialBulletsLeft variable --------------------------------------------------------------
    private int initialBulletsLeft; 


    // Update is called once per frame ----------------------------------------------------------------------
    void Update()
    {
        if (currentShootingMode == shootingMode.Auto)
        {
            // Holding Down Left Mouse Button
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if(currentShootingMode == shootingMode.Single)
        {
            // Clicking Left Mouse Button Once
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        if (readyToShoot && isShooting && bulletsLeft > 0) 
        {
            fireWeapon();
        }
        // Reloading Button
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magzineSize && !isReloading && checkAmmoleft(currentWeaponModel) > 0)
        {
            Reload();
        }
        // Playing Empty Magzine Sound
        if (bulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.Empty_Magzine.Play();
        }
        // // Automatiacally reload
        // if (readyToShoot && bulletsLeft <=0 && !isReloading && !isShooting) 
        // {
        //     Reload();
        // }
        // ----------------------------------------------------------------------------------------------------
        // UI from singlenton / Global Method / Ammo Manager --------------------------------------------------
        // ----------------------------------------------------------------------------------------------------
        if(AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft}/{magzineSize}";
            AmmoManager.Instance.totalAmmoDisplay.text = $"{checkAmmoleft(currentWeaponModel)}";
        }
    }

    private int checkAmmoleft(weaponModel currentWeaponModel)
    {
        switch (currentWeaponModel)
        {
            case weaponModel.Pistol:
                return WeaponManager.Instance.totalPistolAmmo;
            case weaponModel.Rifle:
                return WeaponManager.Instance.totalRifleAmmo;
            // Add More Weapons here

            default:
                return 0;
        }
    }
    private void fireWeapon() {

        readyToShoot = false;
        // decreasing ammo
        bulletsLeft--;
        // Sound from singlenton / Global Method
        // SoundManager.Instance.Pistol_Shot.Play();
        SoundManager.Instance.PlayShootingSound(currentWeaponModel);
        // Playing Muzzle effect
        muzzleEffect.GetComponent<ParticleSystem>().Play();

        // Instantiate Bullet                                                 // default rotation
        GameObject bullet = Instantiate(bulletPrefab , bulletSpawn.position , Quaternion.identity); 
        // storing method of ray casting in variable of vector type
        Vector3 shootingDirection =  CalculateDirectionAndSpread().normalized;
        // Poiting the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;
        // Shoot THe Bullet                       // bulletSpawn.forward.normalized
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        // Destroy the bullet after some time
        StartCoroutine(DestroybulletPrefabLife( bullet , bulletPrefabLife));

        // Checking if we are done shooting
        if (allowReset)
        Invoke("ResetShot", shootingDelay);
        {
            allowReset = false;
        }
        // Animation for recoil
        animator.SetTrigger("isRecoil");


    }

    // For deletation of bullet clones from game 
    private IEnumerator DestroybulletPrefabLife(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
    // Reset Shot Method
    private void ResetShot()
    {
    readyToShoot= true;
    allowReset = true;
    }
    // Raycasting and Spread Bullet Method
    public Vector3 CalculateDirectionAndSpread()
    {
        // Shooting from the middle of the screen to check where are we pointing at
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
       
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
        // Hitting Something
        targetPoint = hit.point;
        }else
        {
        // Shooting at the air
        targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x= UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // Returning the shooting direction and spread
        return direction + new Vector3(x, y, 0);
    }

    // Bullet Reload Method
    private void Reload()
    {
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
        initialBulletsLeft = bulletsLeft; // Store the initial number of bullets left
        // SoundManager.Instance.Pistol_Reload.Play();
        SoundManager.Instance.PlayReloadingSound(currentWeaponModel);
        animator.SetTrigger("isReload");
    }

    // For Checking Remaining Bullets
    private void ReloadCompleted()
    {
        int remainingAmmo = checkAmmoleft(currentWeaponModel);

        if (remainingAmmo >= magzineSize - bulletsLeft)
        {
            // If there are enough remaining bullets to fill the magazine
            bulletsLeft = magzineSize;
        }
        else
        {
            // If there are not enough remaining bullets to fill the magazine
            bulletsLeft += remainingAmmo;
        }

        // Decrease total ammo accordingly
        WeaponManager.Instance.decreaseTotalAmmo(bulletsLeft - initialBulletsLeft , currentWeaponModel);

        isReloading = false;
    }



}
