using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    Vector3 targetPosition;
    Action OnGrenadeBehaviorComplete;

    private void Update()
    {
        Vector3 moveDir = (targetPosition-transform.position).normalized;
        float moveSpeed = 15f;
        float reachedTargetDistance = 0.2f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) <= reachedTargetDistance)
        {
            float damageRadius = 4f;

            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if(collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    int damageAmount = 20;
                    targetUnit.Damage(damageAmount);
                }
            }

            OnGrenadeBehaviorComplete?.Invoke();
            Destroy(gameObject);
        }
    }

    public void Setup(GridPosition targetGridPosition, Action OnGrenadeBehaviorComplete)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        this.OnGrenadeBehaviorComplete = OnGrenadeBehaviorComplete;
    }


}
