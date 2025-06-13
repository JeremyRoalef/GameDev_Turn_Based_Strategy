using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField]
    Transform grenadeExplosionVFXPrefab;

    [SerializeField]
    TrailRenderer trailRenderer;

    [SerializeField]
    AnimationCurve arcYAnimationCurve;

    Vector3 targetPosition;
    Action OnGrenadeBehaviorComplete;

    float totalDistance;
    Vector3 positionXZ;

    private void Update()
    {
        Vector3 moveDir = (targetPosition-positionXZ).normalized;
        float moveSpeed = 15f;
        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - (distance / totalDistance);
        float reachedTargetDistance = 0.2f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        float maxHeight = totalDistance / 3f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;


        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        if (Vector3.Distance(positionXZ, targetPosition) <= reachedTargetDistance)
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
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            trailRenderer.transform.parent = null;
            Instantiate(grenadeExplosionVFXPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void Setup(GridPosition targetGridPosition, Action OnGrenadeBehaviorComplete)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        this.OnGrenadeBehaviorComplete = OnGrenadeBehaviorComplete;
        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(transform.position, targetPosition);
    }
}
