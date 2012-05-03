
using System;
using Microsoft.Win32;
using System.IO;


namespace BarfieldCleaner {

    /// <summary>
    /// Registry Repair and Cleaning Class
    /// http://support.microsoft.com/kb/256986
    /// </summary>
    internal static partial class RegistryCleaner {

        /// <summary>
        /// Corrects erroneous HKEY_LOCAL_MACHINE settings
        /// http://technet.microsoft.com/en-us/library/cc959046.aspx
        /// </summary>
        private static void CleanLocalMachine() {
            ConfigureSoftwareHive();
            ConfigureSystemHive();
        }

        #region HKEY_LOCAL_MACHINE\System

        /// <summary>
        /// 
        /// </summary>
        private static void ConfigureSystemHive() {

            Log.AppendHeader( logfile, "HKEY_LOCAL_MACHINE\\SYSTEM" );

            // --------------------------------------------------------------------------------
            // SYSTEM
            // Reference: http://technet.microsoft.com/en-us/library/cc976042.aspx
            // --------------------------------------------------------------------------------

            //[HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control]
            //"WaitToKillServiceTimeout"="1000"
            SetValue( Registry.LocalMachine, @"SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "1000" );

            // [HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl]
            // "AutoReboot"=dword:00000000
            SetValue( Registry.LocalMachine, @"SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0 );

            // [HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters]
            // "EnablePrefetcher"=dword:00000002
            SetValue( Registry.LocalMachine, @"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnablePrefetcher", 2 );

            //[HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager]
            //"AutoChkTimeOut"=dword:00000002
            SetValue( Registry.LocalMachine, @"SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeOut", 2 );

            // [HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management]
            // "ClearPageFileAtShutdown"=dword:00000000
            // "DisablePagingExecutive"=dword:00000001
            // "SecondLevelDataCache"=dword:00000200
            SetValue( Registry.LocalMachine, @"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "ClearPageFileAtShutdown", 0 );
            SetValue( Registry.LocalMachine, @"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive", 1 );
            SetValue( Registry.LocalMachine, @"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SecondLevelDataCache", 512 );

        }

        #endregion

    }
}
