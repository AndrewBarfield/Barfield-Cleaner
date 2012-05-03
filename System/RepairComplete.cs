using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BarfieldCleaner {
    internal static class RepairComplete {

        internal static void Start() {
            Display.WriteTitle( "Completed" );

            // Create Restore Point
            //CreateRestorePoint();

            // Restore Balanced Power Plan
            //PowerManagement.SetActivePower( new Guid( "381b4222-f694-41f0-9685-ff5bb260df2e" ) );

            // Open Device Manager
            try {
                Process.Start( "devmgmt.msc" );
            }
            catch ( Exception ) {
            }

            // Open Windows Updates
            try {
                Process.Start( "wuapp.exe" );
            }
            catch ( Exception ) {
            }

            //Display.WriteSummary( "Balanced Power Plan Reinstated" );
        }

        /*
        private static void CreateRestorePoint() {

            Display.UpdateStatus( "Creating BarfieldCleaner Restore Point..." );
            long num = 0;
            SystemRestore.StartRestore( "BarfieldCleaner Checkpoint", SystemRestore.RestoreType.Checkpoint, out num );
            SystemRestore.EndRestore( num );
            Display.UpdateStatus( "Restore Point Created" );

        }
        */
    }
}
