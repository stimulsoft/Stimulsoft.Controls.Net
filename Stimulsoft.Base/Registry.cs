#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports												}
{																	}
{	Copyright (C) 2003-2016 Stimulsoft     							}
{	ALL RIGHTS RESERVED												}
{																	}
{	The entire contents of this file is protected by U.S. and		}
{	International Copyright Laws. Unauthorized reproduction,		}
{	reverse-engineering, and distribution of all or any portion of	}
{	the code contained in this file is strictly prohibited and may	}
{	result in severe civil and criminal penalties and will be		}
{	prosecuted to the maximum extent possible under the law.		}
{																	}
{	RESTRICTIONS													}
{																	}
{	THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES			}
{	ARE CONFIDENTIAL AND PROPRIETARY								}
{	TRADE SECRETS OF Stimulsoft										}
{																	}
{	CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON		}
{	ADDITIONAL RESTRICTIONS.										}
{																	}
{*******************************************************************}
*/
#endregion Copyright (C) 2003-2016 Stimulsoft

using System;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text;

namespace Stimulsoft.Base
{
    public class Registry
    {
        public static string GetValue(string name)
        {
            try
            {
                string path = null;

                //Using Net Framework 4 methods
                try
                {
                    RegistryKey registryKey;
                    RegistryKey key2 = null;
                    if (Environment.Is64BitOperatingSystem == true)
                    {
                        registryKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                        key2 = registryKey.OpenSubKey("SOFTWARE\\Stimulsoft\\Stimulsoft Reports");
                    }                    
                    if (key2 == null)
                    {
                        registryKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
                        key2 = registryKey.OpenSubKey("SOFTWARE\\Stimulsoft\\Stimulsoft Reports");
                    }
                    if (key2 != null) path = key2.GetValue(name) as string;
                    if (path != null) return path;
                }
                catch
                {
                }
                
                //Using old methods
                try
                {
                    RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Stimulsoft\\Stimulsoft Reports");
                    if (key != null)
                        path = key.GetValue(name) as string;

                    if (path != null)
                        return path;
                }
                catch
                {
                }

                string value32 = Registry.RegistryWOW6432.GetRegKey32(Registry.RegHive.HKEY_LOCAL_MACHINE, @"SOFTWARE\Stimulsoft\\Stimulsoft Reports", name);
                if (value32 != null)
                    return value32;

                string value64 = Registry.RegistryWOW6432.GetRegKey64(Registry.RegHive.HKEY_LOCAL_MACHINE, @"SOFTWARE\Stimulsoft\\Stimulsoft Reports", name);
                return value64;
            }
            catch
            {
            }
            return null;
        }

        public enum RegSAM
        {
            QueryValue = 0x0001,
            SetValue = 0x0002,
            CreateSubKey = 0x0004,
            EnumerateSubKeys = 0x0008,
            Notify = 0x0010,
            CreateLink = 0x0020,
            WOW64_32Key = 0x0200,
            WOW64_64Key = 0x0100,
            WOW64_Res = 0x0300,
            Read = 0x00020019,
            Write = 0x00020006,
            Execute = 0x00020019,
            AllAccess = 0x000f003f
        }

        public static class RegHive
        {
            internal static UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(0x80000002u);
            internal static UIntPtr HKEY_CURRENT_USER = new UIntPtr(0x80000001u);
        }

        public static class RegistryWOW6432
        {
            #region Member Variables
            #region Read 64bit Reg from 32bit app
            [DllImport("Advapi32.dll")]
            static extern uint RegOpenKeyEx(
                UIntPtr hKey,
                string lpSubKey,
                uint ulOptions,
                int samDesired,
                out UIntPtr phkResult);

            [DllImport("Advapi32.dll")]
            static extern uint RegCloseKey(UIntPtr hKey);

            [DllImport("advapi32.dll", EntryPoint = "RegQueryValueEx")]
            internal static extern int RegQueryValueEx(
                UIntPtr hKey, string lpValueName,
                int lpReserved,
                ref uint lpType,
                System.Text.StringBuilder lpData,
                ref uint lpcbData);
            #endregion
            #endregion

            #region Functions
            internal static string GetRegKey64(UIntPtr inHive, String inKeyName, String inPropertyName)
            {
                return GetRegKey64(inHive, inKeyName, RegSAM.WOW64_64Key, inPropertyName);
            }

            internal static string GetRegKey32(UIntPtr inHive, String inKeyName, String inPropertyName)
            {
                return GetRegKey64(inHive, inKeyName, RegSAM.WOW64_32Key, inPropertyName);
            }

            internal static string GetRegKey64(UIntPtr inHive, String inKeyName, RegSAM in32or64key, String inPropertyName)
            {
                //UIntPtr HKEY_LOCAL_MACHINE = (UIntPtr)0x80000002;
                UIntPtr hkey = UIntPtr.Zero;

                try
                {
                    uint lResult = RegOpenKeyEx(RegHive.HKEY_LOCAL_MACHINE, inKeyName, 0, (int)RegSAM.QueryValue | (int)in32or64key, out hkey);
                    if (0 != lResult) return null;
                    uint lpType = 0;
                    uint lpcbData = 1024;
                    StringBuilder AgeBuffer = new StringBuilder(1024);
                    RegQueryValueEx(hkey, inPropertyName, 0, ref lpType, AgeBuffer, ref lpcbData);
                    string Age = AgeBuffer.ToString();
                    return Age;
                }
                finally
                {
                    if (UIntPtr.Zero != hkey) RegCloseKey(hkey);
                }
            }
            #endregion
        }
    }
}
