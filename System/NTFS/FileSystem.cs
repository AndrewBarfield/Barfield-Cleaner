using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace BarfieldCleaner
{
            internal static class FileSystem
            {

                        private const string logfile = "6 - File System.log";

                        /// <summary>
                        /// 
                        /// </summary>
                        internal static void Start()
                        {

                                    Display.WriteTitle( "File System" );
                                    DriveCleaner.Start();
                                    SetCheckDiskAtBoot();
                                    //Defragment();

                                    Display.WriteSummary( "Cleanup: " + DriveCleaner.FreedSpace + ", Boot-time CHKDSK Set" );


                        }



                        /// <summary>
                        /// 
                        /// </summary>
                        private static void SetCheckDiskAtBoot()
                        {

                                    Display.UpdateStatus( "Scheduling boot-time Check Disk" );
                                    Log.AppendHeader( logfile, "Check NTFS at Boot" );
                                    Process p = new Process();
                                    p.StartInfo.Arguments = Environment.GetEnvironmentVariable( "SYSTEMDRIVE" ) + " /C";
                                    p.StartInfo.CreateNoWindow = false;
                                    p.StartInfo.FileName = "chkntfs.exe";
                                    p.StartInfo.UseShellExecute = false;
                                    p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                                    p.Start();
                                    p.WaitForExit();

                        }


                        /*
                        private static void Defragment()
                        {

                                    Display.UpdateStatus( "Defragmenting..." );
                                    Log.AppendHeader( logfile, "Defragment" );
                                    Process processDefragmenter = new Process();
                                    processDefragmenter.EnableRaisingEvents = true;
                                    processDefragmenter.StartInfo.Arguments = Environment.GetEnvironmentVariable( "SYSTEMDRIVE" ) + " -f -v";
                                    processDefragmenter.StartInfo.CreateNoWindow = true;
                                    processDefragmenter.StartInfo.FileName = "defrag.exe";
                                    processDefragmenter.StartInfo.UseShellExecute = false;
                                    processDefragmenter.StartInfo.RedirectStandardOutput = true;
                                    processDefragmenter.OutputDataReceived += new DataReceivedEventHandler( processDefragmenter_OutputDataReceived );
                                    processDefragmenter.Start();
                                    processDefragmenter.BeginOutputReadLine();
                                    processDefragmenter.WaitForExit();
                                    processDefragmenter.Close();
                                    processDefragmenter.Dispose();

                        }


                        private static void processDefragmenter_OutputDataReceived( object sender, DataReceivedEventArgs e )
                        {
                                    Log.AppendString( logfile, e.Data + Console.Out.NewLine );
                        }
                        */
            }
}
