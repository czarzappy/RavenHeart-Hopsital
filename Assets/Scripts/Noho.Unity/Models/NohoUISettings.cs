namespace Noho.Unity.Models
{
    public static class NohoUISettings 
    {
        public static float OPERATION_TICK_DELAY = 0f;
        public static float OPERATION_TICK_DURATION = .1f;
        public static float LOVE_DISPLAY_DURATION = 1f;

        public const float BRAIN_FADE_OUT = .5f;
        public const float BRAIN_FADE_IN = BRAIN_FADE_OUT;
        public const float BRAIN_FADE_IN_DELAY = 1f;
        
        public const float MIN_FRAGMENT_GOAL_DISTANCE = 0.25f;
        public const float SPAWN_HEMORRHAGE_DELAY = 5f;
        public const float INCISION_CUT_DURATION = 0.25f;
        public const float INCISION_JUICE_DURATION = 0.25f;
        
        public const float TRAY_DUMP_GUNK_DURATION = 1f;
        
        public const float BROKEN_FRAGMENT_FAIL_RESET_DURATION = 0.025f;
        public const float GENERAL_FAIL_RESET_DURATION = 0.025f;
        public const float GLASS_SHARD_FAIL_RESET_DURATION = 0.025f;
        
        public const float CHARACTER_NAME_SHIFT_DURATION = .5f;
        
        public const float CHARACTER_NAME_SHIFT_SPEED = 2800f;
        
        public const float CHARACTER_FADE_IN_DURATION = 0.5f;
        public const float CHARACTER_WALK_IN_DURATION = 0.5f;
    }
}