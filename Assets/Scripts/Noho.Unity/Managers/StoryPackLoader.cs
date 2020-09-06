using System;
using System.Collections;
using Noho.Configs;
using Noho.Factories;
using Noho.Unity.Models;
using SimpleFileBrowser;
using ZEngine.Unity.Core;

namespace Noho.Unity.Managers
{
    public static class StoryPackLoader
    {
        
        public static IEnumerator ShowLoadDialogCoroutine(Action onLoaded)
        {
            
            // Show a load file dialog and wait for a response from user
            // Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
            // yield return FileBrowser.WaitForLoadDialog( false, null, "Load File", "Load" );
            yield return FileBrowser.WaitForLoadDialog( true, null, "Load Resource Pack Folder", "Load" );

            // Dialog is closed
            // Print whether a file is chosen (FileBrowser.Success)
            // and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
            ZBug.Info( FileBrowser.Success + " " + FileBrowser.Result );
			
            if( FileBrowser.Success )
            {
                if (!FileBrowserHelpers.IsDirectory(FileBrowser.Result))
                {
                    ZBug.Warn("FILE", "Expected a folder selection, found a file");
                    yield break;
                }

                var resourcePack = NohoResourcePack.Load(FileBrowser.Result);

                foreach (string background in resourcePack.BGs)
                {
                    ZBug.Info("FILE", $"Found Background: {background} ...");
                }
                
                foreach (string bgms in resourcePack.BGMs)
                {
                    ZBug.Info("FILE", $"Found BGMS: {bgms} ...");
                }
				
                foreach (string character in resourcePack.CharacterDirs)
                {
                    ZBug.Info("FILE", $"Found Character: {character} ...");
                }
				
                foreach (string character in resourcePack.ScriptDirs)
                {
                    ZBug.Info("FILE", $"Found Script: {character} ...");
                }

                ZSource.AddResourcePack(resourcePack);
                
                NohoConfigResolver.ReInit();
                
                App.Instance.ScriptParser = ScriptParserFactory.DefaultParser();
                
                onLoaded();

                // FileBrowserHelpers.direct
                // If a file was chosen, read its bytes via FileBrowserHelpers
                // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
                // byte[] bytes = FileBrowserHelpers.ReadBytesFromFile( FileBrowser.Result );
            }
            else
            {
                ZBug.Warn("FILE", "Failed to load file");
            }
        }
    }
}