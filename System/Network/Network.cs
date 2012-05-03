
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Management;
using System.Threading;

namespace BarfieldCleaner
{
            internal static class Network
            {

                        private const string logfile = "5 - Networking.log";


                        /// <summary>
                        /// 
                        /// </summary>
                        internal static void Start()
                        {

                                    Display.WriteTitle( "Networking" );

                                    EnableAdapters();

                                    //AdapterConfiguration();

                                    ShowIPAddress();

                        }


                        /*
                        /// <summary>
                        /// 
                        /// </summary>
                        internal static void AdapterConfiguration()
                        {
                                    // http://msdn.microsoft.com/en-us/library/aa394217(v=VS.85).aspx

                        }
                        */


                        /// <summary>
                        /// 
                        /// </summary>
                        internal static void EnableAdapters()
                        {

                                    Log.AppendHeader( logfile, "Network Configuration" );

                                    ManagementObjectSearcher searcher = null;

                                    try
                                    {

                                                searcher = new ManagementObjectSearcher( "Select * from Win32_NetworkAdapter" );

                                                foreach ( ManagementObject mo in searcher.Get() )
                                                {

                                                            try
                                                            {

                                                                        // Works for Vista and later
                                                                        // http://msdn.microsoft.com/en-us/library/aa390385(v=VS.85).aspx

                                                                        UInt32 success = (UInt32)mo.InvokeMethod( "Enable", null );

                                                                        if ( success == 0 )
                                                                                    Display.UpdateStatus( mo["Caption"].ToString() + " enabled" );

                                                            }
                                                            catch ( Exception )
                                                            {

                                                            }
                                                            finally
                                                            {

                                                            }

                                                }

                                    }
                                    catch ( Exception )
                                    {

                                    }
                                    finally
                                    {

                                                // Cleanup
                                                if ( searcher != null )
                                                {

                                                            searcher.Dispose();
                                                            searcher = null;

                                                }

                                    }

                        }


                        internal static void ShowIPAddress()
                        {

                                    IPAddress[] ipAddrs = Dns.GetHostAddresses( Dns.GetHostName() );

                                    if ( ipAddrs != null && ipAddrs.Length != 0 )
                                    {

                                                if ( ipAddrs[0].Equals( IPAddress.Loopback ) || ipAddrs[0].Equals( IPAddress.None ) )
                                                {

                                                            Display.WriteSummary( "IP Address: Limited or no connection" );

                                                }
                                                else
                                                {

                                                            Display.WriteSummary( "IP Address: " + ipAddrs[0].ToString() );

                                                }

                                    }
                                    else
                                    {

                                                Display.WriteSummary( "IP Address: No IP Address found" );

                                    }

                        }

            }

}
