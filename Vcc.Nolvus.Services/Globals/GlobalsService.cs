﻿using System;
using System.Management;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Services.Globals
{
    public class GlobalsService : IGlobalsService
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(
             string deviceName, int modeNum, ref DEVMODE devMode);
        const int ENUM_CURRENT_SETTINGS = -1;

        const int ENUM_REGISTRY_SETTINGS = -2;

        [System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        public const string NolvusSection = "Nolvus";
        public const string UserName = "UserName";
        public const string Password = "Password";
        public const string Version = "Version";
        public const string NolvusApi = "https://www.nolvus.net/rest/";                
        public const string NexusSection = "Nexus";
        public const string ApiKey = "ApiKey";
        public const string UserAgent = "UserAgent";
        public const string MegaSection = "Mega";
        public const string MegaUserName = "Email";
        public const string MegaPswd = "Password";
        public const string MegaAnonymous = "AnonymousConnection";



        public string ApiVersion
        {
            get
            {
                return ServiceSingleton.Settings.GetIniValue(NolvusSection, Version);
            }
        }

        public string ApiUrl
        {
            get
            {
                return NolvusApi;
            }
        }

        public string NolvusUserName
        {
            get
            {
                return ServiceSingleton.Settings.GetIniValue(NolvusSection, UserName);
            }
        }

        public string NolvusPassword
        {
            get
            {
                return ServiceSingleton.Settings.GetIniValue(NolvusSection, Password);
            }            
        }

        public bool MegaAnonymousConnection
        {
            get
            {
                var Anonymous = ServiceSingleton.Settings.GetIniValue(MegaSection, MegaAnonymous);
                return Anonymous == null ? true : System.Convert.ToBoolean(Anonymous);                
            }
            set
            {
                ServiceSingleton.Settings.StoreIniValue(MegaSection, MegaAnonymous, value.ToString());
            }
        }

        public string MegaEmail
        {
            get
            {
                var Email = ServiceSingleton.Settings.GetIniValue(MegaSection, MegaUserName);
                return Email == null ? string.Empty : Email;                
            }
            set
            {
                ServiceSingleton.Settings.StoreIniValue(MegaSection, MegaUserName, value);
            }
        }        

        public string MegaPassword
        {
            get
            {
                var Password = ServiceSingleton.Settings.GetIniValue(MegaSection, MegaPswd);

                return Password == null ? string.Empty : ServiceSingleton.Lib.DecryptString(Password);
            }
            set
            {
                ServiceSingleton.Settings.StoreIniValue(MegaSection, MegaPswd, ServiceSingleton.Lib.EncryptString(value));
            }
        }

        public string NexusApiKey
        {
            get
            {
                return ServiceSingleton.Settings.GetIniValue(NexusSection, ApiKey);
            }
        }

        public string NexusUserAgent
        {
            get
            {
                return ServiceSingleton.Settings.GetIniValue(NexusSection, UserAgent);
            }
        }

        public string NolvusUserAgent
        {
            get
            {
                try
                {
                    var UsrAgent = ServiceSingleton.Settings.GetIniValue(NolvusSection, UserAgent);

                    if (UsrAgent == null || UsrAgent == string.Empty)
                    {
                        ServiceSingleton.Settings.StoreIniValue(NolvusSection, UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36 Edge/18.19582");
                        return ServiceSingleton.Settings.GetIniValue(NolvusSection, UserAgent);
                    }
                    else
                    {
                        return UsrAgent;
                    }
                }
                catch
                {                    
                    return "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36 Edge/18.19582";
                }
            }
        }

        public List<string> WindowsResolutions
        {
            get
            {
                List<string> Result = new List<string>();

                DEVMODE vDevMode = new DEVMODE();
                int i = 0;
                while (EnumDisplaySettings(null, i, ref vDevMode))
                {
                    string Reso = string.Format("{0}x{1}", vDevMode.dmPelsWidth, vDevMode.dmPelsHeight);

                    if (!Result.Contains(Reso))
                    {
                        Result.Add(Reso);
                    }
                    i++;
                }

                return Result;
            }

        }

        public List<string> GetVideoAdapters()
        {
            try
            {                
                ManagementObjectCollection GPUs = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController").Get();

                var Result = new List<string>();

                foreach (ManagementObject GPU in GPUs)
                {
                    PropertyData MinRefreshRate = GPU.Properties["MinRefreshRate"];
                    PropertyData Description = GPU.Properties["Description"];

                    if (MinRefreshRate != null && Description != null)
                    {
                        if (MinRefreshRate.Value != null) Result.Add(Description.Value.ToString().ToUpper());
                    }
                }

                return Result;
            }
            catch
            {
                var List = new List<string>();

                List.Add("GPU info not found");

                return List;
            }            
        }

        public async Task<string> GetCPUInfo()
        {
            return await Task.Run(() =>
            {
                try
                {                    
                    string Result = string.Empty;

                    using (ManagementObjectSearcher Win32Proc = new ManagementObjectSearcher("select * from Win32_Processor"))
                    {
                        foreach (ManagementObject Obj in Win32Proc.Get())
                        {
                            Result = Obj["Name"].ToString();
                            break;
                        }
                    };

                    return Result;
                }
                catch
                {
                    return "CPU info not found";
                }
            });                     
        }

        public async Task<string> GetRamCount()
        {
            return await Task.Run(() =>
            {
                try
                {                    
                    int Result = 0;

                    using (ManagementObjectSearcher Win32Proc = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                    {
                        foreach (ManagementObject Obj in Win32Proc.Get())
                        {
                            Result = System.Convert.ToInt32(Math.Ceiling(System.Convert.ToDouble(Obj["TotalVisibleMemorySize"]) / 1024 / 1024));
                        }
                    }

                    return Result.ToString();
                }
                catch
                {
                    return "RAM count not found";
                }
            });        
        }

        public string GetVersion(string FilePath)
        {
            string v = FileVersionInfo.GetVersionInfo(FilePath).ProductVersion;
            return v.Substring(0, v.LastIndexOf('.'));
        }
    }
}        
