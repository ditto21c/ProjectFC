using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames.BasicApi.SavedGame;

public class BaseMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void SignInSuccess() { }
    public virtual void FailedGPG(object param) {  }
    public virtual void LoadScoreSuccess() { }
    public virtual void ReportAchievementSuccess() { }
    public virtual void ReportScoreSuccess() { }
    public virtual void OnSavedGameSelected(ISavedGameMetadata game) { }
    public virtual void OnSavedGameOpened(ISavedGameMetadata game) { }
    public virtual void OnSavedGameWritten(ISavedGameMetadata game) { }
}
