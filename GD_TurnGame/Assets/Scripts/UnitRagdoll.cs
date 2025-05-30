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
        ApplyExplosionToRagdoll(ragdollRootBone, explosionForce, transform.position, explosionRange);
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
