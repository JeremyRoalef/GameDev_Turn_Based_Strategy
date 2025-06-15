using System;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestroyed;

    [SerializeField]
    Transform crateDestroyedPrefab;
    
    GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void Damage()
    {
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
        Transform crateDestroyedTransofrm = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
        ApplyExplosionToChildren(crateDestroyedTransofrm, 150, transform.position, 10);
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
