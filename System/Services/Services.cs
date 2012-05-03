
using System;
using System.Text;
using System.ServiceProcess;
using System.Reflection;
using Microsoft.Win32;

namespace BarfieldCleaner {
    internal static class Services {

        #region Fields

        /// <summary>
        /// Class log filename
        /// </summary>
        private const string logfile = "4 - Services.log";


        /// <summary>
        /// Number of seconds to wait for a service to Start, Stop or Pause
        /// </summary>
        private static int iServiceChangeTimeout = 5;

        /// <summary>
        /// 
        /// </summary>
        private static int iServiceChangedCount;


        /// <summary>
        /// 
        /// </summary>
        private enum StartType {
            Automatic = 2,
            Manual = 3,
            Disabled = 4
        }


        /// <summary>
        /// 
        /// </summary>
        private enum StatusType {
            Start = 1,
            Stop = 2,
            Pause = 3
        }

        #endregion


        #region Internal Methods

        /// <summary>
        /// 
        /// </summary>
        internal static void Start() {

            Display.WriteTitle( "Windows Services" );

            ConfigureServices();

            if ( iServiceChangedCount < 2 ) {

                Display.WriteSummary( iServiceChangedCount.ToString() + " Service property changed" );

            }
            else {

                Display.WriteSummary( iServiceChangedCount.ToString() + " Service properties changed" );

            }

        }

        #endregion


        /// <summary>
        /// Configures the current and start-up status of a service based on service name
        /// </summary>
        private static void ConfigureServices() {

            //
            Log.AppendHeader( logfile, "Windows Services" );

            //
            ServiceController[] services = ServiceController.GetServices();
            foreach ( ServiceController sc in services ) {

                try {

                    Display.UpdateStatus( "Configuring " + sc.DisplayName );

                    switch ( sc.ServiceName ) {

                        case "Alerter":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "ALG":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "AppMgmt":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "aspnet_state":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "AudioSrv":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "BITS":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "Browser":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "cisvc":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "ClipSrv":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "clr_optimization_v2.0.50727_32":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "COMSysApp":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "Creative Service for CDROM Access":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "CryptSvc":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "DcomLaunch":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "Dhcp":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "dmadmin":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "dmserver":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "Dnscache":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "Dot3svc":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "EapHost":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "ERSvc":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "Eventlog":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "EventSystem":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "FastUserSwitchingCompatibility":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "FLEXnet Licensing Service":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "FontCache3.0.0.0":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "gupdate1c9867a2308c840":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "helpsvc":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "HidServ":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "hkmsvc":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "HTTPFilter":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "IDriverT":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "idsvc":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "ImapiService":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "JavaQuickStarterService":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "lanmanserver":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "lanmanworkstation":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "LmHosts":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "Messenger":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "mnmsrvc":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "MSDTC":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "MSIServer":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "napagent":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "NetDDE":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "NetDDEdsdm":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "Netlogon":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "Netman":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "NetTcpPortSharing":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "Nla":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "NtLmSsp":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "NtmsSvc":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "NVSvc":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "odserv":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "ose":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "PlugPlay":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "PolicyAgent":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "ProtectedStorage":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "QTTIUNCL":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "RasAuto":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "RasMan":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "RDSessMgr":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "RemoteAccess":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "RpcLocator":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "RpcSs":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "RSVP":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "SamSs":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "SCardSvr":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "Schedule":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "seclogon":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "SENS":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "SharedAccess":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "ShellHWDetection":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "Spooler":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "srservice":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "SSDPSRV":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "stisvc":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "SwPrv":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "SysmonLog":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "TapiSrv":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "TermService":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "Themes":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "TrkWks":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "upnphost":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "UPS":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "VSS":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "W32Time":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "WebClient":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "winmgmt":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "WMDM PMSP Service":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "WmdmPmSN":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "WmiApSrv":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "WMPNetworkSvc":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Disabled );
                            break;

                        case "wscsvc":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "wuauserv":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "WudfSvc":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        case "WZCSVC":
                            ChangeServiceProperties( sc, StatusType.Start, StartType.Automatic );
                            break;

                        case "xmlprov":
                            ChangeServiceProperties( sc, StatusType.Stop, StartType.Manual );
                            break;

                        default:
                            Log.AppendString( logfile, "UNKNOWN SERVICE: " + sc.ServiceName + ", \"" + sc.DisplayName + "\"" + Environment.NewLine );
                            break;
                    }

                }
                catch ( Exception ) {

                }
                finally {

                    // Cleanup
                    if ( sc != null ) {
                        sc.Close();
                        sc.Dispose();
                    }

                }

            }

        }



        /// <summary>
        /// Changes the status and start-up properties for a service
        /// </summary>
        /// <param name="sc">ServiceController instance</param>
        /// <param name="newStatus"></param>
        /// <param name="newStart"></param>
        private static void ChangeServiceProperties(ServiceController sc, StatusType newStatus, StartType newStart) {
            
            try {

                //
                Log.AppendString( logfile, sc.DisplayName + Environment.NewLine );


                // Save the old running status for logging
                string oldStatus = sc.Status.ToString();


                // Change Service running status to 'newStatus'
                switch ( newStatus ) {

                    case StatusType.Start: {

                            if ( sc.Status != ServiceControllerStatus.Running && sc.Status != ServiceControllerStatus.StartPending ) {
                                sc.Start();
                                sc.WaitForStatus( ServiceControllerStatus.Running, new TimeSpan( 0, 0, iServiceChangeTimeout ) );
                                iServiceChangedCount++;
                            }

                        }
                        break;



                    case StatusType.Stop: {

                            if ( sc.Status != ServiceControllerStatus.Stopped && sc.Status != ServiceControllerStatus.StopPending ) {
                                sc.Stop();
                                sc.WaitForStatus( ServiceControllerStatus.Stopped, new TimeSpan( 0, 0, iServiceChangeTimeout ) );
                                iServiceChangedCount++;
                            }

                        }
                        break;

                }

                sc.Refresh();

                // Log status change
                if ( oldStatus.Equals( sc.Status.ToString(), StringComparison.CurrentCultureIgnoreCase ) ) {

                    Log.AppendString( logfile, "Status: '" + sc.Status.ToString() + "' (not changed)" + Environment.NewLine );

                }
                else {

                    Log.AppendString( logfile, "Status changed from '" + oldStatus + "' to '" + sc.Status.ToString() + "'" + Environment.NewLine );

                }

                // Get old start-up type
                object oldStart = RegistryCleaner.GetValue( Registry.LocalMachine, @"SYSTEM\CurrentControlSet\Services\" + sc.ServiceName, "Start" );
                if ( oldStart != null ) {

                    // Set start-up type
                    if ( (int)newStart != (int)oldStart ) {

                        RegistryCleaner.SetValue( Registry.LocalMachine, @"SYSTEM\CurrentControlSet\Services\" + sc.ServiceName, "Start", (int)newStart );
                        Log.AppendString( logfile, "Start-up changed from '" + ( (StartType)oldStart ).ToString() + "' to '" + newStart.ToString() + "'" + Environment.NewLine );
                        iServiceChangedCount++;

                    }
                    else {

                        Log.AppendString( logfile, "Start-up: '" + ( (StartType)oldStart ).ToString() + "' (not changed)" + Environment.NewLine );

                    }

                }

                Log.AppendString( logfile, Environment.NewLine );


            }
            catch ( Exception ex ) {

                Log.AppendException( logfile, ex );

            }
            finally {

                if ( sc != null ) {

                    sc.Close();
                    sc.Dispose();
                    sc = null;

                }

            }
            
        }


    }
}
