using System;
using System.IO;
using System.Linq;
using System.Security.Permissions;

namespace FSTraversal
{
    class FSTraversal
    {
        ///////////////////////main/////////////////////////
        static void Main(string[] args)
        {
          Console.WriteLine("Traversal Started.\n");

          if(args == null || args.Length != 1)
          {
              Console.WriteLine("ERROR: INVALID ARGUMENT");
              return;
          }

          string path = args[0];
          bool confirmed = false;
          string Key;

          Console.WriteLine("You have defined the following dir: {0}\n", path);
          //ShowAllFoldersUnder(path, 0);
            // Keep the console window open in debug mode.

            ///TODO: Need to clean this up
          do {
              Key = Console.ReadLine();

                ConsoleKey response;
                do
                {
                    Console.Write("Are you sure you want to enable Watchery on this dir? [y/n] ");
                    response = Console.ReadKey(false).Key;   // true is intercept key (dont show), false is show
                    if (response != ConsoleKey.Enter)
                        Console.WriteLine();

                } while (response != ConsoleKey.Y && response != ConsoleKey.N);

                confirmed = response == ConsoleKey.Y;
              } while (!confirmed);
              Console.WriteLine(" ACTIVATING ON {0}...", path);
              Console.ReadLine();

          Console.ReadKey();
        } //main


        private static void ShowAllFoldersUnder(string path, int indent)
        {
            if (path == null) return;

            try
            {

//////TRAVERSE OPTION 1 : LINQ
                if (Directory.Exists(path))
                  Directory.GetFiles(path).ToList().ForEach(s => { gFiInfoviaPath(s); });
                else
                  Console.WriteLine("ERROR: PATH NOT FOUND");

//////TRAVERSE OPTION 2 : RECURSIVE
                // if ((File.GetAttributes(path) & FileAttributes.ReparsePoint)!= FileAttributes.ReparsePoint)
                // {
                //     foreach (string folder in Directory.GetDirectories(path))
                //     {
                //         Console.WriteLine("{0}{1}", new string('-', indent), Path.GetFileName(folder));
                //         ShowAllFoldersUnder(folder, indent + 2);
                //     }
                // }

            }
            catch (UnauthorizedAccessException) { }
        }



        private static void gFiInfoviaPath(string path)
        {
          try
          {
            string extension = Path.GetExtension(path);
            string filename = Path.GetFileName(path);
            string filenameNoExtension = Path.GetFileNameWithoutExtension(path);
            string root = Path.GetPathRoot(path);
            Console.WriteLine("\nFile Extenson: {0}\n --> FILENAME: {1}\nRoot Dir: {2}\n",extension,filenameNoExtension,root);
          }
          catch (UnauthorizedAccessException) { }
        }

        [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
            public static void Run()
            {
                string[] args = System.Environment.GetCommandLineArgs();

                // If a directory is not specified, exit program.
                if(args.Length != 2)
                {
                    // Display the proper way to call the program.
                    Console.WriteLine("Usage: Watcher.exe (directory)");
                    return;
                }

                // Create a new FileSystemWatcher and set its properties.
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = args[1];
                /* Watch for changes in LastAccess and LastWrite times, and
                   the renaming of files or directories. */
                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                // Only watch text files.
                watcher.Filter = "*.txt";

                // Add event handlers.
                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnChanged);
                watcher.Deleted += new FileSystemEventHandler(OnChanged);
                watcher.Renamed += new RenamedEventHandler(OnRenamed);

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Press \'q\' to quit the sample.");
                while(Console.Read()!='q');
            }

            // Define the event handlers.
            private static void OnChanged(object source, FileSystemEventArgs e)
            {
                // Specify what is done when a file is changed, created, or deleted.
               Console.WriteLine("File: " +  e.FullPath + " " + e.ChangeType);
            }

            private static void OnRenamed(object source, RenamedEventArgs e)
            {
                // Specify what is done when a file is renamed.
                Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
            }

    } //class
}
