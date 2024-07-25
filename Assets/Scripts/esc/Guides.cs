using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class EscapeMenuController : MonoBehaviour
{
    public GameObject menuPanel; // 显示 ESC 菜单的面板
    public float animationDuration = 0.3f; // 动画持续时间
    public float menuHeight = 960f; // 菜单显示时的 Y 轴位置

    private bool menuActive = false;
    private RectTransform menuRectTransform;
    private Vector3 hiddenPosition;
    private Vector3 shownPosition;

    void Start()
    {
        menuRectTransform = menuPanel.GetComponent<RectTransform>();

        // 计算菜单面板的位置
        hiddenPosition = new Vector3(menuRectTransform.anchoredPosition.x, -menuHeight, 0f);
        shownPosition = new Vector3(menuRectTransform.anchoredPosition.x, menuHeight, 0f);

        // 初始时将菜单面板移动到隐藏位置
        menuRectTransform.anchoredPosition = hiddenPosition;

        // 确保菜单面板初始状态为隐藏
    }

    // Update 方法中不再需要检测 ESC 键

    void ToggleMenu()
    {
        // 切换 ESC 菜单的显示状态
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

        // 根据最终状态设置菜单面板的激活状态
    }

    // 新增的方法，由 UI 按钮调用来切换菜单状态
    public void ToggleMenuVisibility()
    {
        ToggleMenu();
    }

    
}
