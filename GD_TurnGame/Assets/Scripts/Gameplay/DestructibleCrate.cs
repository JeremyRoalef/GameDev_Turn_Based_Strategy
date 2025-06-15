using System;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestroyed;

    [Header("References")]
    [SerializeField]
    Transform crateDestroyedPrefab;

    [Header("Settings")]
    [SerializeField]
    float explosionForce = 150f;

    [SerializeField]
    float explosionRange = 10f;

    GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void Damage()
    {
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
        ApplyExplosionToChildren(crateDestroyedTransform, explosionForce, transform.position, explosionRange);
        Destroy(gameObject);
    }

    void ApplyExplosionToChildren(Transform originalRoot, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in originalRoot)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRb))
            {
                childRb.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
}
