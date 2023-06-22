using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Vcc.Nolvus.Core.Interfaces;

namespace Vcc.Nolvus.Services.Game
{
    public class GameService : IGameService
    {
        public const string SkyrimSE = "The Elder Scrolls V: Skyrim Special Edition";

        private static RegistryKey GetInstalledApplication(string AppName)
        {
            string displayName;
            RegistryKey key;

            key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            string[] Arr1 = key.GetSubKeyNames();
            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (AppName.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return subkey;
                }
            }

            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            string[] Arr2 = key.GetSubKeyNames();
            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (AppName.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return subkey;
                }
            }

            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            string[] Arr3 = key.GetSubKeyNames();
            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (AppName.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return subkey;
                }
            }

            return null;
        }

        public bool IsGameInstalled()
        {
            return GetInstalledApplication(SkyrimSE) != null;
        }

        public string GetSkyrimSEDirectory()
        {
            if (IsGameInstalled())
            {
                return GetInstalledApplication(SkyrimSE).GetValue("InstallLocation") as string;
            }

            return string.Empty;            
        }
    }
}
