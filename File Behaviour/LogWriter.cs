using System;
using System.IO;

namespace File_Behaviour
{
    class LogWriter
    {
        //Log files stored in windows C directory.
        private String logDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\filelog" + System.DateTime.Now.ToString("yyyyMMdd") + ".fmlog";
        //CSV header for rename and edit event log
        private const String logHeader = "Timestamp,Name,Full_Path,Old_Name,Old_Path,Type";
        private StreamWriter writer;
        private const string comma = ",";

        private System.Threading.Semaphore sema;


        public LogWriter()
        {
            Console.WriteLine(logDirectory);
            sema = new System.Threading.Semaphore(initialCount: 1, maximumCount: 1, name: "monitorSema");

            try
            {
                if (!File.Exists(logDirectory))
                {
                    using (writer = File.CreateText(logDirectory))
                    {
                        //write csv header to file
                        writer.WriteLine(logHeader);
                        writer.Flush();
                        writer.Close();

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

        }

        public void logShort(string timestamp, string name, string path, string type)
        {
            //generate csv string
            String formattedString = timestamp + comma;
            formattedString += name + comma;
            formattedString += path + comma;
            formattedString += type;

            sema.WaitOne();
            using (writer = File.AppendText(logDirectory))
            {
                //Log a longer formatted csv string entry
                writer.WriteLine(formattedString);
                writer.Flush();
                writer.Close();
            }
            sema.Release();
        }

        public void logLong(string timestamp, string name, string path, string oldName, string oldPath, string type)
        {
            //generate csv string 
            String formattedString = timestamp + comma;
            formattedString += name + comma;
            formattedString += path + comma;
            formattedString += oldName + comma;
            formattedString += oldPath + comma;
            formattedString += type;

            sema.WaitOne();
            using (writer = File.AppendText(logDirectory))
            {
                //Log a longer formatted csv string entry
                writer.WriteLine(formattedString);
                writer.Flush();
                writer.Close();
            }
            sema.Release();
        }
    }
}
