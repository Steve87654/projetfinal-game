using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fusils : MonoBehaviour
{
    public int damage;
    public float tempEntreTir,range,reloadTime,timeBetweenShots;
    public int magazineSize, bulletPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletShot;

    bool shooting, readyToShoot, reloading;

    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;


    public void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            shoot();
        }
    }

    private void shoot() 
    {
        readyToShoot = false;

       // if(Physics.Raycast)

        bulletsLeft--;

        Invoke("ResetShot", timeBetweenShots);
    }

    private void Resetshot()
    {
        readyToShoot = true;

     

    }


    private void Reload()
    {
        throw new NotImplementedException();
    }
}
