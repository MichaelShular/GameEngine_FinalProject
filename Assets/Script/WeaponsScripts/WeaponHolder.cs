using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponHolder : MonoBehaviour
{
    [Header("Weapon To Spawn"), SerializeField]
    GameObject weaponToSpawn;

    public Sprite crossHairImage;

    WeaponComponent equippedWeapon;

    [SerializeField]
    GameObject weaponSocketLocation;
    [SerializeField]
    Transform gripIKSocketLocation;

    public PlayerController playerController;
    Animator playerAnimator;

    bool firingPressed = false;

    public readonly int isFiringHash = Animator.StringToHash("isFiring");
    public readonly int isReloadingHash = Animator.StringToHash("isReloading");

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        GameObject spawnedWeapon = Instantiate(weaponToSpawn, weaponSocketLocation.transform.position, weaponSocketLocation.transform.rotation, weaponSocketLocation.transform);

        equippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();
        equippedWeapon.Initialize(this);
        gripIKSocketLocation = equippedWeapon.gripLocation;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!playerController.isReloading)
        {
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, gripIKSocketLocation.transform.position);
        }
    }

    public void OnFire(InputValue value)
    {
        firingPressed = value.isPressed;
        if (firingPressed)
        {
            StartFiring();
        }
        else
        {
            StopFiring();
        }

    }
    public void OnReload(InputValue value)
    {
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
        playerController.isReloading = value.isPressed;

        playerAnimator.SetBool(isReloadingHash, playerController.isReloading);
    }

    void StartFiring()
    {
        if (equippedWeapon.weaponStats.bulletsInClip <= 0) return;
        playerAnimator.SetBool(isFiringHash, playerController.isFiring);
        playerController.isFiring = true;
        equippedWeapon.StartFiringWeapon();
    }

    void StopFiring()
    {
        playerAnimator.SetBool(isFiringHash, false);
        playerController.isFiring = false;

        equippedWeapon.StopFiringWeapon();
    }
}
