using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class EscapeMenuController : MonoBehaviour
{
    public GameObject menuPanel; // ��ʾ ESC �˵������
    public float animationDuration = 0.3f; // ��������ʱ��
    public float menuHeight = 960f; // �˵���ʾʱ�� Y ��λ��

    private bool menuActive = false;
    private RectTransform menuRectTransform;
    private Vector3 hiddenPosition;
    private Vector3 shownPosition;

    void Start()
    {
        menuRectTransform = menuPanel.GetComponent<RectTransform>();

        // ����˵�����λ��
        hiddenPosition = new Vector3(menuRectTransform.anchoredPosition.x, -menuHeight, 0f);
        shownPosition = new Vector3(menuRectTransform.anchoredPosition.x, menuHeight, 0f);

        // ��ʼʱ���˵�����ƶ�������λ��
        menuRectTransform.anchoredPosition = hiddenPosition;

        // ȷ���˵�����ʼ״̬Ϊ����
    }

    // Update �����в�����Ҫ��� ESC ��

    void ToggleMenu()
    {
        // �л� ESC �˵�����ʾ״̬
        menuActive = !menuActive;

        if (menuActive)
        {
            StartCoroutine(MoveMenu(shownPosition));
        }
        else
        {
            StartCoroutine(MoveMenu(hiddenPosition));
        }
    }

    IEnumerator MoveMenu(Vector3 targetPosition)
    {

        Vector3 startPosition = menuRectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            menuRectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        menuRectTransform.anchoredPosition = targetPosition;

        // ��������״̬���ò˵����ļ���״̬
    }

    // �����ķ������� UI ��ť�������л��˵�״̬
    public void ToggleMenuVisibility()
    {
        ToggleMenu();
    }

    
}
