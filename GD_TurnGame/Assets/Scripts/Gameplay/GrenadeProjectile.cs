using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;

    [Header("References")]
    [SerializeField]
    Transform grenadeExplosionVFXPrefab;

    [SerializeField]
    TrailRenderer trailRenderer;

    [SerializeField]
    AnimationCurve arcYAnimationCurve;

    [Header("Settings")]
    [SerializeField]
    float moveSpeed = 15f;

    Action OnGrenadeBehaviorComplete;
    Vector3 targetPosition;
    Vector3 positionXZ;
    Vector3 moveDir;

    float totalDistance;
    float distance;
    float distanceNormalized;
    float maxHeight;
    float positionY;

    const float REACHED_TARGET_DISTANCE = 0.2f;

    private void Update()
    {
        moveDir = (targetPosition-positionXZ).normalized;
        distance = Vector3.Distance(positionXZ, targetPosition);
        distanceNormalized = totalDistance == 0? 1: 1 - (distance / totalDistance);
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        maxHeight = totalDistance / 3f;
        positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;


        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        if (Vector3.Distance(positionXZ, targetPosition) <= REACHED_TARGET_DISTANCE)
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
                if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
                {
                    destructibleCrate.Damage();
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
