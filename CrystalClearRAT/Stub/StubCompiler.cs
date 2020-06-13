using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClearRAT.Stub
{
    public static class StubCompiler
    {

        public static void Compile()
        {
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.dll" }, "foo.exe", true);
            parameters.CompilerOptions = "/target:winexe";
            parameters.GenerateExecutable = true;
            CompilerResults results = csc.CompileAssemblyFromSource(parameters,
            @"using System;
            class Program {
              public static void Main(string[] args) {
               // Console.WriteLine('Hello world');
                Console.ReadKey();
              }
            }");
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
        }

    }
}
