using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    private static UiManager instance;
    public GameObject failedScreen;
    public GameObject winScreen;
    public TextMeshProUGUI MovesRemainingText;
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI levelText;
    public static UiManager Instance()
    {
        return instance;
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        failedScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ActiveFailedScreen()
    {
        failedScreen.SetActive(true);
    }
    public void DisableFailedScreen()
    {
        failedScreen.SetActive(false);
    }
    public void ActiveWinScreen()
    {
        winScreen.SetActive(true);
    }
    public void DisableWinScreen()
    {
        winScreen.SetActive(false);
    }
    public void SetMoveRemaingText(int remaining)
    {
        MovesRemainingText.text = "Moves: " + remaining;
    }
    public void SetPointText(int point)
    {
        pointText.text = point.ToString();
    }
    public void SetLevelText(int level)
    {
        levelText.text = "Level " + level;
    }
}
