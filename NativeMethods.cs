
using System;
using System.Text;
using System.Runtime.InteropServices;



namespace BarfieldCleaner
{
	internal static class NativeMethods
	{

		#region Recycle Bin
		//
		[DllImport( "shell32.dll" )]
		static extern int SHEmptyRecycleBin( IntPtr hWnd, string pszRootPath, uint dwFlags );

		// No dialog box confirming the deletion of the objects will be displayed.
		const int SHERB_NOCONFIRMATION = 0x00000001;
		// No dialog box indicating the progress will be displayed. 
		const int SHERB_NOPROGRESSUI = 0x00000002;
		// No sound will be played when the operation is complete. 
		const int SHERB_NOSOUND = 0x00000004;
		#endregion


		#region Display
		//
		[DllImport( "shlwapi.dll", CharSet = CharSet.Auto )]
		internal static extern bool PathCompactPathEx( [Out] StringBuilder pszOut, string szPath, int cchMax, int dwFlags );
		#endregion


		#region Refresh Explorer/Shell

		#region HChangeNotifyEventID
		/// <summary>
		/// Describes the event that has occurred. 
		/// Typically, only one event is specified at a time. 
		/// If more than one event is specified, the values contained 
		/// in the <i>dwItem1</i> and <i>dwItem2</i> 
		/// parameters must be the same, respectively, for all specified events. 
		/// This parameter can be one or more of the following values. 
		/// </summary>
		/// <remarks>
		/// <para><b>Windows NT/2000/XP:</b> <i>dwItem2</i> contains the index 
		/// in the system image list that has changed. 
		/// <i>dwItem1</i> is not used and should be <see langword="null"/>.</para>
		/// <para><b>Windows 95/98:</b> <i>dwItem1</i> contains the index 
		/// in the system image list that has changed. 
		/// <i>dwItem2</i> is not used and should be <see langword="null"/>.</para>
		/// </remarks>
		[Flags]
		internal enum HChangeNotifyEventID
		{
			/// <summary>
			/// All events have occurred. 
			/// </summary>
			SHCNE_ALLEVENTS = 0x7FFFFFFF,

			/// <summary>
			/// A file type association has changed. <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> 
			/// must be specified in the <i>uFlags</i> parameter. 
			/// <i>dwItem1</i> and <i>dwItem2</i> are not used and must be <see langword="null"/>. 
			/// </summary>
			SHCNE_ASSOCCHANGED = 0x08000000,

			/// <summary>
			/// The attributes of an item or folder have changed. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the item or folder that has changed. 
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>.
			/// </summary>
			SHCNE_ATTRIBUTES = 0x00000800,

			/// <summary>
			/// A nonfolder item has been created. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the item that was created. 
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>.
			/// </summary>
			SHCNE_CREATE = 0x00000002,

			/// <summary>
			/// A nonfolder item has been deleted. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the item that was deleted. 
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
			/// </summary>
			SHCNE_DELETE = 0x00000004,

			/// <summary>
			/// A drive has been added. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the root of the drive that was added. 
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
			/// </summary>
			SHCNE_DRIVEADD = 0x00000100,

			/// <summary>
			/// A drive has been added and the Shell should create a new window for the drive. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the root of the drive that was added. 
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
			/// </summary>
			SHCNE_DRIVEADDGUI = 0x00010000,

			/// <summary>
			/// A drive has been removed. <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the root of the drive that was removed.
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
			/// </summary>
			SHCNE_DRIVEREMOVED = 0x00000080,

			/// <summary>
			/// Not currently used. 
			/// </summary>
			SHCNE_EXTENDED_EVENT = 0x04000000,

			/// <summary>
			/// The amount of free space on a drive has changed. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the root of the drive on which the free space changed.
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
			/// </summary>
			SHCNE_FREESPACE = 0x00040000,

			/// <summary>
			/// Storage media has been inserted into a drive. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the root of the drive that contains the new media. 
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
			/// </summary>
			SHCNE_MEDIAINSERTED = 0x00000020,

			/// <summary>
			/// Storage media has been removed from a drive. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the root of the drive from which the media was removed. 
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
			/// </summary>
			SHCNE_MEDIAREMOVED = 0x00000040,

			/// <summary>
			/// A folder has been created. <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> 
			/// or <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the folder that was created. 
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
			/// </summary>
			SHCNE_MKDIR = 0x00000008,

			/// <summary>
			/// A folder on the local computer is being shared via the network. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the folder that is being shared. 
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
			/// </summary>
			SHCNE_NETSHARE = 0x00000200,

