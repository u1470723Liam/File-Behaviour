using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Behaviour
{
    class Program
    {
        static void Main(string[] args)
        {
            String input = Console.ReadLine();
            if (System.IO.Directory.Exists(input))
            {
                List<string> allDirs = new List<string>();
                //loop through and add all child directories
                allDirs.AddRange(getChildDirectories(input));
                if (allDirs.Count > 0)
                {
                    FileMonitor fm = new FileMonitor(allDirs.ToArray(), input);
                    fm.start();
                }
            }
            else
            {
                List<string> allDirs = new List<string>();
                //loop through and add all child directories
                allDirs.AddRange(getChildDirectories(@"C:\"));
                if (allDirs.Count > 0)
                {
                    FileMonitor fm = new FileMonitor(allDirs.ToArray(), @"C:\");
                    fm.start();
                }
            }
        }

        static List<string> getChildDirectories(string parent)
        {
            List<string> childDirs = new List<string>();
            foreach (string directory in Directory.GetDirectories(parent))
            {
                try
                {
                    childDirs.Add(directory);
                    childDirs.AddRange(getChildDirectories(directory));
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            return childDirs;
        }
    }
}
