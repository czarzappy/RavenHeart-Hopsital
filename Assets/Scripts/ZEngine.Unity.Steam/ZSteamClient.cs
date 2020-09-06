using Steamworks;

namespace ZEngine.Unity.Steam
{
    public static class ZSteamClient
    {
        public static bool IsValid => SteamClient.IsValid;

        public static void Init(uint steamAppId)
        {
            SteamClient.Init(steamAppId);
        }

        public static void Shutdown()
        {
            SteamClient.Shutdown();
        }
    }
}
