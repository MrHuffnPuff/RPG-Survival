using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [SerializeField] private WeaponHandler[] weapons;

    private int currentWeaponIndex;

    private bool armed = false;
    private bool unarmed = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TurnOnSelectedWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TurnOnSelectedWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TurnOnSelectedWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TurnOnSelectedWeapon(3);
        }

        StartCoroutine(HolsterCurrentWeapon());
    }

    void TurnOnSelectedWeapon(int weaponIndex)
    {
        //prevent animation starting again if we're already equiped with the weapon we're selecting
        if (currentWeaponIndex == weaponIndex && armed)
        {
            return;
        }

        //switching to another weapon whilst you already have a weapon equipped
        //plays function that holsters currently equipped weapon before drawing to selected weapon
        if (armed)
        {

            StartCoroutine(WeaponSwitchingAnimations(weaponIndex));
           
        }
        else if(unarmed)
        {

            //turn on selected weapon
            weapons[weaponIndex].gameObject.SetActive(true);

            //current weapon equals selected weapon index
            currentWeaponIndex = weaponIndex;

            armed = true;
            unarmed = false;
        }
    }

    public WeaponHandler GetCurrentSelectedWeapon()
    {
        return weapons[currentWeaponIndex];
    }

    public IEnumerator HolsterCurrentWeapon()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {

            GetCurrentSelectedWeapon().HolsterAnimation();

            //delay to ensure animation state is in Holster
            yield return new WaitForSeconds(0.1f);

            //delay until holstering animation finishes
            yield return new WaitForSeconds(GetCurrentSelectedWeapon().HolsterAnimation());

            //turn off current weapon
            weapons[currentWeaponIndex].gameObject.SetActive(false);
            unarmed = true;
            armed = false;

        }
    }//holster weapon

    public IEnumerator WeaponSwitchingAnimations(int weaponIndex)
    {

        //trigger the 'Holster' parameter within the animator to start the holstering weapon animation
        GetCurrentSelectedWeapon().HolsterAnimation();

        //delay to ensure animation state is in Holster
        yield return new WaitForSeconds(0.1f);

        //delay until holstering animation finishes
        yield return new WaitForSeconds(GetCurrentSelectedWeapon().HolsterAnimation());

        //turn off current weapon
        weapons[currentWeaponIndex].gameObject.SetActive(false);
        unarmed = true;
        armed = false;

        //turn on selected weapon
        weapons[weaponIndex].gameObject.SetActive(true);

        //current weapon equals selected weapon index
        currentWeaponIndex = weaponIndex;

        armed = true;
        unarmed = false;

    }//weapon switching animations
}
