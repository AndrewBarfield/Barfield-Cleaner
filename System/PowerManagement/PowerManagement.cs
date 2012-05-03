using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
//using System.Windows.Forms;
using Microsoft.Win32;

namespace BarfieldCleaner {

    /// <summary>
    /// Avoid system sleep, hibernation, and monitor shutoff while BarfieldCleaner is running.
    /// </summary>
    static class PowerManagement {

        /*
        Key = 381b4222-f694-41f0-9685-ff5bb260df2e, Value = Balanced
        Key = 534803cb-bde6-4ad1-874a-b72e8b915713, Value = BarfieldCleaner Power Plan
        Key = 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c, Value = High performance
        Key = a1841308-3541-4fab-bc81-f71556f20b4a, Value = Power saver
        */

        internal static void Start() {

            // Activate High performance plan
            SetActivePower( new Guid( "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c" ) );
        }

        internal static Dictionary<Guid, string> GetPowerEnumeration() {
            Guid SchemeGuid = Guid.Empty;
            uint index = 0;
            uint BufferSize = (uint)Marshal.SizeOf( SchemeGuid );

            Dictionary<Guid, string> schemes = new Dictionary<Guid, string>();
            while ( 0 == NativeMethods.PowerEnumerate( (IntPtr)null, (IntPtr)null, (IntPtr)null, NativeMethods.POWER_DATA_ACCESSOR.ACCESS_SCHEME, index, ref SchemeGuid, ref BufferSize ) ) {
                schemes.Add( SchemeGuid, GetPowerName( SchemeGuid ) );

                index++;
            }

            return schemes;
        }

        internal static string GetPowerName(Guid SchemeGuid) {
            string name = string.Empty;
            IntPtr lpszName = (IntPtr)null;
            uint dwSize = 0;
            NativeMethods.PowerReadFriendlyName( (IntPtr)null, ref SchemeGuid, (IntPtr)null, (IntPtr)null, lpszName, ref dwSize );
            if ( dwSize > 0 ) {
                lpszName = Marshal.AllocHGlobal( (int)dwSize );
                if ( 0 == NativeMethods.PowerReadFriendlyName( (IntPtr)null, ref SchemeGuid, (IntPtr)null, (IntPtr)null, lpszName, ref dwSize ) ) {
                    name = Marshal.PtrToStringUni( lpszName );
                }
                if ( lpszName != IntPtr.Zero )
                    Marshal.FreeHGlobal( lpszName );
            }

            return name;
        }

        internal static KeyValuePair<Guid, string> GetActivePower() {
            KeyValuePair<Guid, string> active = new KeyValuePair<Guid, string>();
            Guid ActiveScheme = Guid.Empty;
            IntPtr ptr = Marshal.AllocHGlobal( Marshal.SizeOf( typeof( IntPtr ) ) );
            if ( 0 == NativeMethods.PowerGetActiveScheme( (IntPtr)null, out ptr ) ) {
                ActiveScheme = (Guid)Marshal.PtrToStructure( ptr, typeof( Guid ) );

                active = new KeyValuePair<Guid, string>( ActiveScheme, GetPowerName( ActiveScheme ) );

                if ( ptr != null ) {
                    Marshal.FreeHGlobal( ptr );
                }
            }

            return active;

        }

        internal static void SetActivePower(Guid scheme) {
            try {
                NativeMethods.PowerSetActiveScheme( (IntPtr)null, ref scheme );
            }
            catch ( Exception ) {
            }
        }

    }
}
