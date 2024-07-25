using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject objectToScale; // 需要放大的物体
    private Vector3 originalScale;
    private float scaleMultiplier = 1.1f; // 缩放的倍数，例如 1.1 表示比原始大小大 10%

    private void Start()
    {
        originalScale = objectToScale.transform.localScale; // 保存物体的原始大小
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 鼠标进入时放大物体
        objectToScale.transform.localScale = originalScale * scaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 鼠标离开时恢复原始大小
        objectToScale.transform.localScale = originalScale;
    }
}
