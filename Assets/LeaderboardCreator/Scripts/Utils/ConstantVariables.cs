using Dan.Enums;

namespace Dan
{
    internal static class ConstantVariables
    {
        internal const string PlayerPrefsGuidKey = "LEADERBOARD_CREATOR___LOCAL_GUID";
        
        internal static string GetServerURL(Routes route = Routes.None, string extra = "")
        {
            return ServerURL + (route == Routes.Authorize ? "/authorize" :
                route == Routes.Get ? "/get" :
                route == Routes.Upload ? "/entry/upload" :
                route == Routes.UpdateUsername ? "/entry/update-username" :
                route == Routes.DeleteEntry ? "/entry/delete" : "/") + extra;
        }

        private const string ServerURL = "https://lcv2-server.danqzq.games";
    }
}