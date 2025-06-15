using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    Transform bulletProjectilePrefab;

    [SerializeField]
    Transform shootPointTransform;

    [SerializeField]
    Transform rifleTransform;

    [SerializeField]
    Transform meleeWeaponTransform;

    private void Awake()
    {
        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if(TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

        if (TryGetComponent<MeleeAction>(out MeleeAction meleeAction))
        {
            meleeAction.OnMeleeActionStarted += MeleeAction_OnMeleeActionStarted;
            meleeAction.OnMeleeActionCompleted += MeleeAction_OnMeleeActionCompleted;
        }
    }

    private void Start()
    {
        EquipRifle();
    }

    private void MeleeAction_OnMeleeActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();
    }

    private void MeleeAction_OnMeleeActionStarted(object sender, EventArgs e)
    {
        EquipMeleeWeapon();
        animator.SetTrigger("Melee");
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    void EquipMeleeWeapon()
    {
        meleeWeaponTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
    }
    void EquipRifle()
    {
        meleeWeaponTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);
    }
}