			/// <summary>
			/// A folder on the local computer is no longer being shared via the network. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the folder that is no longer being shared. 
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
			/// </summary>
			SHCNE_NETUNSHARE = 0x00000400,

			/// <summary>
			/// The name of a folder has changed. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the previous pointer to an item identifier list (PIDL) or name of the folder. 
			/// <i>dwItem2</i> contains the new PIDL or name of the folder. 
			/// </summary>
			SHCNE_RENAMEFOLDER = 0x00020000,

			/// <summary>
			/// The name of a nonfolder item has changed. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the previous PIDL or name of the item. 
			/// <i>dwItem2</i> contains the new PIDL or name of the item. 
			/// </summary>
			SHCNE_RENAMEITEM = 0x00000001,

			/// <summary>
			/// A folder has been removed. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the folder that was removed. 
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
			/// </summary>
			SHCNE_RMDIR = 0x00000010,

			/// <summary>
			/// The computer has disconnected from a server. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the server from which the computer was disconnected. 
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
			/// </summary>
			SHCNE_SERVERDISCONNECT = 0x00004000,

			/// <summary>
			/// The contents of an existing folder have changed, 
			/// but the folder still exists and has not been renamed. 
			/// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
			/// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
			/// <i>dwItem1</i> contains the folder that has changed. 
			/// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
			/// If a folder has been created, deleted, or renamed, use SHCNE_MKDIR, SHCNE_RMDIR, or 
			/// SHCNE_RENAMEFOLDER, respectively, instead. 
			/// </summary>
			SHCNE_UPDATEDIR = 0x00001000,

			/// <summary>
			/// An image in the system image list has changed. 
			/// <see cref="HChangeNotifyFlags.SHCNF_DWORD"/> must be specified in <i>uFlags</i>. 
			/// </summary>
			SHCNE_UPDATEIMAGE = 0x00008000,

		}
		#endregion // enum HChangeNotifyEventID

		#region HChangeNotifyFlags
		/// <summary>
		/// Flags that indicate the meaning of the <i>dwItem1</i> and <i>dwItem2</i> parameters. 
		/// The uFlags parameter must be one of the following values.
		/// </summary>
		[Flags]
		internal enum HChangeNotifyFlags
		{
			/// <summary>
			/// The <i>dwItem1</i> and <i>dwItem2</i> parameters are DWORD values. 
			/// </summary>
			SHCNF_DWORD = 0x0003,
			/// <summary>
			/// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of ITEMIDLIST structures that 
			/// represent the item(s) affected by the change. 
			/// Each ITEMIDLIST must be relative to the desktop folder. 
			/// </summary>
			SHCNF_IDLIST = 0x0000,
			/// <summary>
			/// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings of 
			/// maximum length MAX_PATH that contain the full path names 
			/// of the items affected by the change. 
			/// </summary>
			SHCNF_PATHA = 0x0001,
			/// <summary>
			/// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings of 
			/// maximum length MAX_PATH that contain the full path names 
			/// of the items affected by the change. 
			/// </summary>
			SHCNF_PATHW = 0x0005,
			/// <summary>
			/// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings that 
			/// represent the friendly names of the printer(s) affected by the change. 
			/// </summary>
			SHCNF_PRINTERA = 0x0002,
			/// <summary>
			/// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings that 
			/// represent the friendly names of the printer(s) affected by the change. 
			/// </summary>
			SHCNF_PRINTERW = 0x0006,
			/// <summary>
			/// The function should not return until the notification 
			/// has been delivered to all affected components. 
			/// As this flag modifies other data-type flags, it cannot by used by itself.
			/// </summary>
			SHCNF_FLUSH = 0x1000,
			/// <summary>
			/// The function should begin delivering notifications to all affected components 
			/// but should return as soon as the notification process has begun. 
			/// As this flag modifies other data-type flags, it cannot by used by itself.
			/// </summary>
			SHCNF_FLUSHNOWAIT = 0x2000
		}
		#endregion // enum HChangeNotifyFlags

		#region CSIDL

