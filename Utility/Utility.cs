using System;
using System.Collections.Generic;
using System.Text;

namespace BarfieldCleaner
{
            internal static class Utility
            {

                        private const string logfile = "utility.log";



                        private const int scale = 1024;
                        private static readonly string[] orders = new string[] { "TB", "GB", "MB", "KB", "Bytes" };



                        internal static string FormatBytes( long bytes )
                        {

                                    long max = (long)Math.Pow( scale, 4 );

                                    foreach ( string order in orders )
                                    {

                                                if ( bytes >= max )
                                                            return string.Format( "{0:##.##} {1}", decimal.Divide( bytes, max ), order );

                                                max /= scale;
                                    }

                                    return "0 bytes";
                        }



                        internal static string FormatBytes( UInt64 bytes )
                        {

                                    UInt64 max = (UInt64)Math.Pow( scale, 4 );

                                    foreach ( string order in orders )
                                    {

                                                if ( bytes >= max )
                                                            return string.Format( "{0:##.##} {1}", decimal.Divide( bytes, max ), order );

                                                max /= scale;

                                    }

                                    return "0 bytes";
                        }



                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="input"></param>
                        /// <returns></returns>
                        internal static string StripControlChars( string input )
                        {

                                    StringBuilder sb = new StringBuilder();

                                    for ( int i = 0 ; i < input.Length ; i++ )
                                    {
                                                if ( input[i] > 31 && input[i] < 127 )
                                                            sb.Append( input[i] );
                                    }

                                    return sb.ToString();
                        }


            }
}
