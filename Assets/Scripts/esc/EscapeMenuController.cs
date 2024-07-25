using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;



public class Guides : MonoBehaviour
{
    public GameObject menuPanel;
    public float animationDuration = 0.3f;
    public float menuHeight = 960f;
    public GameObject continueButton;
    public GameObject returnButton;
    public GameObject quitButton;


    private bool menuActive = false;
    private RectTransform menuRectTransform;
    private Vector3 hiddenPosition;
    private Vector3 shownPosition;

    void Start()
    {
        menuRectTransform = menuPanel.GetComponent<RectTransform>();

        hiddenPosition = new Vector3(menuRectTransform.anchoredPosition.x, -menuHeight, 0f);
        shownPosition = new Vector3(menuRectTransform.anchoredPosition.x, menuHeight, 0f);

        menuRectTransform.anchoredPosition = hiddenPosition;


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
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

    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        ToggleMenu();
    }

    public void ReturnToScene01()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scene0");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void ToggleMenuVisibility()
    {
        ToggleMenu();
    }
}