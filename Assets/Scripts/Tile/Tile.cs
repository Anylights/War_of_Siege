using UnityEngine;

public class Tile : MonoBehaviour
{
    private Renderer rend;

    private void Start()
    {
        // 在当前GameObject或子对象中找到Renderer组件
        rend = GetComponentInChildren<Renderer>();

        if (rend == null)
        {
            Debug.LogError("Tile上没有找到Renderer组件！");
        }
    }

    public void Highlight(bool highlight)
    {
        if (rend != null)
        {
            rend.material.color = highlight ? Color.yellow : Color.white;
        }
    }
}
