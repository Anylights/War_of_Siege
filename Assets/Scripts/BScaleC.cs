using UnityEngine;
using UnityEngine.EventSystems;

public class BScaleC : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject objectToScale1;
    public GameObject objectToScale2;
    private Vector3 originalScale;
    private float scaleMultiplier = 1.1f;

    private void Start()
    {
        originalScale = objectToScale1.transform.localScale;
        originalScale = objectToScale2.transform.localScale;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ������ʱ�Ŵ�����
        objectToScale1.transform.localScale = originalScale * scaleMultiplier;
        objectToScale2.transform.localScale = originalScale * scaleMultiplier;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ����뿪ʱ�ָ�ԭʼ��С
        objectToScale1.transform.localScale = originalScale;
        objectToScale2.transform.localScale = originalScale;
    }
}
