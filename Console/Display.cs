
using System;		// Console, ConsoleColor, Environment
using System.Diagnostics;	// Process
using System.IO;		// DriveInfo
using System.Threading;		// Thread.Sleep(n)



namespace BarfieldCleaner
{

            /// <summary>
            /// Exposes methods writing to the Console
            /// </summary>
            internal static class Display
            {

                        #region Fields

                        private const int preferredWindowHeight = 35;
                        private const int preferredWindowWidth = 75;
                        private const int StatusLeftMargin = 3;
                        private const int StatusRightMargin = 1;
                        private static int UsableRowWidth;
                        private static int MaxStatusLength;

                        #endregion

                        #region Methods

                        /// <summary>
                        /// 
                        /// </summary>
                        internal static void Format()
                        {

                                    // Set the height - noting that the Console can't be taller than LargestWindowHeight
                                    if ( preferredWindowHeight <= Console.LargestWindowHeight )
                                    {

                                                Console.WindowHeight = preferredWindowHeight;
                                                Console.BufferHeight = preferredWindowHeight;

                                    }
                                    else
                                    {

                                                Console.WindowHeight = Console.LargestWindowHeight;
                                                Console.BufferHeight = Console.LargestWindowHeight;

                                    }

                                    // Set the width
                                    if ( preferredWindowWidth <= Console.LargestWindowWidth )
                                    {

                                                Console.WindowWidth = preferredWindowWidth;
                                                Console.BufferWidth = preferredWindowWidth;

                                    }
                                    else
                                    {

                                                Console.WindowWidth = Console.LargestWindowWidth;
                                                Console.BufferWidth = Console.LargestWindowWidth;

                                    }



                                    UsableRowWidth = Console.WindowWidth - StatusRightMargin - StatusLeftMargin;
                                    MaxStatusLength = Console.WindowWidth - StatusRightMargin - StatusLeftMargin - 3;


                                    try
                                    {

                                                Console.Title = Process.GetCurrentProcess().MainModule.FileVersionInfo.ProductName;
                                                Console.BackgroundColor = ConsoleColor.Black;
                                                Console.CursorVisible = false;

                                    }
                                    catch ( Exception )
                                    {
                                    }

                        }


                        /// <summary>
                        /// 
                        /// </summary>
                        internal static void PrintRepairStartBanner()
                        {

                                    // Display OS version
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write( Console.Out.NewLine + " " + Environment.OSVersion.VersionString + Environment.NewLine + " CPU: " );
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.WriteLine( SystemInformation.Hardware.Processor.Name );

                                    // 
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write( " HDD: " );
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.Write( SystemInformation.SystemDrive.CapacityHumanReadable );

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write( "  RAM: " );
                                    Console.ForegroundColor = ConsoleColor.Gray;


                                    // Get and display total RAM
                                    if ( SystemInformation.Hardware.RAM.Total < 1073741824 )
                                    {

                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine( Utility.FormatBytes( SystemInformation.Hardware.RAM.Total ) );

                                    }
                                    else
                                    {

                                                Console.ForegroundColor = ConsoleColor.Gray;
                                                Console.WriteLine( Utility.FormatBytes( SystemInformation.Hardware.RAM.Total ) );

                                    }



                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine( " " + new String( '-', Console.BufferWidth - 2 ) + Console.Out.NewLine );

                        }


                        /// <summary>
                        /// 
                        /// </summary>
                        internal static void PrintRepairEndBanner()
                        {

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine( " " + new String( '-', Console.BufferWidth - 2 ) );

                        }


                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="title"></param>
                        internal static void WriteTitle( string title )
                        {

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.CursorLeft = 1;
                                    Console.WriteLine( "* " + title );

                                    // Immediately following Titles are status updates
                                    Console.ForegroundColor = ConsoleColor.Green;

                        }


                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="status"></param>
                        internal static void UpdateStatus( string status )
                        {

                                    // Indent
                                    Console.CursorLeft = StatusLeftMargin;

                                    // Truncate status message if it's longer than the display area
                                    if ( status.Length > UsableRowWidth )
                                                status = status.Substring( 0, MaxStatusLength ) + "...";

                                    // Write out the status message with spaces at the end to cover previous messages
                                    Console.Write( status + new String( ' ', UsableRowWidth - status.Length ) );

                                    // Pause to paint status message on console
                                    Thread.Sleep( 1 );

                        }


                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="status"></param>
                        internal static void UpdateStatus( string status, ConsoleColor color )
                        {

                                    // Save the current foreground color
                                    ConsoleColor tmpColor = Console.ForegroundColor;

                                    // Change the foreground color
                                    Console.ForegroundColor = color;

                                    // Display the status message
                                    UpdateStatus( status );

                                    // Reinstate the previous foreground color
                                    Console.ForegroundColor = tmpColor;

                                    // Pause to paint status message on console
                                    Thread.Sleep( 10 );
                        }


                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="summary"></param>
                        internal static void WriteSummary( string summary )
                        {

                                    Console.CursorLeft = StatusLeftMargin;
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.WriteLine( summary + new String( ' ', UsableRowWidth - summary.Length ) + Environment.NewLine );

                        }


                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="summary"></param>
                        internal static void WriteSummary( string summary, ConsoleColor color )
                        {

                                    Console.CursorLeft = StatusLeftMargin;
                                    Console.ForegroundColor = color;
                                    Console.WriteLine( summary + new String( ' ', UsableRowWidth - summary.Length ) + Environment.NewLine );

                        }

                        #endregion

            }
}
