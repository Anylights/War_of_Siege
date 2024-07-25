using UnityEngine;

public class Tile : MonoBehaviour
{
    public Material newMaterial; // 要替换的新材质
    private Material originalMaterial; // 保存原始材质
    private Renderer objectRenderer; // 对象的渲染器

    private void Awake()
    {

        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material; // 保存原始材质
    }

    public void isInPath(bool isinpath)
    {
        if (isinpath)
        {
            objectRenderer.material = newMaterial; // 更换材质
        }
        else
        {
            objectRenderer.material = originalMaterial;
        }

    }


}
