
using System;
using Microsoft.Win32;
using System.IO;


namespace BarfieldCleaner {

    /// <summary>
    /// Registry Repair and Cleaning Class
    /// http://support.microsoft.com/kb/256986
    /// </summary>
    internal static partial class RegistryCleaner {

        #region HKEY_LOCAL_MACHINE\Software

        /// <summary>
        /// 
        /// </summary>
        private static void ConfigureSoftwareHive() {
            Software();
            Software_Classes();
            Software_Microsoft();
            Software_Policies();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void Software() {

            Log.AppendHeader( logfile, "HKEY_LOCAL_MACHINE\\SOFTWARE" );

            //HKEY_LOCAL_MACHINE\Software\Fast Browser Search
            DeleteTree( Registry.LocalMachine, @"Software\Fast Browser Search" );

        }

        /// <summary>
        /// 
        /// </summary>
        private static void Software_Classes() {

            Log.AppendHeader( logfile, "HKEY_LOCAL_MACHINE\\SOFTWARE\\CLASSES" );

            // Add "Open with notepad" to all files
            CreateKey( Registry.LocalMachine, @"Software\Classes\*\shell" );
            SetValue( Registry.LocalMachine, @"Software\Classes\*\shell", "", "\"notepad.exe %1\"" );
            CreateKey( Registry.LocalMachine, @"Software\Classes\*\shell\open" );
            SetValue( Registry.LocalMachine, @"Software\Classes\*\shell\open", "", "Open &with notepad" );
            CreateKey( Registry.LocalMachine, @"Software\Classes\*\shell\open\command" );
            SetValue( Registry.LocalMachine, @"Software\Classes\*\shell\open\command", "", "notepad.exe %1" );

            // .exe fix
            DeleteTree( Registry.LocalMachine, @"Software\Classes\.exe\shell" );
            SetValue( Registry.LocalMachine, @"Software\Classes\.exe", "", "exefile" ); // Default value
            SetValue( Registry.LocalMachine, @"Software\Classes\.exe", "Content Type", "application/x-msdownload" );
            DeleteTree( Registry.LocalMachine, @"Software\Classes\secfile" );

            // 
            //PruneCOMClasses();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void Software_Microsoft() {

            Log.AppendHeader( logfile, "HKEY_LOCAL_MACHINE\\SOFTWARE\\MICROSOFT" );

            // [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction]
            // "Enable"="Y"
            SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "Y" );

            //[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PCHealth\ErrorReporting]
            //"AllOrNone"=dword:00000001
            //"DoReport"=dword:00000000
            //"IncludeKernelFaults"=dword:00000000
            //"IncludeMicrosoftApps"=dword:00000000
            //"IncludeWindowsApps"=dword:00000000
            //"ShowUI"=dword:00000000
            //SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\PCHealth\ErrorReporting", "AllOrNone", 1 );
            //SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\PCHealth\ErrorReporting", "DoReport", 0 );
            //SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\PCHealth\ErrorReporting", "IncludeKernelFaults", 0 );
            //SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\PCHealth\ErrorReporting", "IncludeMicrosoftApps", 0 );
            //SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\PCHealth\ErrorReporting", "IncludeWindowsApps", 0 );
            //SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\PCHealth\ErrorReporting", "ShowUI", 0 );

            //[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer]
            //"Max Cached Icons"="2048"
            SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", "Max Cached Icons", "2048" );

            //[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced]
            //"TaskbarSizeMove"=dword:00000000
            SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSizeMove", 0 );

            // [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AlwaysUnloadDll]
            // @="1"
            SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AlwaysUnloadDll", "", "1" );

            //[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\BitBucket]
            //"Percent"=dword:00000003
            //SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\BitBucket", "NukeOnDelete", 0 );
            //SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\BitBucket", "Percent", 3 );
            //SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\BitBucket", "UseGlobalSettings", 1 );

            //
            //
            SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\BrowseNewProcess", "BrowseNewProcess", "yes" );

            // BHO
            DeleteTree( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Browser Helper Objects" );

            // HIDE "Run in separate memory space" checkbox on Run dialog
            // [HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer]
            // "MemCheckBoxInRunDlg"=dword:00000000
            //SetValue( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "MemCheckBoxInRunDlg", 0 );

            //[-HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run]
            DeleteAllValues( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run" );

            //[-HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce]
            DeleteAllValues( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce" );
            DeleteAllValues( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnceEx" );

            //[-HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\RunServices]
            DeleteAllValues( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\RunServices" );

            //[-HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\RunServicesOnce]
            DeleteAllValues( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\RunServicesOnce" );

            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\SharedDLLs
            DeleteNonexistantPathValues( Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\SharedDLLs" );

        }

        /// <summary>
        /// 
        /// </summary>
        private static void Software_Policies() {

            Log.AppendHeader( logfile, "HKEY_LOCAL_MACHINE\\SOFTWARE\\POLICIES" );

            //[HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Reliability]
            //"ShutdownReasonOn"=dword:00000000
            //"ShutdownReasonUI"=dword:00000000
            CreateKey( Registry.LocalMachine, @"SOFTWARE\Policies\Microsoft\Windows NT\Reliability" );
            SetValue( Registry.LocalMachine, @"SOFTWARE\Policies\Microsoft\Windows NT\Reliability", "ShutdownReasonOn", 0 );
            SetValue( Registry.LocalMachine, @"SOFTWARE\Policies\Microsoft\Windows NT\Reliability", "ShutdownReasonUI", 0 );

        }

        #endregion

    }
}
