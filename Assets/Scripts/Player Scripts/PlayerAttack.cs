using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private WeaponManager weaponManager;

    [SerializeField] float fireRate;
    private float nextTimeToFire;
    [SerializeField] float damage = 10f;

    private Animator zoomCameraAnim;
    private bool zoomed;

    private Camera mainCam;

    private GameObject crosshair;

    private bool is_Aiming;

    [SerializeField] private GameObject arrowPrefab, spearPrefab;

    [SerializeField] private Transform arrowBowStartPosition;

    // Start is called before the first frame update
    void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        zoomCameraAnim = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();
        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();
    }

    void WeaponShoot()
    {
        //if we have assualt rifle
        if (weaponManager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            //if we press and hold mouse button AND
            //if time is greater then the nextTimeToFire
            if (Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;

                weaponManager.GetCurrentSelectedWeapon().ShootAnimation();

                BulletFired();
            }
        }
        //if we have a semi-automatic weapon
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                //handle axe
                if (weaponManager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG)
                {
                    weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                }

                //handle shoot
                if (weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET)
                {
                    weaponManager.GetCurrentSelectedWeapon().ShootAnimation();

                    BulletFired();
                }
                else 
                {
                    //we have an arrow or spear
                    if (is_Aiming)
                    {
                        weaponManager.GetCurrentSelectedWeapon().ShootAnimation();

                        if (weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            ThrowArrowOrSpear(true);
                        }
                        else if(weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR)
                        {
                            ThrowArrowOrSpear(false);
                        }
                    }
                }
            }
        }
    }

    void ZoomInAndOut()
    {
        //we are going to aim with our camera on the weapon
        if (weaponManager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                zoomCameraAnim.Play(AnimationTags.ZOOM_IN_ANIM);

                crosshair.SetActive(false);
            }

            //when we release right mouse button
            if (Input.GetMouseButtonUp(1))
            {
                zoomCameraAnim.Play(AnimationTags.ZOOM_OUT_ANIM);

                crosshair.SetActive(true);
            }
        }//if we need to zoom weapon

        if (weaponManager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.SELF_AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                weaponManager.GetCurrentSelectedWeapon().Aim(true);
                is_Aiming = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                weaponManager.GetCurrentSelectedWeapon().Aim(false);
                is_Aiming = false;
            }
        }

    }//zoom in and out()

    void ThrowArrowOrSpear(bool throwArrow)
    {
        if (throwArrow)
        {
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.transform.position = arrowBowStartPosition.position;

            arrow.GetComponent<BowAndArrow>().LaunchObject(mainCam);
        }
        else
        {
            GameObject spear = Instantiate(spearPrefab);
            spear.transform.position = arrowBowStartPosition.position;

            spear.GetComponent<BowAndArrow>().LaunchObject(mainCam);
        }
    }

    void BulletFired()
    {
        RaycastHit hit;

        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))
        {
            print("WE HIT: " + hit.transform.gameObject.name);
        }
    }

}//class
