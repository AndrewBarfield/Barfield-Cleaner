using System;
using System.IO;

namespace BarfieldCleaner
{
            internal static class Search
            {

                        //private const string logfile = "search.log";

                        internal delegate void FoundFileEventHandler( FileInfo file );
                        internal static event FoundFileEventHandler FileFoundEvent;

                        internal delegate void FoundFolderEventHandler( DirectoryInfo directory );
                        internal static event FoundFolderEventHandler FolderFoundEvent;


                        /// <summary>
                        /// Finds all files and folders in currentDirectory
                        /// </summary>
                        /// <param name="d">Starting folder</param>
                        /// <param name="Query">Any legal text string</param>
                        internal static void Query( DirectoryInfo currentDirectory )
                        {

                                    // Tell user that we're currently searching 'currentDirectory'
                                    Display.UpdateStatus( "Searching: " + NativeMethods.PathShortener( currentDirectory.FullName, 61 ) );


                                    // For each file fire a 'found file' event
                                    try
                                    {

                                                FileInfo[] files = currentDirectory.GetFiles();
                                                foreach ( FileInfo file in files )
                                                {
                                                            if ( FileFoundEvent != null )
                                                                        FileFoundEvent( file );
                                                }

                                    }
                                    catch ( Exception )
                                    {

                                                //Log.AppendException( logfile, ex );

                                    }


                                    if ( FolderFoundEvent != null )
                                                FolderFoundEvent( currentDirectory );


                                    try
                                    {

                                                DirectoryInfo[] dirs = currentDirectory.GetDirectories();
                                                
                                                foreach ( DirectoryInfo dir in dirs )
                                                {

                                                            if ( ( File.GetAttributes( dir.FullName ) & FileAttributes.ReparsePoint ) != FileAttributes.ReparsePoint )
                                                                        Query( dir );
                                                }

                                    }
                                    catch ( Exception )
                                    {

                                                //Log.AppendException( logfile, ex );

                                    }



                        }



            }
}
