using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;

namespace File_Behaviour
{
    class FileMonitor
    {
        //   private string directory;
        private string[] dirs;
        private bool enabled;
        //public List<FileEvent> events;
        private LogWriter writer;
        private FileSystemWatcher watcher;

        public FileMonitor(string[] dirs, String outDir)
        {
            this.dirs = dirs;
            this.enabled = true;
            //this.events = new List<FileEvent>();
            this.writer = new LogWriter();
        }

        [IODescriptionAttribute("FileSystemWatcherDesc")]
        [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
        [PermissionSetAttribute(SecurityAction.InheritanceDemand, Name = "FullTrust")]

        public void monitor()
        {
            foreach (string directory in dirs)
            {
                if (!directory.Contains("System Volume Information"))
                {
                    // Create a new FileSystemWatcher and set its properties.
                    watcher = new FileSystemWatcher();
                    watcher.Path = directory;
                    /* Watch for changes in LastAccess and LastWrite times, and
                        the renaming of files or directories. */
                    watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
                    // Only watch text files.
                    watcher.Filter = "*.*";

                    // Add event handlers.
                    watcher.Changed += new FileSystemEventHandler(OnCreateChangeDelete);
                    watcher.Created += new FileSystemEventHandler(OnCreateChangeDelete);
                    watcher.Deleted += new FileSystemEventHandler(OnCreateChangeDelete);
                    watcher.Renamed += new RenamedEventHandler(OnRenamed);

                    // Begin watching.
                    try
                    {
                        watcher.EnableRaisingEvents = true;
                    }
                    catch (Exception e)
                    {
                        //Avoid invisible directories
                        continue;
                    }
                }
            }
            while (Console.ReadLine() != "exit")
            {
                watcher.EnableRaisingEvents = false;
            };
        }


        private void OnCreateChangeDelete(object source, FileSystemEventArgs e)
        {
            if (e.Name != ("filelog" + System.DateTime.Now.ToString("yyyyMMdd") + ".fmlog"))
            {
                /*old method
                FileEvent fileEvent = new FileEvent(e.Name, e.FullPath, e.ChangeType.ToString());
                events.Add(fileEvent);*/

                string timestamp = System.DateTime.Now.ToString("yyyyMMddHHmmssffff");
                Console.WriteLine(e.ChangeType.ToString() + " " + e.Name);
                writer.logShort(timestamp, e.Name, e.FullPath, e.ChangeType.ToString());
            }
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            if (e.Name != ("filelog" + System.DateTime.Now.ToString("yyyyMMdd") + ".fmlog"))
            {

                /*old method
                FileEvent fileEvent = new FileEvent(e.Name, e.FullPath, e.ChangeType.ToString());
                fileEvent.oldName = e.OldName;
                fileEvent.oldPath = e.OldFullPath;
                fileEvent.fileSize = new FileInfo(e.FullPath).Length;
                events.Add(fileEvent);*/

                string timestamp = System.DateTime.Now.ToString("yyyyMMddHHmmssffff");
                Console.WriteLine(e.ChangeType.ToString() + " " + e.OldName + " " + e.Name);
                writer.logLong(timestamp, e.Name, e.FullPath, e.OldName, e.OldFullPath, e.ChangeType.ToString());
            }
        }

        public void start()
        {
            enabled = true;
            monitor();
        }
        public void stop()
        {
            enabled = false;
        }
    }
}