		internal enum CSIDL
		{
			CSIDL_DESKTOP = 0x0000,    // <desktop>
			CSIDL_INTERNET = 0x0001,    // Internet Explorer (icon on desktop)
			CSIDL_PROGRAMS = 0x0002,    // Start Menu\Programs
			CSIDL_CONTROLS = 0x0003,    // My Computer\Control Panel
			CSIDL_PRINTERS = 0x0004,    // My Computer\Printers
			CSIDL_PERSONAL = 0x0005,    // My Documents
			CSIDL_FAVORITES = 0x0006,    // <user name>\Favorites
			CSIDL_STARTUP = 0x0007,    // Start Menu\Programs\Startup
			CSIDL_RECENT = 0x0008,    // <user name>\Recent
			CSIDL_SENDTO = 0x0009,    // <user name>\SendTo
			CSIDL_BITBUCKET = 0x000a,    // <desktop>\Recycle Bin
			CSIDL_STARTMENU = 0x000b,    // <user name>\Start Menu
			CSIDL_MYDOCUMENTS = 0x000c,    // logical "My Documents" desktop icon
			CSIDL_MYMUSIC = 0x000d,    // "My Music" folder
			CSIDL_MYVIDEO = 0x000e,    // "My Videos" folder
			CSIDL_DESKTOPDIRECTORY = 0x0010,    // <user name>\Desktop
			CSIDL_DRIVES = 0x0011,    // My Computer
			CSIDL_NETWORK = 0x0012,    // Network Neighborhood (My Network Places)
			CSIDL_NETHOOD = 0x0013,    // <user name>\nethood
			CSIDL_FONTS = 0x0014,    // windows\fonts
			CSIDL_TEMPLATES = 0x0015,
			CSIDL_COMMON_STARTMENU = 0x0016,    // All Users\Start Menu
			CSIDL_COMMON_PROGRAMS = 0X0017,    // All Users\Start Menu\Programs
			CSIDL_COMMON_STARTUP = 0x0018,    // All Users\Startup
			CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019,    // All Users\Desktop
			CSIDL_APPDATA = 0x001a,    // <user name>\Application Data
			CSIDL_PRINTHOOD = 0x001b,    // <user name>\PrintHood

			CSIDL_LOCAL_APPDATA = 0x001c,    // <user name>\Local Settings\Applicaiton Data (non roaming)

			CSIDL_ALTSTARTUP = 0x001d,    // non localized startup
			CSIDL_COMMON_ALTSTARTUP = 0x001e,    // non localized common startup
			CSIDL_COMMON_FAVORITES = 0x001f,

			CSIDL_INTERNET_CACHE = 0x0020,
			CSIDL_COOKIES = 0x0021,
			CSIDL_HISTORY = 0x0022,
			CSIDL_COMMON_APPDATA = 0x0023,    // All Users\Application Data
			CSIDL_WINDOWS = 0x0024,    // GetWindowsDirectory()
			CSIDL_SYSTEM = 0x0025,    // GetSystemDirectory()
			CSIDL_PROGRAM_FILES = 0x0026,    // C:\Program Files
			CSIDL_MYPICTURES = 0x0027,    // C:\Program Files\My Pictures

			CSIDL_PROFILE = 0x0028,    // USERPROFILE
			CSIDL_SYSTEMX86 = 0x0029,    // x86 system directory on RISC
			CSIDL_PROGRAM_FILESX86 = 0x002a,    // x86 C:\Program Files on RISC

			CSIDL_PROGRAM_FILES_COMMON = 0x002b,    // C:\Program Files\Common

			CSIDL_PROGRAM_FILES_COMMONX86 = 0x002c,    // x86 Program Files\Common on RISC
			CSIDL_COMMON_TEMPLATES = 0x002d,    // All Users\Templates

			CSIDL_COMMON_DOCUMENTS = 0x002e,    // All Users\Documents
			CSIDL_COMMON_ADMINTOOLS = 0x002f,    // All Users\Start Menu\Programs\Administrative Tools
			CSIDL_ADMINTOOLS = 0x0030,    // <user name>\Start Menu\Programs\Administrative Tools

			CSIDL_CONNECTIONS = 0x0031,    // Network and Dial-up Connections
			CSIDL_COMMON_MUSIC = 0x0035,    // All Users\My Music
			CSIDL_COMMON_PICTURES = 0x0036,    // All Users\My Pictures
			CSIDL_COMMON_VIDEO = 0x0037,    // All Users\My Video

			CSIDL_CDBURN_AREA = 0x003b    // USERPROFILE\Local Settings\Application Data\Microsoft\CD Burning
		}
		#endregion

		//
		[DllImport( "user32.dll", SetLastError = true )]
		internal static extern IntPtr SendMessageTimeout( IntPtr hWnd, int Msg, IntPtr wParam, string lParam, uint fuFlags, uint uTimeout, IntPtr lpdwResult );

