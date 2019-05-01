using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames.BasicApi.SavedGame;

public class LoginMode : BaseMode
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject TextObj = GameObject.FindGameObjectWithTag("Text");
        text = TextObj.GetComponent<Text>();

        GameObject GPGMgrObj = GameObject.FindGameObjectWithTag("GPGMgr");
        GPGmgr = GPGMgrObj.GetComponent<GPGMgr>();

        text.text = "Please Sign In";

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TrySignIn() 
    {

#if UNITY_EDITOR
        text.text = "LoadScene : Game";
        SceneManager.LoadScene(1);
#else
       text.text = "TrySignIn";
       GPGmgr.SignIn();
#endif
    }
    public override void SignInSuccess()
    {
        text.text = "SignInSuccess";
        TryLoadScore();
    }
    public override void FailedGPG(object param)
    {
        text.text = "FailedGPG : " + param.ToString();
    }
    public void TryShowAchievement()
    {
        text.text = "ShowAchievement";
        GPGmgr.ShowAchievement();
    }
    public void TryShowLeaderboard()
    {
        text.text = "ShowLeaderboard";
        GPGmgr.ShowLeaderboard();
    }
    void TryLoadScore()
    {
        text.text = "TryLoadScore";
        GPGmgr.LoadScore();
    }
    public override void LoadScoreSuccess()
    {
        text.text = "LoadScoreSuccess";
        SceneManager.LoadScene(1);
    }
    public  void TryShowSelectSavedGameUI()
    {
        text.text = "TryShowSelectSavedGameUI";
        GPGmgr.SavedGame.ShowSelectUI();
    }
    public void TryOpenFile()
    {
        text.text = "TryOpenFile";
        GPGmgr.SavedGame.OpenSavedGame("SaveFile");
    }
    public void TrySave()
    {
        text.text = "TrySave";
        byte[] SaveData = new byte[100];
        System.TimeSpan time = new System.TimeSpan(1, 0, 0);
        GPGmgr.SavedGame.SaveGame(SavedGameData, SaveData, time);
    }
    public override void OnSavedGameSelected(ISavedGameMetadata game)
    {
        SavedGameData = game;
        text.text = "OnSavedGameSelected";
    }
    public override void OnSavedGameOpened(ISavedGameMetadata game)
    {
        SavedGameData = game;
        text.text = "OnSavedGameOpened";
    }
    public override void OnSavedGameWritten(ISavedGameMetadata game)
    {
        SavedGameData = game;
        text.text = "OnSavedGameWritten";
    }

    Text text;
    GPGMgr GPGmgr;
    ISavedGameMetadata SavedGameData;
}
