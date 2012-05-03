
using System;
using Microsoft.Win32;
using System.IO;


namespace BarfieldCleaner
{

            /// <summary>
            /// Registry Repair and Cleaning Class
            /// http://support.microsoft.com/kb/256986
            /// </summary>
            internal static partial class RegistryCleaner
            {


                        /// <summary>
                        /// 
                        /// </summary>
                        internal static void PruneCOMClasses()
                        {

                                    Log.AppendHeader( logfile, "Pruning COM Classes" );

                                    // Search COM Classes for invalid entries
                                    // http://msdn.microsoft.com/en-us/library/ms690440(VS.85).aspx
                                    Display.UpdateStatus( "Generating Class list..." );
                                    string[] classes = GetSubkeys( Registry.LocalMachine, @"Software\Classes" );


                                    // Return to caller if no classes are found
                                    // ( which would be really odd! )
                                    if ( classes != null && classes.Length != 0 )
                                    {

                                                foreach ( string className in classes )
                                                {

                                                            RegistryKey rkClass = null;
                                                            Display.UpdateStatus( "Class: " + className );

                                                            try
                                                            {

                                                                        // Try to open the CLSID subkey for this class
                                                                        // EXAMPLE: "HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AVIFile\CLSID"
                                                                        rkClass = Registry.LocalMachine.OpenSubKey( @"Software\Classes\" + className + @"\CLSID" );

                                                                        // Try to find the CLSID subkey for the class
                                                                        // EXAMPLE: "HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AVIFile\CLSID" -> @="{00022602-0000-0000-C000-000000000046}"
                                                                        FindCLSIDKey( rkClass, className );

                                                            }
                                                            catch ( Exception )
                                                            {

                                                                        // ********************************************************************************
                                                                        // SKIP KEYS THAT DON'T HAVE A "CLSID" SUBKEY
                                                                        // ********************************************************************************

                                                            }
                                                            finally
                                                            {

                                                                        if ( rkClass != null )
                                                                        {
                                                                                    rkClass.Close();
                                                                                    rkClass = null;
                                                                        }

                                                            }

                                                }

                                    }

                        }



                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="rkClass"></param>
                        /// <param name="className"></param>
                        private static void FindCLSIDKey( RegistryKey rkClass, string className )
                        {

                                    // Locals
                                    RegistryKey rkCLSID = null;
                                    object objCLSID = null;


                                    // Get the CLSID for this COM class
                                    objCLSID = rkClass.GetValue( String.Empty );
                                    if ( objCLSID != null )
                                    {

                                                try
                                                {

                                                            // HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID
                                                            // EXAMPLE: HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{00022602-0000-0000-C000-000000000046}
                                                            rkCLSID = Registry.LocalMachine.OpenSubKey( @"Software\Classes\CLSID\" + objCLSID.ToString() );

                                                            FindInprocHandler32( rkCLSID );

                                                }
                                                catch ( Exception )
                                                {

                                                            // CLSID can't be found so delete the class
                                                            // EXAMPLE: "HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AVIFile"
                                                            DeleteTree( Registry.LocalMachine, @"Software\Classes\" + className );

                                                }
                                                finally
                                                {

                                                            if ( rkCLSID != null )
                                                            {

                                                                        rkCLSID.Close();
                                                                        rkCLSID = null;

                                                            }

                                                            objCLSID = null;

                                                }

                                    }

                        }



                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="rkCLSID"></param>
                        private static void FindInprocHandler32( RegistryKey rkCLSID )
                        {
                                    // Locals
                                    RegistryKey rkHandlerServer = null;
                                    object objPath = null;


                                    try
                                    {

                                                // HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID
                                                // EXAMPLE: HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{00022602-0000-0000-C000-000000000046}\InprocHandler32
                                                rkHandlerServer = rkCLSID.OpenSubKey( "InprocHandler32" );

                                                objPath = rkHandlerServer.GetValue( String.Empty );
                                                if ( objPath != null )
                                                {

                                                            string path = objPath.ToString();

                                                            if ( !Path.IsPathRooted( path ) )
                                                            {

                                                                        string envPathVar = Environment.ExpandEnvironmentVariables( "%path%" );

                                                                        if ( !String.IsNullOrEmpty( envPathVar ) )
                                                                        {

                                                                                    string[] envPaths = envPathVar.Split( ';' );

                                                                                    if ( envPaths != null && envPaths.Length != 0 )
                                                                                    {
                                                                                                bool bFound = false;
                                                                                                string fqPath = null;

                                                                                                foreach ( string envPath in envPaths )
                                                                                                {

                                                                                                            fqPath = Path.Combine( envPath, path );

                                                                                                            if ( File.Exists( fqPath ) )
                                                                                                            {
                                                                                                                        bFound = true;
                                                                                                                        break;
                                                                                                            }

                                                                                                }

                                                                                                if ( !bFound )
                                                                                                {
                                                                                                            // Delete invalid FQ Path
                                                                                                            Log.AppendString( logfile, fqPath + " not found." + Environment.NewLine );
                                                                                                }
                                                                                                else
                                                                                                {

                                                                                                            Log.AppendString( logfile, "Found: " + fqPath + Environment.NewLine );

                                                                                                }

                                                                                    }

                                                                        }

                                                            }
                                                            else
                                                            {

                                                                        if ( !File.Exists( path ) )
                                                                        {

                                                                                    // Delete invalid FQ Path
                                                                                    Log.AppendString( logfile, path + " not found." + Environment.NewLine );

                                                                        }
                                                                        else
                                                                        {

                                                                                    Log.AppendString( logfile, "Found: " + path + Environment.NewLine );

                                                                        }

                                                            }
                                                }

                                    }
                                    catch ( Exception )
                                    {

                                    }
                                    finally
                                    {

                                                if ( rkHandlerServer != null )
                                                {

                                                            rkHandlerServer.Close();
                                                            rkHandlerServer = null;

                                                }

                                                objPath = null;

                                    }


                        }



            }


}
