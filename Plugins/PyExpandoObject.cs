using System;
using System.Collections.Generic;
using System.Dynamic;
using Python.Runtime;

namespace Unison.Extensions
{
    /// <summary>
    /// 付与したメソッドは自動的にRPC
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class PyRPCAttribute : Attribute
    {
        private readonly string m_command;
        // private readonly string m_description;

        public string Command => m_command;

        // public string Description => m_description;

        public PyRPCAttribute(string command, string description = "")
        {
            m_command = command;
            // m_description = description;
        }
    }

    /// TODO: intelisense
    /// 
    /// <summary>
    /// Dynamic Member Lookup type for Python
    /// </summary>
    ///
    /// <code>
    /// py = new PyExpandoObject();
    /// py.sys.version_info
    /// py.my_value = true;
    /// </code>
    public class PyExpandoObject : DynamicObject
    {
        private Dictionary<string, PyObject> sysModule;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            dynamic module = Py.Import(binder.Name);
            result = module;
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // オブジェクトはスコープ内で変換
            if (value.GetType().IsClass)
            {
                using (var scope = Py.CreateScope())
                {
                    // convert the binder object to a PyObject
                    var pyPerson = binder.ToPython();

                    // create a Python variable "person"
                    scope.Set("person", pyPerson);

                    // the person object may now be used in Python
                    var code = "fullName = person.FirstName + ' ' + person.LastName";
                    scope.Exec(code);
                }
            }
            // プリミティブはそのまま戻す
            else
            {
                binder.Name.ToPython().SetAttr(binder.Name, value.ToPython());
            }

            return true;
        }
    }
}