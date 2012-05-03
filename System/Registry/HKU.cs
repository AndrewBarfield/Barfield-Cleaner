
using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;

namespace BarfieldCleaner {

    /// <summary>
    /// Registry Repair and Cleaning Class
    /// http://support.microsoft.com/kb/256986
    /// </summary>
    internal static partial class RegistryCleaner {

        /// <summary>
        /// Corrects erroneous HKEY_USERS settings
        /// </summary>
        private static void CleanUsers() {

            Log.AppendHeader( logfile, "HKEY_USERS" );

            // Get a list of all users
            string[] users = Registry.Users.GetSubKeyNames();

            // Iterate each user and apply settings
            foreach ( string currentUser in users ) {

                // --------------------------------------------------------------------------------
                // Color Schemes
                // Reference: http://technet.microsoft.com/en-us/library/cc978599.aspx
                // --------------------------------------------------------------------------------

                // [HKEY_CURRENT_USER\Control Panel\Current]
                // "Color Schemes"="Windows Standard"
                //SetValue( Registry.Users, currentUser + @"\Control Panel\Current", "Color Schemes", "Windows Standard" );



                // --------------------------------------------------------------------------------
                // Desktop
                // Reference: http://technet.microsoft.com/en-us/library/cc978603.aspx
                // --------------------------------------------------------------------------------
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "ActiveWndTrkTimeout", 0 );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "AutoEndTasks", "1" );                       //
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "CaretWidth", 1 );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "CoolSwitch", "1" );                         //
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "CoolSwitchColumns", "7" );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "CoolSwitchRows", "3" );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "CursorBlinkRate", "200" );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "DragFullWindows", "0" );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "DragHeight", "4" );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "DragWidth", "4" );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "FontSmoothing", "2" );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "FontSmoothingType", 2 );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "ForegroundFlashCount", 2 );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "ForegroundLockTimeout", 0x0012fa6c );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "GridGranularity", "0" );                    //
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "HungAppTimeout", "100" );                   //
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "LowPowerActive", "0" );                     //
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "LowPowerTimeOut", "0" );                    //
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "MenuShowDelay", "0" );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "NoAutoReturnToWelcome", "1" );              //
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "PaintDesktopVersion", 0 ); 
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "PowerOffActive", "0" );                     //
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "PowerOffTimeOut", "0" );                    //
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "ScreenSaveActive", "1" );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "ScreenSaverIsSecure", "0" );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "ScreenSaveTimeOut", "60" );
                DeleteValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "SCRNSAVE.EXE" );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "SmoothScroll", 0 );                         //
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "WaitToKillAppTimeout", "1000" );            //
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop", "WheelScrollLines", "2" );

                // --------------------------------------------------------------------------------
                // Window Metrics
                // Reference: http://technet.microsoft.com/en-us/library/cc978629.aspx
                // --------------------------------------------------------------------------------
                SetValue( Registry.Users, currentUser + @"\Control Panel\Desktop\WindowMetrics", "MinAnimate", "0" );

                // --------------------------------------------------------------------------------
                // Keyboard
                // Reference: http://technet.microsoft.com/en-us/library/cc978656.aspx
                // --------------------------------------------------------------------------------
                SetValue( Registry.Users, currentUser + @"\Control Panel\Keyboard", "InitialKeyboardIndicators", "2" );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Keyboard", "KeyboardDelay", "1" );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Keyboard", "KeyboardSpeed", "31" );

                // --------------------------------------------------------------------------------
                // Sound
                // Reference: 
                // --------------------------------------------------------------------------------
                SetValue( Registry.Users, currentUser + @"\Control Panel\Sound", "Beep", "yes" );
                SetValue( Registry.Users, currentUser + @"\Control Panel\Sound", "ExtendedSounds", "yes" );

                // --------------------------------------------------------------------------------
                // "Open With..." executable fix
                // --------------------------------------------------------------------------------
                DeleteTree( Registry.Users, currentUser + @"\Software\Classes\.exe" );
                DeleteTree( Registry.Users, currentUser + @"\Software\Classes\secfile" );

                // [HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer]
                // "EnableAutoTray"=dword:00000000
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer", "EnableAutoTray", 0 );

                // --------------------------------------------------------------------------------
                // Advanced
                // Reference: 
                // --------------------------------------------------------------------------------

                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "AlwaysShowMenus", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "AutoCheckSelect", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisablePreviewDesktop", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache", 1 );                      //
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DontPrettyPath", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Filter", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Hidden", 2 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideDrivesWithNoMedia", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideIcons", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "IconsOnly", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewAlphaSelect", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewShadow", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MapNetDrvBtn", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "NavPaneExpandToCurrentFolder", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "NavPaneShowAllFolders", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SeparateProcess", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ServerAdminUI", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCompColor", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowInfoTip", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowPreviewHandlers", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSuperHidden", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTypeOverlay", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_AdminToolsRoot", 2 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_JumpListItems", 10 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_MinMFU", 10 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_NotifyNewApps", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_PowerButtonAction", 2 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_SearchFiles", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_ShowControlPanel", 2 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_ShowHelp", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_ShowMyComputer", 2 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_ShowMyDocs", 2 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_ShowMyGames", 2 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_ShowMyMusic", 2 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_ShowMyPics", 2 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_ShowPrinters", 2 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_ShowRun", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_ShowSetProgramAccessAndDefaults", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "StartMenuAdminTools", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "StartMenuInit", 4 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SuperHidden", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSizeMove", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 0 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "WebView", 1 );



                // --------------------------------------------------------------------------------
                // BrowseNewProcess
                // Reference: 
                // --------------------------------------------------------------------------------
                //SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\BrowseNewProcess", "BrowseNewProcess", "Yes" );



                // --------------------------------------------------------------------------------
                // Desktop\CleanupWiz
                // Reference: 
                // --------------------------------------------------------------------------------
                //SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\Desktop\CleanupWiz", "NoRun", 1 );



                // --------------------------------------------------------------------------------
                // MenuOrder
                // Reference: 
                // --------------------------------------------------------------------------------
                DeleteTree( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\MenuOrder" );
                CreateKey( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\MenuOrder" );



                // --------------------------------------------------------------------------------
                // UserAssist
                // Reference: 
                // --------------------------------------------------------------------------------
                DeleteTree( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\UserAssist" );
                CreateKey( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\UserAssist" );
                CreateKey( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\UserAssist\Settings" );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\UserAssist\Settings", "NoLog", 1 );


                // --------------------------------------------------------------------------------
                // VisualEffects
                // Reference: 
                // --------------------------------------------------------------------------------

                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 3 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\AnimateMinMax", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\ComboBoxAnimation", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\ControlAnimations", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\CursorShadow", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\DragFullWindows", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\DropShadow", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\DWMAeroPeekEnabled", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\DWMEnabled", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\DWMSaveThumbnailEnabled", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\FontSmoothing", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\ListBoxSmoothScrolling", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\ListviewAlphaSelect", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\ListviewShadow", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\MenuAnimation", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\SelectionFade", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\TaskbarAnimations", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\ThumbnailsOrIcon", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\TooltipAnimation", "DefaultApplied", 1 );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects\TransparentGlass", "DefaultApplied", 1 );


                // [-HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies]
                // [HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies]
                DeleteTree( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Policies" );
                CreateKey( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Policies" );

                // [HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer]
                // @=""
                CreateKey( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer" );
                SetValue( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "", "" );

                // [HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System]
                CreateKey( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Policies\System" );



                // --------------------------------------------------------------------------------
                // Start-up
                // Reference:
                // --------------------------------------------------------------------------------
                DeleteAllValues( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\Run" );
                DeleteAllValues( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\RunOnce" );
                DeleteAllValues( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\RunOnceEx" );
                DeleteAllValues( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\RunServices" );
                DeleteAllValues( Registry.Users, currentUser + @"\Software\Microsoft\Windows\CurrentVersion\RunServicesOnce" );

            }

            Display.UpdateStatus( "Refreshing Windows Shell" );
            NativeMethods.BroadcastSettingsChange();
        }



    }
}
