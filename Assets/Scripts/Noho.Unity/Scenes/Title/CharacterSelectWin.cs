using System.Collections;
using System.Collections.Generic;
using Noho.Configs;
using Noho.Unity.Managers;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Title
{
    public class CharacterSelectWin : UIMonoBehaviour
    {
        public CharacterOptionController Template;

        public Transform OptionsContainer;

        public Button AddYourOwnBtn;
        public Button BackButton;
        public Button UnlockAllButton;

        public void OnDestroy()
        {
            UnInit();
        }

        private void UnInit()
        {
            AddYourOwnBtn.onClick.RemoveListener(OnAddYourOwnBtnClicked);
            BackButton.onClick.RemoveListener(OnBackBtnClicked);
            UnlockAllButton.onClick.RemoveListener(OnUnlockAllBtnClicked);
        }

        public void Awake()
        {
            Init();
        }

        public void Init()
        {
            ZBug.Info("Character Select", "Init");
            AddYourOwnBtn.onClick.AddListener(OnAddYourOwnBtnClicked);
            BackButton.onClick.AddListener(OnBackBtnClicked);
            UnlockAllButton.onClick.AddListener(OnUnlockAllBtnClicked);

            
            StartCoroutine(nameof(LoadCharacters));
        }

        private void OnBackBtnClicked()
        {
            Send.Msg(new CloseWinMsg
            {
                
            });
        }

        private void OnUnlockAllBtnClicked()
        {
            Cheat.With(CheatType.UNLOCK_ALL_CHARACTERS);
            
            // StartCoroutine(nameof(LoadCharacters));
            LoadCharacters();
        }

        public void LoadCharacters()
        {
            ZBug.Info("Character Select", "Loading characters");
            // clear existing
            // TODO: recycle existing / reset but don't destroy
            // pooling
            foreach (Transform child in OptionsContainer.GetChildren())
            {
                if (child == AddYourOwnBtn.transform)
                {
                    continue;
                }
                
                if (child == Template.transform)
                {
                    continue;
                }
                
                Destroy(child.gameObject);
            }

            List<(CharacterConfig config, bool isAvailable)> list = new List<(CharacterConfig config, bool isAvailable)>();
            
            foreach (var config in NohoConfigResolver.GetConfigs<CharacterConfig>())
            {
                if (!config.HasEpisodeTrack)
                {
                    continue;
                }

                int unlockedEpisode = App.Instance.PersistentData.GetUnlockedCharacterEpisode(config);

                // TextAsset firstEpisode;
                // if (unlockedEpisode == 0)
                // {
                //     firstEpisode = null;
                // }
                // else
                // {
                //     string episode = $"Episode_{unlockedEpisode:000}";
                //     ZBug.Info("Character Select", $"Loading character {config.DevName} episode: {episode}");
                //     var request = ZSource.LoadAsync<TextAsset>(NohoResourcePack.FOLDER_SCRIPTS, config.DevName, episode);
                //
                //     yield return request;
                //     
                //     firstEpisode = request.asset as TextAsset;
                //     
                //     ZBug.Info("Character Select", $"Loading character {config.DevName} episode, success?: {firstEpisode != null}");
                //     // firstEpisode = ZSource.Load<TextAsset>(NohoResourcePack.FOLDER_SCRIPTS, config.DevName, $"Episode_{unlockedEpisode:000}");
                // }

                list.Add((config, config.IsAvailable && unlockedEpisode > 0));
                //
                // if (collection.TextAssets.Length == 0)
                // {
                //     continue;
                // }
            }

            list.Sort((a, b) => (b.isAvailable).CompareTo(a.isAvailable));

            foreach (var item in list)
            {
                var option = Instantiate(Template, OptionsContainer);

                bool isAvailable = item.isAvailable;
                option.Init(item.config, isAvailable, () =>
                {
                    MsgMgr.Instance.Send(new ShowWinMsg
                    {
                        Win = Win.EPISODE_SELECT
                    });
                    MsgMgr.Instance.Send(new LoadCharacterEpisodesMsg
                    {
                        CharacterDevName = item.config.DevName
                    });
                    // AppManager.Instance.UnlockNextEpisode(item.character.DevName);
                    //
                    // var episode = item.Item2;
                    //
                    // AppManager.Instance.StartEpisode(episode);
                });
                
                option.Show();
            }
            
            // AddYourOwnBtn.transform.par
            AddYourOwnBtn.transform.SetToLastSibling();
        }

        private void OnAddYourOwnBtnClicked()
        {
            ZBug.Info("Clicked Add Your Own!");
            
            FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );

            // Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
            // It is sufficient to add a quick link just once
            // Name: Users
            // Path: C:\Users
            // Icon: default (folder icon)
            FileBrowser.AddQuickLink( "Users", "C:\\Users", null );

            // Show a save file dialog 
            // onSuccess event: not registered (which means this dialog is pretty useless)
            // onCancel event: not registered
            // Save file/folder: file, Initial path: "C:\", Title: "Save As", submit button text: "Save"
            // FileBrowser.ShowSaveDialog( null, null, false, "C:\\", "Save As", "Save" );

            // Show a select folder dialog 
            // onSuccess event: print the selected folder's path
            // onCancel event: print "Canceled"
            // Load file/folder: folder, Initial path: default (Documents), Title: "Select Folder", submit button text: "Select"
            // FileBrowser.ShowLoadDialog( (path) => { Debug.Log( "Selected: " + path ); }, 
            //                                () => { Debug.Log( "Canceled" ); }, 
            //                                true, null, "Select Folder", "Select" );

            // Coroutine example
            // StartCoroutine( ShowLoadDialogCoroutine() );
            StartCoroutine( StoryPackLoader.ShowLoadDialogCoroutine(() =>
            {
                StartCoroutine(nameof(LoadCharacters));
            }) );
        }

        // private IEnumerator ShowLoadDialogCoroutine()
        // {
        //     
        //     // Show a load file dialog and wait for a response from user
        //     // Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
        //     // yield return FileBrowser.WaitForLoadDialog( false, null, "Load File", "Load" );
        //     yield return FileBrowser.WaitForLoadDialog( true, null, "Load Resource Pack Folder", "Load" );
        //
        //     // Dialog is closed
        //     // Print whether a file is chosen (FileBrowser.Success)
        //     // and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
        //     ZBug.Info( FileBrowser.Success + " " + FileBrowser.Result );
			     //
        //     if( FileBrowser.Success )
        //     {
        //         if (!FileBrowserHelpers.IsDirectory(FileBrowser.Result))
        //         {
        //             ZBug.Warn("FILE", "Expected a folder selection, found a file");
        //             yield break;
        //         }
        //
        //         var resourcePack = NohoResourcePack.Load(FileBrowser.Result);
        //
        //         foreach (string background in resourcePack.Backgrounds)
        //         {
        //             ZBug.Info("FILE", $"Found Background: {background} ...");
        //         }
				    //
        //         foreach (string character in resourcePack.CharacterDirs)
        //         {
        //             ZBug.Info("FILE", $"Found Character: {character} ...");
        //         }
				    //
        //         foreach (string character in resourcePack.ScriptDirs)
        //         {
        //             ZBug.Info("FILE", $"Found Script: {character} ...");
        //         }
        //
        //         ZSource.AddResourcePack(resourcePack);
        //         
        //         NohoConfigResolver.ReInit();
        //         
        //         App.Instance.ScriptParser = ScriptParserFactory.DefaultParser();
        //
        //         StartCoroutine(nameof(LoadCharacters));
        //
        //         // FileBrowserHelpers.direct
        //         // If a file was chosen, read its bytes via FileBrowserHelpers
        //         // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
        //         // byte[] bytes = FileBrowserHelpers.ReadBytesFromFile( FileBrowser.Result );
        //     }
        //     else
        //     {
        //         ZBug.Warn("FILE", "Failed to load file");
        //     }
        // }
    }
}