using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Python.Runtime;
using Unison.Extensions;

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#elif GODOT
using Godot;
#endif

namespace Unison.Bind
{
    /**
     * <summary>
     * 指定namespace.class以下を取得
     * https://mitosuya.net/execute-all-class-in-namespace
     * </summary>
     */
    public class PythonNetBinder
    {
        private static CodeNamespace nameSpace;

        #region Comment

        private const string _template = @"
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Date Time: YMD
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
        ";

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static PythonNetBinder Gen()
        {
            nameSpace = new CodeNamespace(name: "Client");
            var time = DateTime.Now.ToString("yyyy/MM/dd");
            var comment = new CodeCommentStatement(new CodeComment(_template.Replace("YMD", time)));
            nameSpace.Comments.Add(comment);
            nameSpace.Imports.Add(new CodeNamespaceImport(nameSpace: "System"));

            var mainClass = new CodeTypeDeclaration(name: "Runtime");
#if UNITY
            nameSpace.Imports.Add(new CodeNamespaceImport(nameSpace: "UnityEngine"));
            mainClass.BaseTypes.Add(new CodeTypeReference(typeof(MonoBehaviour)));
#elif GODOT
            nameSpace.Imports.Add(new CodeNamespaceImport(nameSpace: "Godot"));
            mainClass.BaseTypes.Add(new CodeTypeReference(typeof(Node)));
#endif

            var assemblies = new HashSet<Assembly>
            {
                Assembly.GetAssembly(typeof(PythonNetBinder)), Assembly.Load("Assembly-CSharp")
            };

            foreach (var assembly in assemblies)
            {
                foreach (var typeName in assembly.GetExportedTypes())
                {
                    var fields =
                        typeName.GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Instance);
                    foreach (var field in fields)
                    {
                        var variable = new CodeMemberField(field.FieldType.Name, field.Name);
#if UNITY
                var codeAttrDecl = new CodeAttributeDeclaration(
                    "SerializeField",
                    new CodeAttributeArgument(new CodePrimitiveExpression(false)));
                variable.CustomAttributes.Add(codeAttrDecl);
#endif
                        mainClass.Members.Add(variable);
                    }


                    var methods = typeName.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                                      BindingFlags.Instance | BindingFlags.Static |
                                                      BindingFlags.DeclaredOnly);
                    foreach (var method in methods)
                    {
                        foreach (var attribute in method.GetCustomAttributes(typeof(PyRPCAttribute), false))
                        {
                            if (attribute is PyRPCAttribute consoleMethod)
                                continue;
                        }

                        var mainMethod = new CodeMemberMethod
                        {
                            ReturnType = new CodeTypeReference(method.ReturnType),
                            Attributes = MemberAttributes.Public | MemberAttributes.Final,
                            Name = method.Name
                        };

                        CodeExpression target;
                        if ((method.Attributes & MethodAttributes.Static) != 0)
                            target = new CodeSnippetExpression(typeName.FullName);
                        else
                            target = new CodeObjectCreateExpression(typeName.FullName);

                        var invoke = new CodeMethodInvokeExpression(
                            targetObject: target, methodName: method.Name
                        );

                        foreach (var p in method.GetParameters())
                        {
                            invoke.Parameters.Add(new CodeArgumentReferenceExpression(p.Name));
                            var exp = new CodeParameterDeclarationExpression(p.ParameterType, p.Name);
                            mainMethod.Parameters.Add(exp);
                        }

                        if (method.ReturnType.Name != "Void")
                            mainMethod.Statements.Add(new CodeMethodReturnStatement(invoke));
                        else
                            mainMethod.Statements.Add(invoke);

                        mainClass.Members.Add(mainMethod);
                    }
                }
            }
            return new PythonNetBinder();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        void Compile(string fileName)
        {
            if (fileName.EndsWith(".cs"))
            {
                ToCode(fileName);
                return;
            }

            // https://stackoverflow.com/questions/23551757/what-are-the-possible-parameters-for-compilerparameters-compileroptions
            var compileParameters = new CompilerParameters
            {
                OutputAssembly = fileName,
                CompilerOptions = "/optimize+ /target:library /unsafe"
            };
            var codeCompileUnit = new CodeCompileUnit();
#if UNITY
            codeCompileUnit.ReferencedAssemblies.Add("UnityEngine.dll");
            codeCompileUnit.ReferencedAssemblies.Add("UnityEditor.dll");
#elif GODOT
            codeCompileUnit.ReferencedAssemblies.Add("GodotSharp.dll");
#elif INHOUSE
            codeCompileUnit.ReferencedAssemblies.Add("");
#endif
            codeCompileUnit.Namespaces.Add(nameSpace);

            var snippets = new CodeCompileUnit[] { };
            snippets.SetValue(codeCompileUnit, 0);

            for (var i = 1; i < AppDomain.CurrentDomain.GetAssemblies().Length; i++)
            {
                var file = AppDomain.CurrentDomain.GetAssemblies()[i];
                var fstring = File.OpenText(file.Location);
                var unit = CodeDomProvider.CreateProvider("C#").Parse(fstring);
                snippets.SetValue(unit, i);
                fstring.Close();
            }
            

            var result = CodeDomProvider.CreateProvider("C#")
                .CompileAssemblyFromDom(compileParameters, compilationUnits: snippets);

            foreach (var str in result.Output)
            {
                Console.WriteLine(str);
            }
        }

