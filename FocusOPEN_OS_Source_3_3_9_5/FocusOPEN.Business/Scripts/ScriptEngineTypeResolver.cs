using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jint;
using System.Reflection;
using System.Threading;
using System.Diagnostics;

namespace FocusOPEN.Business.Scripts
{
    /// <summary>
    /// Ensures types from parent assembly are loaded 
    /// </summary>
    public class ScriptEngineTypeResolver:ITypeResolver
    {

        static Dictionary<string, Type> _Cache = new Dictionary<string, Type>();
        static ReaderWriterLock rwl = new ReaderWriterLock();

        public Type ResolveType(string fullname)
        {
            rwl.AcquireReaderLock(Timeout.Infinite);

            try
            {
                if (_Cache.ContainsKey(fullname))
                {
                    return _Cache[fullname];
                }
            }
            finally
            {
                rwl.ReleaseReaderLock();
            }

            Type type = LoadType(fullname);

            rwl.AcquireWriterLock(Timeout.Infinite);

            try
            {
                _Cache.Add(fullname, type);
                return type;
            }
            finally
            {
                rwl.ReleaseWriterLock();
            }
        }



        private Type LoadType(string fullName)
        {
            Type type = GetLoadedType(fullName);

            if (type == null)
            {
                //check main assemblies references to find type
                AssemblyName[] assemblies = GetEntryAssembly().GetReferencedAssemblies();
                foreach (AssemblyName aName in assemblies)
                {
                    if (!IsAssemblyLoaded(aName.FullName))
                    {
                        Assembly ass = Assembly.Load(aName);

                        type = ass.GetType(fullName, false, false);
                        if (type != null)
                        {
                            //assembly with type has been loaded now
                            break;
                        }

                    }
                }

            }
            return type;
        }

        private Type GetLoadedType(string fullName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type type = assembly.GetType(fullName, false, false);

                if (type != null)
                {
                    return type;
                }

            }
            return null;
        }

        private bool IsAssemblyLoaded(string fullName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                if (assembly.FullName == fullName)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Version of 'GetEntryAssembly' that works with web applications
        /// </summary>
        /// <returns>The entry assembly, or the first called assembly in a web application</returns>
        private Assembly GetEntryAssembly()
        {
            // get the entry assembly
            var result = Assembly.GetEntryAssembly();

            // if none (ex: web application)
            if (result == null)
            {
                // current method
                MethodBase methodCurrent = null;
                // number of frames to skip
                int framestoSkip = 1;


                // loop until we cannot got further in the stacktrace
                do
                {
                    // get the stack frame, skipping the given number of frames
                    StackFrame stackFrame = new StackFrame(framestoSkip);
                    // get the method
                    methodCurrent = stackFrame.GetMethod();
                    // if found
                    if ((methodCurrent != null))
                    {
                        // get its type
                        var typeCurrent = methodCurrent.DeclaringType;
                        // if valid
                        if (typeCurrent != typeof(RuntimeMethodHandle))
                        {
                            // get its assembly
                            var assembly = typeCurrent.Assembly;

                            // if valid
                            if (!assembly.GlobalAssemblyCache && !HasGeneratedCodeAttribute(assembly))
                            {
                                // then we found a valid assembly, get it as a candidate
                                result = assembly;
                            }
                        }
                    }

                    // increase number of frames to skip
                    framestoSkip++;
                } // while we have a working method
                while (methodCurrent != null);
            }
            return result;
        }


        private bool HasGeneratedCodeAttribute(Assembly assembly)
        {
            object[] configAttributes = Attribute.GetCustomAttributes(assembly,
              typeof(System.CodeDom.Compiler.GeneratedCodeAttribute), false);

            return (configAttributes != null && configAttributes.Count() > 0);
        }

    }


}
