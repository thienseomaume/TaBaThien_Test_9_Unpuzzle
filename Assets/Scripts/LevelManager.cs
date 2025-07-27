using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public Action collectPoint;
    private int remainingBlock;
    private int remainingStep;
    private int point;
    [SerializeField] float winScreenTime;
    [SerializeField] List<LevelData> listLevels;
    public static LevelManager Instance()
    {
        return instance;
    }
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        LevelData levelData = listLevels[PlayerPrefs.GetInt(StringData.CurrentLevel, 0)];
        GridManager.Instance().GenerateLevel(levelData);
        SetRemainingStep(levelData.moveStep);
        SetPoint(PlayerPrefs.GetInt(StringData.Point, 0));
        UiManager.Instance().SetLevelText(PlayerPrefs.GetInt(StringData.CurrentLevel, 0)+1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CollectPoint()
    {
        collectPoint?.Invoke();
        remainingBlock -= 1;
        if (remainingBlock == 0)
        {
            Debug.Log("win level");
            LoadNextLevel();
        }
        SetPoint(point += 1);
        PlayerPrefs.SetInt(StringData.Point, point);
        SoundManager.Instance().PlayCorrectSound();
    }
    public void SetRemainingBlock(int remainingBlock)
    {
        this.remainingBlock = remainingBlock;
    }
    public void SetRemainingStep(int remainingStep)
    {
        this.remainingStep = remainingStep;
        UiManager.Instance().SetMoveRemaingText(remainingStep);
    }
    public void SetPoint(int point)
    {
        this.point = point;
        UiManager.Instance().SetPointText(point);
    }
    public void MadeATouch()
    {
        SetRemainingStep(remainingStep -= 1);
        if (remainingStep <= 0 && remainingBlock > 0)
        {
            Debug.Log("failed level");
            UiManager.Instance().ActiveFailedScreen();
        }
    }
    public void ReloadLevel()
    {
        LevelData levelData = listLevels[PlayerPrefs.GetInt(StringData.CurrentLevel, 0)];
        GridManager.Instance().ReloadLevel(levelData);
        UiManager.Instance().DisableFailedScreen();
        SetRemainingStep(levelData.moveStep);
    }
    public void LoadNextLevel()
    {
        if (PlayerPrefs.GetInt(StringData.CurrentLevel, 0) + 1 >= listLevels.Count)
        {
            return;
        }
        PlayerPrefs.SetInt(StringData.CurrentLevel, PlayerPrefs.GetInt(StringData.CurrentLevel, 0) + 1);
        StartCoroutine(WaitWinScreen());
    }
    IEnumerator WaitWinScreen()
    {
        UiManager.Instance().ActiveWinScreen();
        yield return new WaitForSeconds(winScreenTime);
        UiManager.Instance().DisableWinScreen();
        ReloadLevel();
        UiManager.Instance().SetLevelText(PlayerPrefs.GetInt(StringData.CurrentLevel, 0)+1);
    }
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
