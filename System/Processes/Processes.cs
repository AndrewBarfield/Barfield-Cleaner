
using System;
using System.Diagnostics;
using System.Threading;

namespace BarfieldCleaner {

    /// <summary>
    /// To free RAM and calm the system overall, this class iterates
    /// processes and closes all but those required by Windows
    /// </summary>
    internal static class Processes {

        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private const string logfile = "1 - Processes.log";


        /// <summary>
        /// 
        /// </summary>
        private static int processesKilledCount = 0;


        /// <summary>
        /// 
        /// </summary>
        private static long releasedMemory = 0;

        #endregion

        #region Internal Methods

        /// <summary>
        /// 'EntryPoint' for the class:
        /// Logs and closes processes
        /// </summary>
        internal static void Start() {
            Display.WriteTitle( "Release Resources" );
            CloseProcesses();

            Display.WriteSummary( processesKilledCount.ToString() +
                ( processesKilledCount > 1 ? " applications" : " application" ) +
                " closed (" + Utility.FormatBytes( releasedMemory ) + " released)" );
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Terminates all processes but those required to run Windows
        /// </summary>
        /// <param name="writeLog"></param>
        private static void CloseProcesses() {

            processesKilledCount = 0;
            releasedMemory = 0;

            Log.AppendHeader( logfile, "Closed Processes" );

            try {


                // Iterate all processes, closing extraneous ones
                foreach ( Process p in Process.GetProcesses() ) {

                    try {

                        // Get Process Name
                        if ( !String.IsNullOrEmpty( p.ProcessName ) ) {

                            string processName = p.ProcessName.ToLower();

                            Display.UpdateStatus( "Found '" + processName + "'" );

                            // Kill the process if its not in the list
                            if ( !Properties.Settings.Default.RequiredOSProcesses.Contains( processName ) ) {
                                releasedMemory += p.WorkingSet64;
                                p.Kill();
                                processesKilledCount++;
                                Log.AppendString( logfile, processName + Environment.NewLine );
                                Display.UpdateStatus( "Closed '" + processName + "'", ConsoleColor.Red );
                            }

                            processName = null;

                        }

                    }
                    catch ( Exception ) {

                    }
                    finally {

                        if ( p != null ) {

                            p.Close();
                            p.Dispose();

                        }

                    }

                }

            }
            catch ( Exception ex ) {
                Log.AppendException( logfile, ex );
            }
            finally {
            }

        }

        #endregion

    }
}
