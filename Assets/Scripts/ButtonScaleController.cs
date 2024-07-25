using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject objectToScale; // ��Ҫ�Ŵ������
    private Vector3 originalScale;
    private float scaleMultiplier = 1.1f; // ���ŵı��������� 1.1 ��ʾ��ԭʼ��С�� 10%

    private void Start()
    {
        originalScale = objectToScale.transform.localScale; // ���������ԭʼ��С
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ������ʱ�Ŵ�����
        objectToScale.transform.localScale = originalScale * scaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ����뿪ʱ�ָ�ԭʼ��С
        objectToScale.transform.localScale = originalScale;
    }
}
