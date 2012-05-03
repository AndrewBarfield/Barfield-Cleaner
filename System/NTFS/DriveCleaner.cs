
using System;
using System.IO;
using System.Diagnostics;
using System.Security;
using System.Management;
using System.Threading;

namespace BarfieldCleaner
{

            /// <summary>
            /// Corrects errors in the file system
            /// </summary>
            internal static class DriveCleaner
            {

                        #region Fields

                        /// <summary>
                        /// Amount of space freed by the Drive Cleaner in human readable format
                        /// </summary>
                        private static string freedspace = String.Empty;
                        

                        /// <summary>
                        /// This class log filename
                        /// </summary>
                        private const string logfile = "6 - File System.log";

                        #endregion
                        

                        #region Properties

                        /// <summary>
                        /// Gets the amount of space freed by the Drive Cleaner in human readable format
                        /// </summary>
                        internal static string FreedSpace
                        {
                                    get
                                    {

                                                return freedspace;

                                    }
                        }

                        #endregion


                        #region Internal Methods

                        /// <summary>
                        /// 
                        /// </summary>
                        internal static void Start()
                        {

                                    Log.AppendHeader( logfile, "Drive Cleaner" );
                                    long startFreeSpace = new DriveInfo( SystemInformation.SystemDrive.RootPath ).AvailableFreeSpace;


                                    // Delete files/folders related to software
                                    CleanApplications();


                                    // Delete files/folders inside system folders
                                    CleanKnownFolders();


                                    // 
                                    //ScanFileSystem();


                                    // Delete shortcuts with nonexistant targets
                                    PruneShortcuts();


                                    // Refresh Explorer
                                    NativeMethods.BroadcastSettingsChange();


                                    // Report free space gained
                                    freedspace = Utility.FormatBytes( new DriveInfo( SystemInformation.SystemDrive.RootPath ).AvailableFreeSpace - startFreeSpace );
                                    Log.AppendString( logfile, Environment.NewLine + "System HDD Cleanup: " + freedspace + Environment.NewLine );
                        }
                        
                        #endregion


                        #region Private Methods

                        /// <summary>
                        /// Deletes a shortcut if it's target does not exist
                        /// </summary>
                        private static void PruneShortcuts()
                        {

                                    // http://msdn.microsoft.com/en-us/library/aa394438(v=VS.85).aspx
                                    ManagementObjectSearcher searcher = new ManagementObjectSearcher( "SELECT * FROM Win32_ShortcutFile" );

                                    foreach ( ManagementObject mo in searcher.Get() )
                                    {

                                                object objTarget = null;
                                                string target = null;
                                                string displayPath = null;

                                                try
                                                {

                                                            objTarget = mo["Target"];
                                                            if ( objTarget != null )
                                                            {

                                                                        target = objTarget.ToString();
                                                                        if ( !String.IsNullOrEmpty( target ) )
                                                                        {
                                                                                    displayPath = NativeMethods.PathShortener( target, 61 );

                                                                                    Display.UpdateStatus( "Shortcut: " + displayPath );

                                                                                    if ( !File.Exists( target ) )
                                                                                    {

                                                                                                if ( ( (UInt32)mo.InvokeMethod( "Delete", null ) ) == 0 )
                                                                                                {

                                                                                                            Log.AppendString( logfile, "Deleted shortcut \"" + mo["FileName"].ToString() + "\"to: " + target + Environment.NewLine );
                                                                                                            Display.UpdateStatus( "Deleted: " + displayPath, ConsoleColor.Red );

                                                                                                }

                                                                                    }

                                                                        }

                                                            }

                                                }
                                                catch ( Exception ex )
                                                {

                                                            Log.AppendException( logfile, ex );

                                                }
                                                finally
                                                {

                                                            objTarget = null;
                                                            target = null;
                                                            displayPath = null;

                                                }

                                    }

                        }
                        
                        /*
                        /// <summary>
                        /// Search the entire file system and correct errors
                        /// </summary>
                        private static void ScanFileSystem()
                        {

                                    // Event Handlers
                                    Search.FolderFoundEvent += new Search.FoundFolderEventHandler( OnFoundFolder );
                                    Search.FileFoundEvent += new Search.FoundFileEventHandler( OnFoundFile );

                                    // Start Search
                                    Search.Query( new DirectoryInfo( SystemInformation.SystemDrive.RootPath ) );

                        }
                        */
                        #endregion


                        #region Application Cleanup

                        /// <summary>
                        /// Delete files/folders related to software
                        /// </summary>
                        private static void CleanApplications()
                        {

                                    CleanInternetExplorer();

                                    //CleanGoogleChrome();

                                    //CleanMozillaFirefox();
                        }



