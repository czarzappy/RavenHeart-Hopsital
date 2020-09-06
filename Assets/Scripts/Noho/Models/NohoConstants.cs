namespace Noho.Models
{
    public static class NohoConstants
    {
        public static uint STEAM_APP_ID = 1305280;
        public static uint STEAM_DEMO_APP_ID = 1305520;

        public enum ToolType
        {
            FORCEPS,
            JUICE,
            BANDAGE,
            SUTURE,
            SCALPEL,
            SUCTION,
            INJECTION
        }

        public static class GOTags
        {

            /// <summary>
            /// Tool Tags
            /// </summary>
            public const string SUTURE = "SUTURE";
            public const string JUICE = "JUICE";
            public const string SCALPEL = "SCALPEL";
            public const string TRAY = "TRAY";
            public const string DEFIBRILLATOR = "DEFIBRILLATOR";
            
            /// <summary>
            /// Collider associated with the suction tool
            /// </summary>
            public const string SUCTION = "SUCTION";
            
            /// <summary>
            /// Misc Tags
            /// </summary>
            public const string FAILBOUND = "FAILBOUND";
            
            /// <summary>
            /// Gunk Tags
            /// </summary>
            public const string SMALLCUT = "SMALLCUT";
            public const string LARGECUT = "LARGECUT";
            public const string GLASS = "GLASS";
            public const string OBSTRUCTION = "OBSTRUCTION";
            public const string HEMORRHAGE = "HEMORRHAGE";
            public const string SPAWNER_HEMORRHAGE = "SPAWNER_HEMORRHAGE";
            public const string SYRINGE = "SYRINGE";
            public const string GLASSWOUND = "GLASSWOUND";
            public const string INCISION_AREA = "INCISION_AREA";
            public const string BANDAGE_CUT = "BANDAGE_CUT";
            public const string BANDAGE = "BANDAGE";
        }

        public static class SceneNames
        {
            public const string POST_OP_SCENE = "PostOpScene";
            public const string DIALOGUE_SCENE = "PureDialogueScene";
            public const string SURGERY_SCENE = "SurgeryScene";
        }

        public enum ProtagBaseModel
        {
            A,
            M,
            F,
        }
    }
}