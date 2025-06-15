using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField]
    Transform ragdollRootBone;

    [SerializeField]
    float explosionForce = 300;

    [SerializeField]
    float explosionRange = 10;

    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransform(originalRootBone, ragdollRootBone);

        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

        ApplyExplosionToRagdoll(ragdollRootBone, explosionForce, transform.position + randomDir, explosionRange);
    }

    void MatchAllChildTransform(Transform root, Transform clone)
    {
        foreach(Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                MatchAllChildTransform(child, cloneChild);
            }
        }
    }

    void ApplyExplosionToRagdoll(Transform originalRoot, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach(Transform child in originalRoot)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRb))
            {
                childRb.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
