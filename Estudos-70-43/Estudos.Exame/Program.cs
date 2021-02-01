using System;
using System.Threading;
using System.Threading.Tasks;
using Estudos.Exame.Capitulo1.CreateAndImplementEventsAndCallbacks.Delegates;
using Estudos.Exame.Capitulo1.CreateAndImplementEventsAndCallbacks.Delegates.Action;
using Estudos.Exame.Capitulo1.ImplementProgramFlow;
using Estudos.Exame.Capitulo2.Attributes;
using Estudos.Exame.Capitulo2.ConvertTypes;
using Estudos.Exame.Capitulo2.CreatAndUseTypes;
using Estudos.Exame.Capitulo2.Enumerator;
using Estudos.Exame.Capitulo2.EvaluateRegularExpressionToValidateInput;
using Estudos.Exame.Capitulo2.FormatString;
using Estudos.Exame.Capitulo2.GenerateCodeAtRunTime;
using Estudos.Exame.Capitulo2.GenerateCodeAtRunTime.ExpressionTree;
using Estudos.Exame.Capitulo2.ManageTheObjectLifeCycle;
using Estudos.Exame.Capitulo2.ManipulateStringByUsingThe_StringBuilderStringWriterAndStringReader;
using Estudos.Exame.Capitulo2.StringComparasionAndCultures;
using Estudos.Exame.Capitulo2.SystemReflection;
using Estudos.Exame.Capitulo2.SystemReflection.FindComponentssInAssemblies;
using Estudos.Exame.Capitulo2.UseExpandoObject;
using Estudos.Exame.Capitulo3.CreateAndMonitorPerformanceCounters;
using Estudos.Exame.Capitulo3.Data_Integrity_By_Hashing_Data;
using Estudos.Exame.Capitulo3.Encrypting_Streams;
using Estudos.Exame.Capitulo3.Implement_Diagnostics_In_An_Application;
using Estudos.Exame.Capitulo3.ImplementingPublicAndPrivateKeyManagement;
using Estudos.Exame.Capitulo3.SymmetricAndAsymmetricEncryption;
using Estudos.Exame.Capitulo4.Query_And_Manipulate_Data_And_Objects_By_Using_Linq;
using Estudos.Exame.Capitulo4.Read_And_Write_Files_And_Streams;
using Estudos.Exame.Capitulo4.Serialize_And_Deserialize_Data_By_Using_Serializations;

namespace Estudos.Exame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            var assemblyName = typeof(Program).Assembly.FullName;
            Console.WriteLine(assemblyName);

            Binary_Serialization.Test();
        }
    }
}