        void ToCode(string fileName, string type = "C#")
        {
            var codeText = new StringBuilder();
            using (var codeWriter = new StringWriter(codeText))
            {
                var compilerOptions = new CodeGeneratorOptions
                {
                    IndentString = "    ", BracingStyle = type
                };
                CodeDomProvider.CreateProvider(type)
                    .GenerateCodeFromNamespace(nameSpace, codeWriter, compilerOptions);
            }

            using (var writer = new StreamWriter(fileName))
            {
                writer.Write(codeText);
            }
        }
    }


    public class RPC
    {
        private static dynamic server;
        private static Dictionary<Action<string>, string> _functions = new Dictionary<Action<string>, string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public static void RpcServer(string address, int port)
        {
            GetAttributes();

            using (Py.GIL())
            {
                try
                {
                    dynamic server = Py.Import("xmlrpc.server");
                    var param = new PyObject[] {new PyString(address), new PyInt(port)};
                    server = server.SimpleXMLRPCServer(
                        new PyTuple(param), server.SimpleXMLRPCRequestHandler, false, true);

                    foreach (var func in _functions)
                    {
                        server.register_function(func.Key, func.Value.ToPython());
                    }

                    server.register_multicall_functions();
                    server.serve_forever();
                }
                catch (PythonException e)
                {
                    server.server_close();
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        static void Register(string command, string description, MethodInfo method)
        {
            Action<string> action;

            if (method.GetType() == typeof(PyObject))
                action = (Action<string>) Delegate.CreateDelegate(typeof(Action<string>), method);
            else
                action = method as dynamic;

            _functions.Add(action, command);
        }
        
        static Dictionary<Action<string>, string> GetAttributes()
        {
            var assemblies = new HashSet<Assembly>
            {
                Assembly.GetAssembly(typeof(PythonNetBinder)), Assembly.Load("Assembly-CSharp")
            };

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetExportedTypes())
                {
                    foreach (var method in type.GetMethods(
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly))
                    {
                        foreach (var attribute in method.GetCustomAttributes(typeof(PyRPCAttribute), false))
                        {
                            if (attribute is PyRPCAttribute consoleMethod)
                            {
                                Register(consoleMethod.Command, consoleMethod.Description, method);
                            }
                        }
                    }
                }
            }
            // TODO: Play, Stop, Goto などのラッパー登録

            return _functions;
        }
    }
}