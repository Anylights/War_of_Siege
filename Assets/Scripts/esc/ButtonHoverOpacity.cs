using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject objectToScale; // ��Ҫ����������
    private Vector3 originalScale;
    private float scaleMultiplier = 0.97f; // ���ŵı��������� 0.98 ��ʾ��ԭʼ��СС 2%

    private bool isMouseOver = false; // �Ƿ������ͣ�������ϵı�־

    private void Start()
    {
        originalScale = objectToScale.transform.localScale; // ���������ԭʼ��С
    }

    private void Update()
    {
        // �� Update �и�������Ƿ���ͣ����������Ĵ�С
        if (isMouseOver)
        {
            objectToScale.transform.localScale = originalScale * scaleMultiplier; // ��С����
        }
        else
        {
            objectToScale.transform.localScale = originalScale; // �ָ�ԭʼ��С
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ������ʱ��С����
        objectToScale.transform.localScale = originalScale * scaleMultiplier;

        // ��������ͣ��������
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ����뿪ʱ�ָ�ԭʼ��С
        objectToScale.transform.localScale = originalScale;

        // ȡ����������ͣ��������
        isMouseOver = false;
    }
}