                        /// <summary>
                        /// Cleanup IE
                        /// </summary>
                        private static void CleanInternetExplorer()
                        {

                                    // Empty "Cookies"
                                    DirectoryInfo di = new DirectoryInfo( Environment.GetFolderPath( Environment.SpecialFolder.Cookies ) );
                                    EmptyFolder( di );
                                    di = null;

                                    // Empty "History"
                                    di = new DirectoryInfo( Environment.GetFolderPath( Environment.SpecialFolder.History ) );
                                    EmptyFolder( di );
                                    di = null;

                                    // Empty "InternetCache"
                                    di = new DirectoryInfo( Environment.GetFolderPath( Environment.SpecialFolder.InternetCache ) );
                                    EmptyFolder( di );
                                    di = null;


                                    // Make IE Clean Itself (I know the previous 3 sections make this redundant... just humor me...)
                                    // rundll32.exe InetCpl.cpl,ClearMyTracksByProcess 4351
                                    Display.UpdateStatus( "Delete all Internet Explorer data" );
                                    Program.SetNormalPriority();
                                    Process p = new Process();
                                    p.StartInfo.Arguments = "InetCpl.cpl,ClearMyTracksByProcess 4351";
                                    p.StartInfo.CreateNoWindow = false;
                                    p.StartInfo.FileName = "rundll32.exe";
                                    p.StartInfo.ErrorDialog = false;
                                    p.StartInfo.UseShellExecute = true;
                                    p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                                    p.Start();
                                    p.PriorityClass = ProcessPriorityClass.AboveNormal;
                                    p.WaitForExit();
                                    Program.SetRealTimePriority();


                        }


                        /*
                        /// <summary>
                        /// Cleanup Chrome
                        /// </summary>
                        private static void CleanGoogleChrome()
                        {

                                    // C:\Documents and Settings\Mickey Barfield.MICKEYBARFIELD\Local Settings\Application Data\Google\Chrome\User Data\Default\XXXXXXX

                                    string chromeDefault = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Google\\Chrome\\User Data\\Default" );
                                    if ( Directory.Exists( chromeDefault ) )
                                    {

                                                FileInfo fi = null;

                                                string[] files = Directory.GetFiles( chromeDefault, "*history*", SearchOption.TopDirectoryOnly );
                                                foreach ( string file in files )
                                                {
                                                            fi = new FileInfo( file );
                                                            DeleteFile( fi );
                                                            fi = null;
                                                }


                                                fi = new FileInfo( Path.Combine( chromeDefault, "Current Tabs" ) );
                                                DeleteFile( fi );
                                                fi = null;

                                                fi = new FileInfo( Path.Combine( chromeDefault, "Last Tabs" ) );
                                                DeleteFile( fi );
                                                fi = null;

                                                fi = new FileInfo( Path.Combine( chromeDefault, "Visited Links" ) );
                                                DeleteFile( fi );
                                                fi = null;


                                                string chromeCache = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Google\\Chrome\\User Data\\Default\\Cache" );
                                                DirectoryInfo di = new DirectoryInfo( chromeCache );
                                                EmptyFolder( di );
                                                di = null;




                                                di = new DirectoryInfo( Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Google\\Chrome\\User Data\\Default\\Local Storage" ) );
                                                EmptyFolder( di );
                                                di = null;

                                                di = new DirectoryInfo( Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Google\\Chrome\\User Data\\Default\\Media Cache" ) );
                                                EmptyFolder( di );
                                                di = null;

                                    }

                        }
                        */


                        /*

                        /// <summary>
                        /// Cleanup Firefox
                        /// </summary>
                        private static void CleanMozillaFirefox()
                        {
                        }
                        
                        */

                        #endregion
                        

                        #region Known Folder Cleanup

                        /// <summary>
                        /// 
                        /// </summary>
                        private static void CleanKnownFolders()
                        {

                                    // Empty Recycle Bin
                                    Display.UpdateStatus( "Empty Recycle Bin" );
                                    NativeMethods.EmptyRecycleBin( String.Empty );


                                    // Empty "Recent Documents"
                                    DirectoryInfo di = new DirectoryInfo( Environment.GetFolderPath( Environment.SpecialFolder.Recent ) );
                                    EmptyFolder( di );
                                    di = null;


                                    // Empty "Startup"
                                    di = new DirectoryInfo( Environment.GetFolderPath( Environment.SpecialFolder.Startup ) );
                                    EmptyFolder( di );
                                    di = null;


                                    //
                                    CleanAppDataFolder();


                                    //
                                    //CleanProgramFiles();


                                    //
                                    CleanWindowsFolder();


                                    //
                                    //CleanSystemFolder();

                        }
                        

