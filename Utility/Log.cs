
using System;			// DateTime, Environment, Exception
using System.Diagnostics;	// StackTrace
using System.IO;		// Directory, File, Path



namespace BarfieldCleaner {

    /// <summary>
    /// Exposes methods for creating a log folder and writing to log files
    /// </summary>
    internal static class Log {

        #region Fields
        /// <summary>
        /// A horizontal rule made of 80 hyphens, used to 'underline' headers
        /// </summary>
        internal static readonly string HorizontalRule = Environment.NewLine + new String( '-', 80 ) + Environment.NewLine;

        /// <summary>
        /// Log folder name
        /// </summary>
        private static readonly string logFolderName = "BarfieldCleaner Logs";

        /// <summary>
        /// Log folder instance
        /// </summary>
        private static DirectoryInfo logFolderDirectoryInfo;
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the ECS log folder structure
        /// </summary>
        internal static void CreateFolder() {

            // Create a folder in the root of the system drive
            string logFolderFQPath = Path.Combine( SystemInformation.SystemDrive.RootPath, logFolderName );

            // Delete the folder if it already exists
            if ( Directory.Exists( logFolderFQPath ) ) {

                // Get a list of files to delete
                string[] logFolderFiles = null;
                try {

                    logFolderFiles = Directory.GetFiles( logFolderFQPath );

                }
                catch ( Exception ) {
                }

                // Attempt to delete all files in the log folder
                foreach ( string file in logFolderFiles ) {
                    try {
                        File.Delete( file );
                    }
                    catch ( Exception ) {
                    }
                }

            }

            // Create the Log folder
            try {
                logFolderDirectoryInfo = Directory.CreateDirectory( logFolderFQPath );
                logFolderDirectoryInfo.Attributes = FileAttributes.Directory | FileAttributes.NotContentIndexed | FileAttributes.System;
            }
            catch ( Exception ) {
            }


            // Notify the shell that the log folder has been created
            NativeMethods.BroadcastSettingsChange();

        }

        /// <summary>
        /// Adds error messages to specified log file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="ex"></param>
        internal static void AppendException(string filename, Exception ex) {
            // get call stack
            StackTrace stackTrace = new StackTrace();
            AppendString( filename, "Exception in " + stackTrace.GetFrame( 1 ).GetMethod().Name + "(): " + ex.GetType().Name + ": " + ex.Message + Environment.NewLine );

        }

        /// <summary>
        /// Adds header to specified log file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="header"></param>
        internal static void AppendHeader(string filename, string header) {
            AppendString( filename, Environment.NewLine + header + Environment.NewLine + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + Log.HorizontalRule );
        }

        /// <summary>
        /// Adds text to specified log file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="text"></param>
        internal static void AppendString(string filename, string text) {
            StreamWriter sw = null;

            string logFilePath = Path.Combine( logFolderDirectoryInfo.FullName, filename );

            try {

                if ( Directory.Exists( logFolderDirectoryInfo.FullName ) ) {

                    sw = File.AppendText( logFilePath );
                    sw.Write( text );
                    sw.Close();

                }

            }
            catch ( Exception ) {
            }

            sw = null;
        }

        #endregion

    }
}
