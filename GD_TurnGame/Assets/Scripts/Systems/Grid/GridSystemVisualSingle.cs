using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField]
    MeshRenderer meshRenderer;

    public void Show(Material material)
    {
        meshRenderer.material = material;
        meshRenderer.enabled = true;
    }

    public void Hide()
    {
        if (!meshRenderer)
        {
            TryGetComponent<MeshRenderer>(out meshRenderer);
        }

        if (meshRenderer)
        {
            meshRenderer.enabled = false;
        }
    }

}
