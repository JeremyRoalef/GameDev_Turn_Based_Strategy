using System;
using UnityEngine;

public class ScreenShakeActions : MonobehaviourEventListener
{
    [SerializeField]
    float meleeActionScreenShakeIntensity = 0.2f;

    [SerializeField]
    float grenadeActionScreenShakeIntensity = 0.5f;

    [SerializeField]
    float shootActionScreenShakeIntensity = 1f;


    private void Start()
    {
        SubscribeEvents();
    }



    protected override void SubscribeEvents()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        MeleeAction.OnAnyMeleeWeaponHit += MeleeAction_OnAnyMeleeWeaponHit;
    }

    protected override void UnsubscribeEvents()
    {
        ShootAction.OnAnyShoot -= ShootAction_OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded -= GrenadeProjectile_OnAnyGrenadeExploded;
        MeleeAction.OnAnyMeleeWeaponHit -= MeleeAction_OnAnyMeleeWeaponHit;
    }



    private void MeleeAction_OnAnyMeleeWeaponHit(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(meleeActionScreenShakeIntensity);
    }

    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(grenadeActionScreenShakeIntensity);
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake(shootActionScreenShakeIntensity);
    }
}
