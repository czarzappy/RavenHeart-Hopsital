using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace ZEngine.Unity.Core.IO
{
    public static class ZFile
    {
        public static Texture2D LoadPNG(string filePath) 
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            
            var fileData = File.ReadAllBytes(filePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            return tex;
        }
        
        public static TextAsset LoadTextAsset(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
                                
            string text = File.ReadAllText(path);
            TextAsset textAsset = new TextAsset(text)
            {
                name = Path.GetFileNameWithoutExtension(path),
            };

            return textAsset;
        }
        
        static async Task<AudioClip> LoadClip(string path)
        {
            AudioClip clip = null;
            using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV))
            {
                uwr.SendWebRequest();
 
                // wrap tasks in try/catch, otherwise it'll fail silently
                try
                {
                    while (!uwr.isDone) await Task.Delay(5);
 
                    if (uwr.isNetworkError || uwr.isHttpError) Debug.Log($"{uwr.error}");
                    else
                    {
                        clip = DownloadHandlerAudioClip.GetContent(uwr);
                    }
                }
                catch (Exception err)
                {
                    Debug.Log($"{err.Message}, {err.StackTrace}");
                }
            }
 
            return clip;
        }

        public static AudioClip LoadAudioClip(string path, AudioType audioType = AudioType.OGGVORBIS)
        {
            AudioClip clip = null;
            using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip($"file://{path}", audioType))
            {
                uwr.SendWebRequest();
 
                // wrap tasks in try/catch, otherwise it'll fail silently
                try
                {
                    while (!uwr.isDone)
                    {
                    }

                    if (uwr.isNetworkError || uwr.isHttpError)
                    {
                        ZBug.Error("ZFILE", $"Path: {path}, type: {audioType}, Error: {uwr.error}");
                    }
                    else
                    {
                        clip = DownloadHandlerAudioClip.GetContent(uwr);
                    }
                }
                catch (Exception err)
                {
                    ZBug.Error("ZFILE", $"Path: {path}, type: {audioType}, Error: {err.Message}, {err.StackTrace}");
                }
            }
 
            return clip;
        }
    }
}