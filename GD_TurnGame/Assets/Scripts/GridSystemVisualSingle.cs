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
        meshRenderer.enabled = false;
    }

}
