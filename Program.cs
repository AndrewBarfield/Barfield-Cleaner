
// Attributes, Console, Exception
using System;

using System.Collections.Generic;

// Process, ProcessPriorityClass
using System.Diagnostics;

// PermissionSetAttribute
using System.Security.Permissions;

// Thread, ThreadPriority
using System.Threading;


[assembly: CLSCompliant( true )]
[assembly: PermissionSetAttribute( SecurityAction.RequestMinimum,
    Name = "FullTrust" )]
namespace BarfieldCleaner {

    class Program {

        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args) {

            // Only the OS is 'more important' than the cleaner
            SetRealTimePriority();

            // Format the Console
            Display.Format();

            // Create the log folder in the root of the system drive
            Log.CreateFolder();

            Repair();

            Console.Read();

        }

        /// <summary>
        /// Repairs and configures the system
        /// </summary>
        private static void Repair() {

            Display.PrintRepairStartBanner();

            PowerManagement.Start();
            Processes.Start();
            RegistryCleaner.Start();
            Services.Start();
            Network.Start();
            FileSystem.Start();
            //MalwareCleaner.Start();
            RepairComplete.Start();

            Display.PrintRepairEndBanner();
            
        }

        /// <summary>
        /// Set realtime priority for internal processing
        /// </summary>
        internal static void SetRealTimePriority() {

            Process p = Process.GetCurrentProcess();

            if ( p != null ) {

                // Change Process priority
                try {

                    p.PriorityClass = ProcessPriorityClass.RealTime;
                    p.PriorityBoostEnabled = true;

                }
                catch ( Exception ) {
                }

                // Change Thread priority
                try {
                    Thread.CurrentThread.Priority = ThreadPriority.Highest;
                }
                catch ( Exception ) {
                }

            }

        }


        /// <summary>
        /// Set normal priority when launching a process
        /// </summary>
        internal static void SetNormalPriority() {

            Process p = Process.GetCurrentProcess();

            if ( p != null ) {

                // Change Process priority
                try {
                    p.PriorityClass = ProcessPriorityClass.Normal;
                }
                catch ( Exception ) {
                }

                // Change Thread priority
                try {
                    Thread.CurrentThread.Priority = ThreadPriority.Normal;
                }
                catch ( Exception ) {
                }
            }

        }

    }
}
