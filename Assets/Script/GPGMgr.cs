using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;

public class GPGMgr : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        SavedGame = new GPGSavedGame();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject ModeObj = GameObject.FindGameObjectWithTag("Mode");
        Mode = ModeObj.GetComponent<BaseMode>();
#if UNITY_ANDROID

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();

#elif UNITY_IOS
 
        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
 
#endif

        SavedGame.Mode = Mode;

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SignIn()
    {
        Social.localUser.Authenticate((result, errorMessage) => {
            if (result)
            {
                Mode.SignInSuccess();
            }
            else
            {
                Mode.FailedGPG(errorMessage);
            }
        });
    }
    public void SignOut()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.SignOut();
#endif
    }
    
    public void ShowAchievement()
    {
        if(Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((result, errorMessage) => {
                if (result)
                {
                    Social.ShowAchievementsUI();
                }
                else
                {
                    Mode.FailedGPG(errorMessage);
                }
            });
        }
        else
        {
            Social.ShowAchievementsUI();
        }
    }
    public void ReportAchievement(float time)
    {
        string achievementID = "";
        if (time < 1.0f)
            achievementID = GPGSIds.achievement_1s;
        else if (time < 5.0f)
            achievementID = GPGSIds.achievement_5s;
        else if (time < 10.0f)
            achievementID = GPGSIds.achievement_10s;

        if (0 < achievementID.Length)
        {
            Social.ReportProgress(achievementID, 100.0, (bool success) =>
            {
                if (success)
                {
                    Mode.ReportAchievementSuccess();
                }
                else
                {
                    Mode.FailedGPG("ReportAchievement");
                }
            });
        }
    }

    public void ShowLeaderboard()
    {
        if (Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((result, errorMessage) => {
                if (result)
                {
                    Social.ShowLeaderboardUI();
                }
                else
                {
                    Mode.FailedGPG(errorMessage); 
                }
            });
        }
        else
        {
            Social.ShowLeaderboardUI();
        }
    }
    public void ReportScore(long score)
    {
        Social.ReportScore(score, GPGSIds.leaderboard_score, (bool success) =>
        {
            if (success)
            {
                Mode.ReportScoreSuccess();
            }
            else
            {
                Mode.FailedGPG("ReportScore");
            }
        });
    }
    public void LoadScore()
    {
        Scores.Clear();
        Social.LoadScores(GPGSIds.leaderboard_score, scores => {
            if (scores.Length > 0)
            {
                Debug.Log("Got " + scores.Length + " scores");
                string myScores = "Leaderboard:\n";
                foreach (IScore score in scores)
                {
                    myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
                    Scores.Add(score.value);
                }
                Debug.Log(myScores);

                
            }
            Mode.LoadScoreSuccess();
        });
    }

    ArrayList Scores = new ArrayList();
    BaseMode Mode;
    public GPGSavedGame SavedGame;

}
