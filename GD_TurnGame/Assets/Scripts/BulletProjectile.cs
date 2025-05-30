using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 200f;

    [SerializeField]
    TrailRenderer trailRenderer;

    [SerializeField]
    Transform bulletHitVFXPrefab;

    Vector3 targetPos;

    private void Update()
    {
        Vector3 moveDir = (targetPos - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPos);

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPos);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            //overshot target
            transform.position = targetPos;
            trailRenderer.transform.parent = null;
            Destroy(gameObject);

            Instantiate(bulletHitVFXPrefab, targetPos, Quaternion.identity);
        }
    }

    public void Setup(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }
}