		// Notifies the system of an event that an application has performed.
		// An application should use this function if it performs an action that may affect the Shell. 
		[DllImport( "shell32.dll" )]
		internal static extern void SHChangeNotify( HChangeNotifyEventID wEventId, HChangeNotifyFlags uFlags, IntPtr dwItem1, IntPtr dwItem2 );

		//
		[DllImport( "shell32.dll", SetLastError = true )]
		static extern int SHGetSpecialFolderLocation( IntPtr hwndOwner, CSIDL nFolder, ref IntPtr ppidl );

		//
		[DllImport( "user32.dll", CharSet = CharSet.Auto )]
		internal static extern IntPtr SendMessage( IntPtr hwndOwner, int msg, IntPtr wParam, IntPtr lParam );

		//
		private static readonly IntPtr HWND_BROADCAST = new IntPtr( 0xffff );
		private static readonly IntPtr REFRESH = new IntPtr( 0x7103 );

		private const int WM_SETTINGCHANGE = 0x1a;
		private const int SMTO_ABORTIFHUNG = 0x0002;
		private const int WM_COMMAND = 0x0111;
		#endregion


        #region Power Management

            internal static readonly Guid GUID_MAX_POWER_SAVINGS = new Guid( 0xA1841308, 0x3541, 0x4FAB, 0xBC, 0x81, 0xF7, 0x15, 0x56, 0xF2, 0x0B, 0x4A );
            internal static readonly Guid GUID_MIN_POWER_SAVINGS = new Guid( 0x8C5E7FDA, 0xE8BF, 0x4A96, 0x9A, 0x85, 0xA6, 0xE2, 0x3A, 0x8C, 0x63, 0x5C );
            internal static readonly Guid GUID_TYPICAL_POWER_SAVINGS = new Guid( 0x381B4222, 0xF694, 0x41F0, 0x96, 0x85, 0xFF, 0x5B, 0xB2, 0x60, 0xDF, 0x2E );

            internal static readonly Guid NO_SUBGROUP_GUID = new Guid( 0xfea3413e, 0x7e05, 0x4911, 0x9a, 0x71, 0x70, 0x03, 0x31, 0xf1, 0xc2, 0x94 );
            internal static readonly Guid GUID_VIDEO_SUBGROUP = new Guid( 0x7516b95f, 0xf776, 0x4464, 0x8c, 0x53, 0x06, 0x16, 0x7f, 0x40, 0xcc, 0x99 );

            internal const uint ERROR_INVALID_PARAMETER = 87;
            internal const uint ERROR_MORE_DATA = 234;
            internal const uint ERROR_SUCCESS = 0;
            internal const uint ERROR_ERRORS_ENCOUNTERED = 774;

            internal enum POWER_DATA_ACCESSOR {
                ACCESS_SCHEME = 16,
                ACCESS_SUBGROUP = 17,
                ACCESS_INDIVIDUAL_SETTING = 18
            }

            [DllImportAttribute( "powrprof.dll", EntryPoint = "PowerEnumerate" )]
            internal static extern uint PowerEnumerate(IntPtr RootPowerKey, IntPtr SchemeGuid, IntPtr SubGroupOfPowerSettingsGuid, POWER_DATA_ACCESSOR AccessFlags, uint Index, ref Guid Buffer, ref uint BufferSize);
            [DllImportAttribute( "powrprof.dll", EntryPoint = "PowerReadFriendlyName" )]
            internal static extern uint PowerReadFriendlyName(IntPtr RootPowerKey, ref Guid SchemeGuid, IntPtr SubGroupOfPowerSettingsGuid, IntPtr PowerSettingGuid, IntPtr Buffer, ref uint BufferSize);
            [DllImportAttribute( "powrprof.dll", EntryPoint = "PowerGetActiveScheme" )]
            internal static extern uint PowerGetActiveScheme(IntPtr UserPowerKey, out IntPtr ActivePolicyGuid);
            [DllImportAttribute( "powrprof.dll", EntryPoint = "PowerSetActiveScheme" )]
            internal static extern uint PowerSetActiveScheme(IntPtr UserPowerKey, ref Guid ActivePolicyGuid);


