using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EndMenuManager : MonoBehaviour
{
    public RectTransform TiePanel;
    public RectTransform PlayerAWinPanel;
    public RectTransform PlayerBWinPanel;

    public MapManager mapManager;

    public Text A_Score;
    public Text B_Score;
    public Text Tie_Score;

    public GameObject EffectsPrehab1;
    public GameObject EffectsPrehab2;
    public GameObject EffectsPrehab3;

    public AudioSource audioSource;
    public AudioClip firework;

    public float animationDuration = 5.0f;
    private Vector3 offScreenPosition;
    private Vector3 onScreenPosition;

    public GameObject EffectPosition;

    void Start()
    {
        offScreenPosition = new Vector3(0, -1000, 0);
        onScreenPosition = new Vector3(0, 0, 0);

        TiePanel.anchoredPosition = offScreenPosition;
        PlayerAWinPanel.anchoredPosition = offScreenPosition;
        PlayerBWinPanel.anchoredPosition = offScreenPosition;
        audioSource = GetComponent<AudioSource>();
    }

    public void ShowTiePanel()
    {
        StartCoroutine(AnimatePanel(TiePanel, Tie_Score));
    }

    public void ShowPlayerAWinPanel()
    {
        StartCoroutine(AnimatePanel(PlayerAWinPanel, A_Score));
    }

    public void ShowPlayerBWinPanel()
    {
        StartCoroutine(AnimatePanel(PlayerBWinPanel, B_Score));
    }

    IEnumerator AnimatePanel(RectTransform panel, Text Winner_Score)
    {
        Winner_Score.text = mapManager.GetWinnerScore().ToString();
        float elapsedTime = 0;
        Vector3 startingPos = panel.anchoredPosition;

        while (elapsedTime < animationDuration)
        {
            panel.anchoredPosition = Vector3.Lerp(startingPos, onScreenPosition, (elapsedTime / animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = onScreenPosition;
        audioSource.PlayOneShot(firework);
        Instantiate(EffectsPrehab1, EffectPosition.transform.position, Quaternion.identity);
        Instantiate(EffectsPrehab2, EffectPosition.transform.position, Quaternion.identity);
        Instantiate(EffectsPrehab3, EffectPosition.transform.position, Quaternion.identity);
    }
}
