using System;
using UnityEngine;

public class UnitRagdollSpawner : MonobehaviourEventListener
{
    [SerializeField]
    Transform ragdollPrefab;

    [SerializeField]
    Transform originalRootBone;

    HealthSystem healthSystem;


    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        SubscribeEvents();
    }



    protected override void SubscribeEvents()
    {
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    protected override void UnsubscribeEvents()
    {
        healthSystem.OnDead -= HealthSystem_OnDead;
    }



    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);

        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);
    }
}
