using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;

namespace Estudos.Exame.Capitulo2.GenerateCodeAtRunTime
{
    public class CodeDOMStudy
    {
        public static void Test()
        {
            var compileUnit = new CodeCompileUnit();

            // Create a namespace to hold the types we are going to create
            var personnelNameSpace = new CodeNamespace("Personnel");

            // Import the system namespace
            personnelNameSpace.Imports.Add(new CodeNamespaceImport("System"));

            // Create a Person class
            var personClass = new CodeTypeDeclaration("Person")
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public
            };

            // Add the Person class to personnelNamespace
            personnelNameSpace.Types.Add(personClass);

            // Create a field to hold the name of person
            var nameField = new CodeMemberField("String", "name")
            {
                Attributes = MemberAttributes.Private
            };

            // Add the name field to the person class
            personClass.Members.Add(nameField);
            
            // Add the namespace to the document
            compileUnit.Namespaces.Add(personnelNameSpace);

            var provider = CodeDomProvider.CreateProvider("CSharp");
            var s = new StringWriter();
            var options = new CodeGeneratorOptions();
            
            provider.GenerateCodeFromCompileUnit(compileUnit, s, options);
            s.Close();
            
            Console.WriteLine(s.ToString());
        }
    }
}