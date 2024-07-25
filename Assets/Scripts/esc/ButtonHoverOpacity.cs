using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject objectToScale; // 需要操作的物体
    private Vector3 originalScale;
    private float scaleMultiplier = 0.97f; // 缩放的倍数，例如 0.98 表示比原始大小小 2%

    private bool isMouseOver = false; // 是否鼠标悬停在物体上的标志

    private void Start()
    {
        originalScale = objectToScale.transform.localScale; // 保存物体的原始大小
    }

    private void Update()
    {
        // 在 Update 中根据鼠标是否悬停来更新物体的大小
        if (isMouseOver)
        {
            objectToScale.transform.localScale = originalScale * scaleMultiplier; // 缩小物体
        }
        else
        {
            objectToScale.transform.localScale = originalScale; // 恢复原始大小
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 鼠标进入时缩小物体
        objectToScale.transform.localScale = originalScale * scaleMultiplier;

        // 标记鼠标悬停在物体上
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 鼠标离开时恢复原始大小
        objectToScale.transform.localScale = originalScale;

        // 取消标记鼠标悬停在物体上
        isMouseOver = false;
    }
}
