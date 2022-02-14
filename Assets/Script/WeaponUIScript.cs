using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WeaponUIScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI weaponNameText;
    [SerializeField] TextMeshProUGUI currentBullectCountText;
    [SerializeField] TextMeshProUGUI totalBullectCountText;

    [SerializeField] WeaponComponent weaponComponent;

    private void OnEnable()
    {
        PlayerEvents.OnWeaponEquipped += OnWeaponEquipped;
    }
    private void OnDisable()
    {
        PlayerEvents.OnWeaponEquipped -= OnWeaponEquipped;

    }
    void OnWeaponEquipped(WeaponComponent _weaponComponent)
    {
        weaponComponent = _weaponComponent;
    }

    // Update is called once per frame
    void Update()
    {
        if (!weaponComponent)
            return;

        weaponNameText.text = weaponComponent.weaponStats.weaponName;
        currentBullectCountText.text = weaponComponent.weaponStats.bulletsInClip.ToString();
        totalBullectCountText.text = weaponComponent.weaponStats.totalBullets.ToString();

    }
}
