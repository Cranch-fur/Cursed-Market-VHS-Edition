using System;
using System.IO;

namespace VHS_Cursed_Market
{
    public static class Globals
    {
        public static readonly string SelfExecutableName = AppDomain.CurrentDomain.FriendlyName;
        public static readonly string SelfExecutableFriendlyName = SelfExecutableName.Remove(Globals.SelfExecutableName.Length - 4, 4);
        public static readonly string SelfDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Cursed Market VHS";
        public static void EnsureSelfDataFolderExists()
        {
            if (Directory.Exists(SelfDataFolder) == false)
                Directory.CreateDirectory(SelfDataFolder);
        }


        public const string OfflineVersion = "1000";

        public static string marketFile = null;
    }
}
