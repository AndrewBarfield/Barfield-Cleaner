
using System;
using Microsoft.Win32;
using System.IO;


namespace BarfieldCleaner {

    /// <summary>
    /// Registry Repair and Cleaning Class
    /// http://support.microsoft.com/kb/256986
    /// </summary>
    internal static partial class RegistryCleaner {

        private const string logfile = "3 - Registry.log";


        /// <summary>
        /// Count of Registry keys deleted
        /// </summary>
        private static int iDeletedKeysCount;

        /// <summary>
        /// Count of Registry values deleted
        /// </summary>
        private static int iDeletedValuesCount;

        /// <summary>
        /// Count of Registry values changed
        /// </summary>
        private static int iAlteredValuesCount;

        /// <summary>
        /// 
        /// </summary>
        internal static void Start() {

            Display.WriteTitle( "Registry" );

            iDeletedKeysCount = 0;
            iDeletedValuesCount = 0;
            iAlteredValuesCount = 0;

            CleanLocalMachine();
            CleanUsers();

            Display.WriteSummary( "Deleted items: " + ( iDeletedKeysCount + iDeletedValuesCount ).ToString() + ", Changed items: " + iAlteredValuesCount.ToString() );

        }

        /// <summary>
        /// Delete all values found in a Registry key.
        /// </summary>
        /// <param name="key">Registry Key Path</param>
        internal static void DeleteAllValues(RegistryKey root, string subKey) {

            RegistryKey rk = null;
            string[] valueNames = null;

            try {

                rk = root.OpenSubKey( subKey, true );
                if ( rk != null ) {

                    // Retain a list of value names for this key
                    valueNames = rk.GetValueNames();

                    // Return to caller if no values are found
                    if ( valueNames != null && valueNames.Length != 0 ) {

                        // Delete each value in this key
                        foreach ( string valueName in valueNames ) {

                            rk.DeleteValue( valueName );
                            iDeletedValuesCount++;

                            Log.AppendString( logfile, "Deleted value: " + rk.ToString() + "\\" + valueName + Environment.NewLine );
                            Display.UpdateStatus( "Deleted: " + root.ToString() + "\\...\\" + valueName, ConsoleColor.Red );

                        }

                    }

                }

            }
            catch ( Exception ex ) {

                // Record exceptions in the log file
                Log.AppendException( logfile, ex );

            }
            finally {

                // Finally, cleanup and prepare variables for garbage collection
                if ( rk != null ) {

                    rk.Close();
                    rk = null;

                }

                valueNames = null;

                subKey = null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="subKey"></param>
        internal static void DeleteNonexistantPathValues(RegistryKey root, string subKey) {

            RegistryKey rk = null;
            string[] valueNames = null;

            try {

                rk = root.OpenSubKey( subKey, true );
                if ( rk != null ) {

                    // Retain a list of value names for this key
                    valueNames = rk.GetValueNames();

                    // Return to caller if no values are found
                    if ( valueNames != null && valueNames.Length != 0 ) {

                        // Iterate each path (value) in this key
                        foreach ( string valueName in valueNames ) {

                            // Delete the path (value) if its invalid / not found
                            if ( !File.Exists( valueName ) ) {

                                rk.DeleteValue( valueName );
                                iDeletedValuesCount++;

                                Log.AppendString( logfile, "Deleted value: \"" + valueName + "\"" + Environment.NewLine );
                                Display.UpdateStatus( "Deleted: " + root.ToString() + "\\...\\" + valueName, ConsoleColor.Red );

                            }

                        }

                    }

                }

            }
            catch ( Exception ex ) {

                // Record exceptions in the log file
                Log.AppendException( logfile, ex );

            }
            finally {

                // Finally, cleanup and prepare variables for garbage collection
                if ( rk != null ) {

                    rk.Close();
                    rk = null;

                }

                valueNames = null;

                subKey = null;
            }

        }

        /// <summary>
        /// Deletes a Registry key tree
        /// </summary>
        /// <param name="root"></param>
        /// <param name="subKey"></param>
        internal static void DeleteTree(RegistryKey root, string subKey) {

            RegistryKey rk = null;
            string fqKeyPath = null;

            try {

                rk = root.OpenSubKey( subKey );
                if ( rk != null ) {

                    fqKeyPath = rk.ToString();
                    iDeletedKeysCount += ( rk.SubKeyCount + 1 );
                    iDeletedValuesCount += rk.ValueCount;

                    rk.Close();
                    rk = null;

                    root.DeleteSubKeyTree( subKey );

                    if ( fqKeyPath != null ) {
                        Log.AppendString( logfile, "Deleted tree: " + fqKeyPath + Environment.NewLine );
                        Display.UpdateStatus( "Deleted: " + root.ToString() + "\\..." + subKey.Substring( subKey.LastIndexOf( '\\' ) ) + "\\*", ConsoleColor.Red );
                    }

                }

            }
            catch ( Exception ex ) {

                // Record exceptions in the log file
                Log.AppendException( logfile, ex );

            }
            finally {

                // Finally, cleanup and prepare variables for garbage collection
                if ( rk != null ) {

                    rk.Close();
                    rk = null;

                }

                fqKeyPath = null;

                subKey = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="subKey"></param>
        internal static void CreateKey(RegistryKey root, string subKey) {

            RegistryKey rk = null;

            try {

                rk = root.CreateSubKey( subKey );
                if ( rk != null ) {

                    Log.AppendString( logfile, "Created key: " + rk.ToString() + Environment.NewLine );
                    Display.UpdateStatus( "Created: " + root.ToString() + "\\..." + subKey.Substring( subKey.LastIndexOf( '\\' ) ) + "\\*" );

                }

            }
            catch ( Exception ex ) {

                // Record exceptions in the log file
                Log.AppendException( logfile, ex );

            }
            finally {

                // Finally, cleanup and prepare variables for garbage collection
                if ( rk != null ) {

                    rk.Close();
                    rk = null;

                }

                subKey = null;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="subKey"></param>
        /// <returns></returns>
        internal static string[] GetSubkeys(RegistryKey root, string subKey) {

            RegistryKey rk = null;
            string[] tmp = null;

            try {

                rk = root.OpenSubKey( subKey );
                if ( rk != null ) {

                    tmp = rk.GetSubKeyNames();

                }

            }
            catch ( Exception ex ) {

                // Record exceptions in the log file
                Log.AppendException( logfile, ex );

            }
            finally {

                // Finally, cleanup and prepare variables for garbage collection
                if ( rk != null ) {

                    rk.Close();
                    rk = null;

                }

                subKey = null;

            }

            return tmp;

        }

        /// <summary>
        /// Gets an object value from the Registry
        /// </summary>
        /// <param name="root"></param>
        /// <param name="subKey"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static object GetValue(RegistryKey root, string subKey, string name) {

            RegistryKey rk = null;
            object obj = null;

            try {

                rk = root.OpenSubKey( subKey );
                if ( rk != null ) {
                    obj = rk.GetValue( name );
                }

            }
            catch ( Exception ex ) {

                Log.AppendException( logfile, ex );

            }
            finally {

                if ( rk != null ) {

                    rk.Close();
                    rk = null;

                }

                subKey = null;

                name = null;

            }

            return obj;

        }

        /// <summary>
        /// Writes a string value to key
        /// </summary>
        /// <param name="key">Registry Key Path</param>
        /// <param name="value">Any valid string</param>
        internal static void SetValue(RegistryKey root, string subKey, string name, string value) {

            RegistryKey rk = null;

            try {

                rk = root.OpenSubKey( subKey, true );
                if ( rk != null ) {
                    rk.SetValue( name, value, RegistryValueKind.String );
                    iAlteredValuesCount++;

                    Log.AppendString( logfile, "Set value: " + rk.ToString() + "\\" + name + " = \"" + value + "\"" + Environment.NewLine );
                    Display.UpdateStatus( "Changed: " + root.ToString() + "\\...\\" + name + " = \"" + value + "\"" );
                }

            }
            catch ( Exception ex ) {

                Log.AppendException( logfile, ex );

            }
            finally {

                if ( rk != null ) {

                    rk.Close();
                    rk = null;

                }

                name = null;

                value = null;
            }
        }



        /// <summary>
        /// Writes a string value to key
        /// </summary>
        /// <param name="key">Registry Key Path</param>
        /// <param name="value">Any valid string</param>
        internal static void SetExpandStringValue(RegistryKey root, string subKey, string name, string value) {

            RegistryKey rk = null;

            try {

                rk = root.OpenSubKey( subKey, true );
                if ( rk != null ) {
                    rk.SetValue( name, value, RegistryValueKind.ExpandString );
                    iAlteredValuesCount++;

                    Log.AppendString( logfile, "Set value: " + rk.ToString() + "\\" + name + " = \"" + value + "\"" + Environment.NewLine );
                    Display.UpdateStatus( "Changed: " + root.ToString() + "\\...\\" + name + " = \"" + value + "\"" );
                }

            }
            catch ( Exception ex ) {

                Log.AppendException( logfile, ex );

            }
            finally {

                if ( rk != null ) {

                    rk.Close();
                    rk = null;

                }

                name = null;

                value = null;
            }
        }


        /// <summary>
        /// Writes a int value to key
        /// </summary>
        /// <param name="key">Registry Key Path</param>
        /// <param name="value">32-bit binary value</param>
        internal static void SetValue(RegistryKey root, string subKey, string name, int value) {

            RegistryKey rk = null;

            try {

                rk = root.OpenSubKey( subKey, true );
                if ( rk != null ) {

                    rk.SetValue( name, value, RegistryValueKind.DWord );
                    iAlteredValuesCount++;

                    Log.AppendString( logfile, "Set value: " + rk.ToString() + "\\" + name + " = " + value + Environment.NewLine );
                    Display.UpdateStatus( "Changed: " + root.ToString() + "\\...\\" + name + " = " + value );

                }

            }
            catch ( Exception ex ) {

                Log.AppendException( logfile, ex );

            }
            finally {

                if ( rk != null ) {

                    rk.Close();
                    rk = null;

                }

                subKey = null;

                name = null;

            }
        }


        /// <summary>
        /// Writes a byte[] value to key
        /// </summary>
        /// <param name="key">Registry Key Path</param>
        /// <param name="value">32-bit binary value</param>
        internal static void SetValue(RegistryKey root, string subKey, string name, byte[] value) {

            RegistryKey rk = null;

            try {

                rk = root.OpenSubKey( subKey, true );
                if ( rk != null ) {

                    rk.SetValue( name, value, RegistryValueKind.Binary );
                    iAlteredValuesCount++;

                    //Log.AppendString( logfile, "Set value: " + rk.ToString() + "\\" + name + " = " + value + Environment.NewLine );
                    Display.UpdateStatus( "Changed: " + root.ToString() + "\\...\\" + name + " = [BINARY]" );

                }

            }
            catch ( Exception ex ) {

                Log.AppendException( logfile, ex );

            }
            finally {

                if ( rk != null ) {

                    rk.Close();
                    rk = null;

                }

                subKey = null;

                name = null;

            }
        }


        /// <summary>
        /// Delete a value
        /// </summary>
        /// <param name="key">Registry Key Path</param>
        internal static void DeleteValue(RegistryKey root, string subKey, string name) {

            RegistryKey rk = null;

            try {

                rk = root.OpenSubKey( subKey, true );
                if ( rk != null ) {

                    rk.DeleteValue( name );
                    iAlteredValuesCount++;

                    Log.AppendString( logfile, "Deleted value: " + rk.ToString() + "\\" + name + Environment.NewLine );
                    Display.UpdateStatus( "Deleted: " + root.ToString() + "\\...\\" + name, ConsoleColor.Red );

                }

            }
            catch ( Exception ex ) {

                Log.AppendException( logfile, ex );

            }
            finally {

                if ( rk != null ) {
                    rk.Close();
                    rk = null;
                }

                subKey = null;
                name = null;

            }
        }


    }
}