                        /// <summary>
                        /// 
                        /// </summary>
                        private static void CleanAppDataFolder()
                        {

                                    DirectoryInfo di = null;

                                    // Flash Player
                                    di = new DirectoryInfo( Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ), "Macromedia\\Flash Player\\macromedia.com\\support\\flashplayer\\sys" ) );
                                    EmptyFolder( di );
                                    di = null;

                        }
                        
                        /*

                        /// <summary>
                        /// 
                        /// </summary>
                        private static void CleanProgramFiles()
                        {

                                    // Delete any files in this folder
                                    DeleteFilesOnly( new DirectoryInfo( Environment.GetFolderPath( Environment.SpecialFolder.ProgramFiles ) ) );

                                    DirectoryInfo di = null;

                                    di = new DirectoryInfo( Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ProgramFiles ), "Uninstall Information" ) );
                                    EmptyFolder( di );
                                    di = null;

                                    // Folder is no longer used
                                    di = new DirectoryInfo( Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ProgramFiles ), "WindowsUpdate" ) );
                                    DeleteFolder( di );
                                    di = null;

                                    // Fake AV
                                    di = new DirectoryInfo( Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ProgramFiles ), "Antivirus" ) );
                                    DeleteFolder( di );
                                    di = null;

                        }
                        
                        */

                        /// <summary>
                        /// 
                        /// </summary>
                        private static void CleanWindowsFolder()
                        {

                                    DirectoryInfo di = null;

                                    di = new DirectoryInfo( Path.Combine( SystemInformation.SpecialFolders.Windows, "$hf_mig$" ) );
                                    DeleteFolder( di );
                                    di = null;


                                    // Empty 'Service Pack' and 'Uninstall' folders in Windows folder
                                    string[] dirs = Directory.GetDirectories( SystemInformation.SpecialFolders.Windows, "$nt*$", SearchOption.TopDirectoryOnly );
                                    foreach ( string dir in dirs )
                                    {
                                                di = new DirectoryInfo( dir );
                                                DeleteFolder( di );
                                                di = null;
                                    }


                                    //di = new DirectoryInfo( Path.Combine( SystemInformation.SpecialFolders.Windows, "Downloaded Installations" ) );
                                    //EmptyFolder( di );
                                    //di = null;

                                    //di = new DirectoryInfo( Path.Combine( SystemInformation.SpecialFolders.Windows, "help" ) );
                                    //EmptyFolder( di );
                                    //di = null;

                                    di = new DirectoryInfo( Path.Combine( SystemInformation.SpecialFolders.Windows, "ie6" ) );
                                    EmptyFolder( di );
                                    di = null;

                                    di = new DirectoryInfo( Path.Combine( SystemInformation.SpecialFolders.Windows, "ie6updates" ) );
                                    EmptyFolder( di );
                                    di = null;

                                    di = new DirectoryInfo( Path.Combine( SystemInformation.SpecialFolders.Windows, "ie7" ) );
                                    EmptyFolder( di );
                                    di = null;

                                    di = new DirectoryInfo( Path.Combine( SystemInformation.SpecialFolders.Windows, "ie7updates" ) );
                                    EmptyFolder( di );
                                    di = null;

                                    di = new DirectoryInfo( Path.Combine( SystemInformation.SpecialFolders.Windows, "Prefetch" ) );
                                    EmptyFolder( di );
                                    di = null;

                                    //di = new DirectoryInfo( Path.Combine( SystemInformation.SpecialFolders.Windows, "RegisteredPackages" ) );
                                    //DeleteFolder( di );
                                    //di = null;

                                    //di = new DirectoryInfo( Path.Combine( SystemInformation.SpecialFolders.Windows, "servicepackfiles\\i386" ) );
                                    //EmptyFolder( di );
                                    //di = null;

                                    //di = new DirectoryInfo( Path.Combine( SystemInformation.SpecialFolders.Windows, "SoftwareDistribution" ) );
                                    //EmptyFolder( di );
                                    //di = null;

                        }
                        
                        /*

                        /// <summary>
                        /// 
                        /// </summary>
                        private static void CleanSystemFolder()
                        {

                                    DirectoryInfo di = null;

                                    di = new DirectoryInfo( Path.Combine( SystemInformation.SpecialFolders.System, "CatRoot" ) );
                                    EmptyFolder( di );
                                    di = null;

                                    di = new DirectoryInfo( Path.Combine( SystemInformation.SpecialFolders.System, "CatRoot2" ) );
                                    EmptyFolder( di );
                                    di = null;

                                    di = new DirectoryInfo( Path.Combine( SystemInformation.SpecialFolders.System, "LogFiles" ) );
                                    DeleteFolder( di );
                                    di = null;

                        }
                        
                        */
                        #endregion
                        

                        #region File System Scan

                        /*
                        
                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="directory">The discovered folder</param>
                        private static void OnFoundFolder( DirectoryInfo directory )
                        {

                                    switch ( directory.Name.ToLower() )
                                    {
                                                case "BarfieldCleaner":
                                                            return;

                                                case "application data":
                                                            DeleteFilesOnly( directory );
                                                            return;

                                                case "av7":
                                                case "combofix":
                                                case "fast browser search":
                                                case "freeze.com":
                                                case "mywebsearch":
                                                case "msocache":
                                                case "sand-box":
                                                case "sdfix":
                                                case "spybot - search & destroy":
                                                case "vundofix backups":
                                                case "wildtangent":
                                                            DeleteFolder( directory );
                                                            return;

                                                case "cache":
                                                case "installtemp":
                                                case "temp":
                                                case "startup":
                                                case "#sharedobjects":
                                                            EmptyFolder( directory );
                                                            return;

                                    }
                        }
                        

                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="path"></param>
                        private static void OnFoundFile( FileInfo file )
                        {

                                    // Complete filename
                                    switch ( file.Name.ToLower() )
                                    {
                                                case "install.log":
                                                case "desktop.ini":
                                                            return;

                                                case "iconcache.db":
                                                case "thumbs.db":
                                                case "history.xml":
                                                            DeleteFile( file );
                                                            return;

                                    }


                                    // If the file's parent directory is named 'BarfieldCleaner' do nothing
                                    switch ( file.Directory.Name.ToLower() )
                                    {
                                                case "BarfieldCleaner":
                                                            return;
                                    }


                                    // Extension only
                                    switch ( file.Extension.ToLower() )
                                    {

                                                case ".001":
                                                case ".002":
                                                case ".avm":
                                                case ".bac":
                                                case ".bck":
                                                case ".bk!":
                                                case ".bk$":
                                                case ".b~k":
                                                case ".chk":
                                                case ".gid":
                                                case ".hlp":
                                                case ".log":
                                                case ".old":
                                                case ".tmp":
                                                            DeleteFile( file );
                                                            return;

                                    }


                                    // Filename without extension
                                    switch ( Path.GetFileNameWithoutExtension( file.Name ).ToLower() )
                                    {
                                                case "readme":
                                                            DeleteFile( file );
                                                            return;
                                    }

                        }
                        
                        */

                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="file"></param>
                        private static void DeleteFile( FileInfo file )
                        {

                                    // Additional fail-safe to protect certain files
                                    switch ( file.Name.ToLower() )
                                    {
                                                case "install.log":
                                                case "desktop.ini":
                                                            return;

                                    }

                                    // Strip 'read-only' attribute and delete the file
                                    try
                                    {

                                                file.Attributes = FileAttributes.Normal;
                                                file.Delete();

                                                Log.AppendString( logfile, "Deleted file: " + file.FullName + Environment.NewLine );
                                                Display.UpdateStatus( "Deleted: " + NativeMethods.PathShortener( file.FullName, 61 ), ConsoleColor.Red );

                                    }
                                    catch ( Exception ex )
                                    {

                                                //
                                                Log.AppendException( logfile, ex );

                                    }
                                    finally
                                    {

                                                // Clean up
                                                file = null;

                                    }

                        }
                        

                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="directory"></param>
                        private static void DeleteFolder( DirectoryInfo directory )
                        {

                                    try
                                    {

                                                directory.Attributes = FileAttributes.Normal;
                                                directory.Delete( true );

                                                Log.AppendString( logfile, "Deleted folder: " + directory.FullName + Environment.NewLine );
                                                Display.UpdateStatus( "Deleted: " + NativeMethods.PathShortener( directory.FullName, 61 ), ConsoleColor.Red );

                                    }
                                    catch ( Exception ex )
                                    {

                                                //
                                                Log.AppendException( logfile, ex );

                                    }
                                    finally
                                    {

                                                //
                                                directory = null;

                                    }

                        }
                        

                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="path"></param>
                        private static void EmptyFolder( DirectoryInfo di )
                        {

                                    // Delete all files
                                    try
                                    {

                                                foreach ( FileInfo fi in di.GetFiles() )
                                                {
                                                            DeleteFile( fi );
                                                }


                                                foreach ( DirectoryInfo subdi in di.GetDirectories() )
                                                {
                                                            DeleteFolder( subdi );
                                                }

                                    }
                                    catch ( Exception ex )
                                    {

                                                //
                                                Log.AppendException( logfile, ex );

                                    }
                                    finally
                                    {

                                                //
                                                di = null;

                                    }

                        }
                        

                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="path"></param>
                        private static void DeleteFilesOnly( DirectoryInfo di )
                        {

                                    //
                                    try
                                    {

                                                foreach ( FileInfo fi in di.GetFiles() )
                                                            DeleteFile( fi );

                                    }
                                    catch ( Exception ex )
                                    {

                                                //
                                                Log.AppendException( logfile, ex );

                                    }
                                    finally
                                    {

                                                di = null;

                                    }


                        }

                        #endregion

            }
}