            internal enum POWER_INFORMATION_LEVEL {
                AdministratorPowerPolicy = 9,
                LastSleepTime = 15,
                LastWakeTime = 14,
                ProcessorInformation = 11,
                ProcessorPowerPolicyAc = 18,
                ProcessorPowerPolicyCurrent = 22,
                ProcessorPowerPolicyDc = 19,
                SystemBatteryState = 5,
                SystemExecutionState = 16,
                SystemPowerCapabilities = 4,
                SystemPowerInformation = 12,
                SystemPowerPolicyAc = 0,
                SystemPowerPolicyCurrent = 8,
                SystemPowerPolicyDc = 1,
                SystemReserveHiberFile = 10,
                SystemWakeSource = 35,
                VerifyProcessorPowerPolicyAc = 20,
                VerifyProcessorPowerPolicyDc = 21,
                VerifySystemPolicyAc = 2,
                VerifySystemPolicyDc = 3
            }

            [StructLayoutAttribute( System.Runtime.InteropServices.LayoutKind.Sequential )]
            internal struct SYSTEM_BATTERY_STATE {
                internal byte AcOnLine;
                internal byte BatteryPresent;
                internal byte Charging;
                internal byte Discharging;
                [MarshalAsAttribute( System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1 )]
                internal byte[] Spare1;
                internal UInt32 MaxCapacity;
                internal UInt32 RemainingCapacity;
                internal Int32 Rate;
                internal UInt32 EstimatedTime;
                internal UInt32 DefaultAlert1;
                internal UInt32 DefaultAlert2;
            }

            [DllImport( "powrprof.dll", EntryPoint = "CallNtPowerInformation", SetLastError = true )]
            internal static extern UInt32 CallNtPowerInformation(POWER_INFORMATION_LEVEL InformationLevel, IntPtr lpInputBuffer, UInt32 nInputBufferSize, IntPtr lpOutputBuffer, UInt32 nOutputBufferSize);

        #endregion


        /// <summary>
		/// Broadcasts a message that user settings have changed and then forces a desktop/explorer refresh
		/// </summary>
		internal static void BroadcastSettingsChange()
		{

			try
			{

				// HWND_BROADCAST = "All top-level windows"
				// A message that is sent to all top-level windows when the SystemParametersInfo function changes
				// a system-wide setting or when policy settings have changed.
				//
				// http://msdn.microsoft.com/en-us/library/ms725497(VS.85).aspx
				SendMessageTimeout( HWND_BROADCAST, WM_SETTINGCHANGE, IntPtr.Zero, null, SMTO_ABORTIFHUNG, 100, IntPtr.Zero );


				// Refresh the Desktop (it's like hitting F5)
				SendMessageTimeout( HWND_BROADCAST, WM_COMMAND, REFRESH, null, SMTO_ABORTIFHUNG, 1000, IntPtr.Zero );
				//SendMessage( HWND_BROADCAST, WM_COMMAND, REFRESH, IntPtr.Zero );


				// Notifies the system of an event that an application has performed. An application should use
				// this function if it performs an action that may affect the Shell.
				//
				// http://msdn.microsoft.com/en-us/library/bb762118(VS.85).aspx
				SHChangeNotify( HChangeNotifyEventID.SHCNE_ASSOCCHANGED, HChangeNotifyFlags.SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero );

			}
			catch ( Exception )
			{
			}

		}



		/// <summary>
		/// Force explorer window to update a location
		/// </summary>
		/// <param name="specialItemID"></param>
		internal static void RefreshSpecialItem( CSIDL specialItemID )
		{
			try
			{

				IntPtr pidl = IntPtr.Zero;

				if ( SHGetSpecialFolderLocation( IntPtr.Zero, specialItemID, ref pidl ) == 0 )
					SHChangeNotify( HChangeNotifyEventID.SHCNE_ALLEVENTS, HChangeNotifyFlags.SHCNF_IDLIST | HChangeNotifyFlags.SHCNF_FLUSH, pidl, IntPtr.Zero );

			}
			catch ( Exception )
			{
			}

		}


		/// <summary>
		/// Shortens a Path and add ellipsis
		/// </summary>
		/// <param name="path"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		internal static string PathShortener( string path, int length )
		{

			StringBuilder sb = new StringBuilder( length + 1 );
			PathCompactPathEx( sb, path, length, 0 );
			return sb.ToString();

		}


		/// <summary>
		/// Empties the Recycle Bin
		/// </summary>
		/// <param name="rootPath"></param>
		internal static void EmptyRecycleBin( string rootPath )
		{

			try
			{

				SHEmptyRecycleBin( IntPtr.Zero, rootPath, SHERB_NOCONFIRMATION );

			}
			catch ( Exception )
			{
			}

		}

	}
}
