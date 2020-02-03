using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAim { 
    NONE,
    SELF_AIM,
    AIM
}

public enum WeaponFireType { 
    SINGLE,
    MULTIPLE
}

public enum WeaponBulletType { 
    BULLET,
    ARROW,
    SPEAR,
    NONE
}

public class WeaponHandler : MonoBehaviour
{

    private Animator anim;
    private float m_CurrentStateLength;

    public WeaponAim weapon_Aim;

    [SerializeField] private GameObject muzzleFlash;

    [SerializeField] private AudioSource shootSound, reloadSound;

    public WeaponFireType fireType;
    public WeaponBulletType bulletType;
    public GameObject attack_Point;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ShootAnimation()
    {
        anim.SetTrigger(AnimationTags.SHOOT_TRIGGER);
    }

    public void Aim(bool canAim)
    {
        anim.SetBool(AnimationTags.AIM_PARAMETER, canAim);
    }

    public float HolsterAnimation()
    {        
        anim.SetTrigger(AnimationTags.HOLSTER_TRIGGER);
        m_CurrentStateLength = anim.GetCurrentAnimatorStateInfo(0).length;
        return m_CurrentStateLength;
    }

    void Turn_On_MuzzleFlash()
    {
        muzzleFlash.SetActive(true);
    }

    void Turn_Off_MuzzleFlash()
    {
        muzzleFlash.SetActive(false);
    }

    void PlayShootSound()
    {
        shootSound.Play();
    }

    void Play_ReloadSound()
    {
        reloadSound.Play();
    }

    void TurnOnAttackPoint()
    {
        attack_Point.SetActive(true);
    }

    void TurnOffAttackPoint()
    {
        if (attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }
}
