using System;
using UnityEditor;
using UnityEngine;

public class WebGLBuilder 
{
    private const string OUTPUT_KEYWORD = "-zarg:output";
    
    public static void build() {
        // string[] scenes = {"Assets/main.unity"};
        
        string[] args = System.Environment.GetCommandLineArgs();
        
        BuildPlayerOptions options = new BuildPlayerOptions();

        string output = null;
        Debug.Log(string.Join(", ", args));
        for (int i = 0; i < args.Length; i++)
        {
            Debug.Log($"[{i}] {args[i]}");
            switch (args[i])
            {
                case OUTPUT_KEYWORD:
                    output = args[i + 1];
                    break;
            }
        }

        if (output == null)
        {
            throw new ArgumentException($"Expected argument, {OUTPUT_KEYWORD} LOCATION_PATH_NAME");
        }
        
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
        string[] scenes = new string[sceneCount];
        for( int i = 0; i < sceneCount; i++ )
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension( UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( i ) );
        }

        options.target = BuildTarget.WebGL;
        options.locationPathName = output;
        options.scenes = scenes;
        
        BuildPipeline.BuildPlayer(options);
    }
}