using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.IO;


namespace BarfieldCleaner
{
	internal static class SystemInformation
	{

		/// <summary>
		/// 
		/// </summary>
		internal static string GetWMIProperty( string Class, string PropertyName )
		{

			string result = string.Empty;

			//
			ManagementObjectSearcher searcher = new ManagementObjectSearcher( "SELECT " + PropertyName + " FROM " + Class );


			//
			foreach ( ManagementObject mo in searcher.Get() )
			{
				result = mo[PropertyName].ToString();
				break;
			}


			// Cleanup
			if ( searcher != null )
			{
				searcher.Dispose();
				searcher = null;
			}


			//
			return result.Trim();


		}



		/// <summary>
		/// Represents Hardware Properties
		/// </summary>
		internal static class Hardware
		{

			/// <summary>
			/// Represnets Processor properties
			/// </summary>
			internal static class Processor
			{

				/// <summary>
				/// Gets the CPU name
				/// </summary>
				internal static string Name
				{
					get
					{

						return GetWMIProperty( "Win32_Processor", "Name" );

					}
				}


				/// <summary>
				/// Gets the CPU Socket Designation
				/// </summary>
				internal static string SocketDesignation
				{
					get
					{

						return GetWMIProperty( "Win32_Processor", "SocketDesignation" );

					}
				}

				

			}


			/// <summary>
			/// Represents RAM properties
			/// </summary>
			internal static class RAM
			{

				/// <summary>
				/// 
				/// </summary>
				internal static UInt64 Total
				{
					get
					{

						UInt64 total = 0;

						ManagementObjectSearcher searcher = new ManagementObjectSearcher( "SELECT Capacity FROM Win32_PhysicalMemory" );

						foreach ( ManagementObject mo in searcher.Get() )
						{
							total += (UInt64)mo["Capacity"];
						}

						return total;
					}
				}



				/// <summary>
				/// 
				/// </summary>
				internal static int ModuleCount
				{
					get
					{

						ManagementObjectSearcher searcher = new ManagementObjectSearcher( "SELECT Capacity FROM Win32_PhysicalMemory" );
						return searcher.Get().Count;

					}
				}


				/// <summary>
				/// Gets RAM speed in MHz
				/// </summary>
				internal static UInt32 Speed
				{
					get
					{

						UInt32 speed = 0;

						ManagementObjectSearcher searcher = new ManagementObjectSearcher( "SELECT Speed FROM Win32_PhysicalMemory" );

						foreach ( ManagementObject mo in searcher.Get() )
						{
							speed = (UInt32)mo["Speed"];
						}

						return speed;

					}
				}



				/// <summary>
				/// 
				/// </summary>
				internal static string Type
				{
					get
					{

						UInt16 type = 0;

						ManagementObjectSearcher searcher = new ManagementObjectSearcher( "SELECT MemoryType FROM Win32_PhysicalMemory" );

						foreach ( ManagementObject mo in searcher.Get() )
						{
							type = (UInt16)mo["MemoryType"];
						}

						switch ( type )
						{

							case 0:
								return "Type Unknown";

							case 1:
								return "Other";

							case 2:
								return "DRAM";

							case 3:
								return "Synchronous DRAM";

							case 4:
								return "Cache DRAM";

							case 5:
								return "EDO";

							case 6:
								return "EDRAM";

							case 7:
								return "VRAM";

							case 8:
								return "SRAM";

							case 9:
								return "RAM";

							case 10:
								return "ROM";

							case 11:
								return "Flash";

							case 12:
								return "EEPROM";

							case 13:
								return "FEPROM";

							case 14:
								return "EPROM";

							case 15:
								return "CDRAM";

							case 16:
								return "3DRAM";

							case 17:
								return "SDRAM";

							case 18:
								return "SGRAM";

							case 19:
								return "RDRAM";

							case 20:
								return "DDR";

							case 21:
								return "DDR-2";

							default:
								return "(ERROR)";

						}


					}
				}


			}

		}




		
		/// <summary>
		/// Represents OS properties
		/// </summary>
		internal static class OperatingSystem
		{

			/// <summary>
			/// Gets the Operating System friendly name
			/// </summary>
			internal static string FriendlyName
			{
				get
				{

					return GetWMIProperty( "Win32_OperatingSystem", "Caption" );

				}
			}



			/// <summary>
			/// Gets the Operating System serial number
			/// </summary>
			internal static string SerialNumber
			{
				get
				{

					return GetWMIProperty( "Win32_OperatingSystem", "SerialNumber" );

				}
			}



		}
		


		/// <summary>
		/// 
		/// </summary>
		internal static class SystemDrive
		{

			/// <summary>
			/// Drive Capacity
			/// </summary>
			internal static long Capacity
			{
				get
				{

					return new DriveInfo( RootPath ).TotalSize;

				}
			}


			/// <summary>
			/// Drive Capacity in human readable format
			/// </summary>
			internal static string CapacityHumanReadable
			{
				get
				{

					return Utility.FormatBytes( new DriveInfo( RootPath ).TotalSize );

				}
			}


			/// <summary>
			/// System folder root. Usually "C:\"
			/// </summary>
			internal static string RootPath
			{
				get
				{
					return Path.GetPathRoot( Environment.GetFolderPath( Environment.SpecialFolder.System ) );
				}
			}


		}



		/// <summary>
		/// Wrapper and extension of Environment.SpecialFolder
		/// </summary>
		internal static class SpecialFolders
		{


			/// <summary>
			/// Windows folder. Usually "C:\Windows"
			/// </summary>
			public static string Windows
			{
				get
				{
					return Directory.GetParent( Environment.GetFolderPath( Environment.SpecialFolder.System ) ).FullName;
				}
			}



			/// <summary>
			/// System folder. Usually "C:\Windows\System32"
			/// </summary>
			public static string System
			{
				get
				{
					return Environment.GetFolderPath( Environment.SpecialFolder.System );
				}
			}



		}



	}
}
