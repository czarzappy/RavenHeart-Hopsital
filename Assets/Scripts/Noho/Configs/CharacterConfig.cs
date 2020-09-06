using System;
using System.Collections.Generic;
using UnityEngine;
using ZEngine.Core.Configs;
using ZEngine.Core.Localization;
using ZEngine.Core.Math;
using ZEngine.Unity.Core.Models;

namespace Noho.Configs
{
    // Bigger numbers will show in front
    public enum CharacterSize
    {
        SMALL = 100,
        MEDIUM = 10,
        LARGE = 1
    }
    
    [Serializable]
    public class CharacterConfig : MasterConfigItem
    {
        [Obsolete]
        public LocId Name;
        public string RawName;
        [Obsolete]
        public LocId Desc;

        public ZColor CharacterNameBackgroundColor;

        public CharacterSize Size;

        public string DefaultPoseDevName;
        public List<CharacterPose> Poses;
        public LocId Role;
        public string RawRole;

        public bool HasEpisodeTrack = false;
        public ResourcePath FaceSpritePath;
        public bool FirstEpisodeUnlocked;
        public bool IsAvailable = true;

        public CharacterPose GetDefaultPose()
        {
            return GetPose(DefaultPoseDevName);
        }
        
        public string[] DefaultSpritePath
        {
            get
            {
                var pose = GetDefaultPose();

                return GetPoseSpritePath(pose);
            }
        }

        public string[] GetPoseSpritePath(CharacterPose pose)
        {
            if (pose.SpritePath.Length == 1)
            {
                return new[] {"Characters", DevName, pose.SpritePath[0]};
            }

            return pose.SpritePath;
        }

        public CharacterPose GetPose(string posName)
        {
            foreach (CharacterPose pose in Poses)
            {
                if (pose.ExpressionDevName.Equals(posName, StringComparison.OrdinalIgnoreCase))
                {
                    return pose;
                }
            }

            ZBug.Warn("CharacterConfig", $"Pose: {posName} not found!");
            return null;
        }

        public string[] GetPoseSpritePath(string expressionDevName)
        {
            CharacterPose pose = GetPose(expressionDevName);

            if (pose == null)
            {
                pose = GetDefaultPose();
            }
            
            return GetPoseSpritePath(pose);
        }
    }

    [Serializable]
    public class CharacterPose
    {
        public string ExpressionDevName;
        
        // Fully qualified resource asset path
        // Or
        // Local resource path
        public string[] SpritePath;
    }
}
