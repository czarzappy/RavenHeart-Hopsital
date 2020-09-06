
// #define SHIP_BUILD

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZEngine.Unity.Core.Logging;
using ILogger = ZEngine.Unity.Core.Logging.ILogger;

// ReSharper disable CheckNamespace

public static class ZBug
{
    private const string DEFAULT_CATEGORY = "MISC";
    private const string DEFAULT_OBJECT_MSG = "null";

    private static readonly ILogger DEFAULT_LOGGER;
    private static readonly ICategoryLogger CAT_LOGGER;

    private static string FormatMessage(string cat, string message)
    {
        return $"[Frame: {Time.frameCount}] [SCENE: {SceneManager.GetActiveScene().name}] [{cat}] {message}";
    }
    
    static ZBug()
    {
        var logger = new AggregateLogger();

        File.WriteAllText(Path.Combine(Application.persistentDataPath, "test.log"), "This is a text");
        // File.WriteAllText(Path.Combine(Application.dataPath, "test.log"), "This is a text");

#if UNITY_EDITOR && !SHIP_BUILD
        logger.Register(new UnityLogger());
#endif
        logger.Register(new FileLogger(Path.Combine(Application.persistentDataPath, "game.log")));
        
        CAT_LOGGER = new FilteredCategoryLogger(logger,
            FormatMessage,
            new HashSet<string>
            {
                DEFAULT_CATEGORY,
                "Dialogue",
                // "Operation",
                // "Character",
                "MSGMGR",
                "SMALLCUT",
                // "GUNK",
                "ZATTR",
                "TextTick",
                "BRAIN",
            },
            new HashSet<string>
            {
            
            });

        DEFAULT_LOGGER = new DefaultCategoryLogger(DEFAULT_CATEGORY, CAT_LOGGER);
        
        ZLog.Register(DEFAULT_LOGGER); // for DLL logging
    }
    
    public static void Info(string message)
    {
        DEFAULT_LOGGER.Info(message);
    }
    public static void Info(string cat, string message)
    {
        CAT_LOGGER.Info(cat, message);
    }
    
    public static void Info(string cat, object message)
    {
        Info(cat, message == null ? DEFAULT_OBJECT_MSG : message.ToString());
    }

    public static void Info(object message)
    {
        Info(message == null ? DEFAULT_OBJECT_MSG : message.ToString());
    }
    
    public static void Warn(string message)
    {
        DEFAULT_LOGGER.Warn(message);
    }
    
    public static void Warn(string cat, string message)
    {
        CAT_LOGGER.Warn(cat, message);
    }
    
    public static void Error(string message)
    {
        DEFAULT_LOGGER.Error(message);
    }
    
    public static void Error(string cat, string message)
    {
        CAT_LOGGER.Error(cat, message);
    }
    
    public static void Ex(Exception exception)
    {
        DEFAULT_LOGGER.Ex(exception);
    }
    
    public static void Ex(string cat, Exception exception)
    {
        CAT_LOGGER.Ex(cat, exception);
    }
}
