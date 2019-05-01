using UnityEngine;
using UnityEditor;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;
using System;

public class GPGSavedGame : ScriptableObject
{
#if UNITY_EDITOR
    [MenuItem("Tools/MyTool/Do It in C#")]
    static void DoIt()
    {
        EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    }
#endif

    // Displaying saved games UI
    public void ShowSelectUI()
    {
#if UNITY_ANDROID
        if (Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((result, errorMessage) => {
                if (result)
                {
                    uint maxNumToDisplay = 5;
                    bool allowCreateNew = false;
                    bool allowDelete = true;

                    ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                    savedGameClient.ShowSelectSavedGameUI("Select saved game",
                        maxNumToDisplay,
                        allowCreateNew,
                        allowDelete,
                        OnSavedGameSelected);
                }
                else
                {
                    Mode.FailedGPG(errorMessage);
                }
            });
        }
        else
        {
            uint maxNumToDisplay = 5;
            bool allowCreateNew = true;
            bool allowDelete = true;

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ShowSelectSavedGameUI("Select saved game",
                maxNumToDisplay,
                allowCreateNew,
                allowDelete,
                OnSavedGameSelected);
        }
#endif
    }
    public void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    {
        if (status == SelectUIStatus.SavedGameSelected)
        {
            // handle selected game save
            Mode.OnSavedGameSelected(game);
        }
        else
        {
            string errorStr = string.Format("OnSavedGameSelected Status {0}", status);
            Mode.FailedGPG(errorStr);
        }
    }

    // Opening a saved game
   public void OpenSavedGame(string filename)
    {
#if UNITY_ANDROID
        if (Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((result, errorMessage) =>
            {
                if (result)
                {
                    ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                    savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
                        ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
                }
                else
                {
                    Mode.FailedGPG(errorMessage);
                }
            });
        }
        else
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
        }
#endif
    }
    public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
            Mode.OnSavedGameOpened(game);
        }
        else
        {
            string errorStr = string.Format("OnSavedGameOpened Status {0}", status);
            Mode.FailedGPG(errorStr);
        }
    }

    public void SaveGame(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
        builder = builder
            .WithUpdatedPlayedTime(totalPlaytime)
            .WithUpdatedDescription("Saved game at " + DateTime.Now);

        if (savedImage != null)
        {
            // This assumes that savedImage is an instance of Texture2D
            // and that you have already called a function equivalent to
            // getScreenshot() to set savedImage
            // NOTE: see sample definition of getScreenshot() method below
            byte[] pngData = savedImage.EncodeToPNG();
            builder = builder.WithUpdatedPngCoverImage(pngData);
        }
        SavedGameMetadataUpdate updatedMetadata = builder.Build();
        savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
    }

    public void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
            Mode.OnSavedGameWritten(game);
        }
        else
        {
            string errorStr = string.Format("OnSavedGameWritten Status {0}", status);
            Mode.FailedGPG(errorStr);
        }
    }

    public Texture2D getScreenshot()
    {
        // Create a 2D texture that is 1024x700 pixels from which the PNG will be
        // extracted
        Texture2D screenShot = new Texture2D(1024, 700);

        // Takes the screenshot from top left hand corner of screen and maps to top
        // left hand corner of screenShot texture
        screenShot.ReadPixels(
            new Rect(0, 0, Screen.width, (Screen.width / 1024) * 700), 0, 0);
        return screenShot;
    }

    void LoadGameData(ISavedGameMetadata game)
    {
#if UNITY_ANDROID
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
#endif
    }

    public void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle processing the byte array data
        }
        else
        {
            string errorStr = string.Format("OnSavedGameDataRead Status {0}", status);
            Mode.FailedGPG(errorStr);
        }
    }
    void DeleteGameData(string filename)
    {
#if UNITY_ANDROID
        // Open the file to get the metadata.
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, DeleteSavedGame);
#endif
    }

    public void DeleteSavedGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
#if UNITY_ANDROID
        if (status == SavedGameRequestStatus.Success)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.Delete(game);
        }
        else
        {
            string errorStr = string.Format("DeleteSavedGame Status {0}", status);
            Mode.FailedGPG(errorStr);
        }
#endif
    }

    public BaseMode Mode;
    Texture2D savedImage;
}