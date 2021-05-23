using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;

namespace Zen.Utilities.Reflection
{
    internal sealed class DynModuleBlder
    {
        const TypeAttributes ClassTypeAttributes = TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AutoLayout;
        const MethodAttributes ConstructorAttributes = MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
        const MethodAttributes VirtualMethodAttributes = MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual;

        public DynModuleBlder()
            : this("Anoymous")
        {
        }

        public DynModuleBlder(string asmName)
        {
            _asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(asmName), AssemblyBuilderAccess.RunAndSave);
            _builder = _asmBuilder.DefineDynamicModule("Dynamic.Module");
        }

        /// <summary>
        /// Delegate is a single method type.
        /// </summary>
        public Type BuildDelegateType<TypeT>(TypeT queryType, string methodName) where TypeT : Type
        {
            MethodInfo methodInfo = queryType.GetMethod(methodName);
            if (methodInfo == null)
                return null;

            TypeBuilder typeBlder = _builder.DefineType(string.Format("{0}.{1}_Delegate", queryType.FullName, methodName), ClassTypeAttributes, typeof(MulticastDelegate), PackingSize.Unspecified);
            ConstructorBuilder conBlder = typeBlder.DefineConstructor(ConstructorAttributes, CallingConventions.Standard, new Type[] { typeof(object), typeof(IntPtr) });
            conBlder.SetImplementationFlags(MethodImplAttributes.Runtime);

            ParameterInfo[] parameters = methodInfo.GetParameters();
            Type[] paramTypes = new Type[parameters.Length];
            for (int n = 0; n < parameters.Length; n++)
                paramTypes[n] = parameters[n].ParameterType;

            Type methodReturnT = methodInfo.ReturnType;
            MethodBuilder mb = typeBlder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, methodReturnT, paramTypes);
            mb.SetImplementationFlags(MethodImplAttributes.Runtime);

            return typeBlder.CreateType();
        }

        #region private data
        private AssemblyBuilder _asmBuilder = null;
        private ModuleBuilder _builder = null;
        #endregion
    }

    /// <summary>
    /// Invoker helper.
    ///   Maintains delegate type & object instance cache
    ///   Do *NOT* hold it longer than needed
    /// </summary>
    public sealed class Invoker
    {
        const string DelegateMethodFmt = "{0}.{1}";
        const string BindMethodFailureMsgFmt = "GetMethod('{0}') call is failed.";

        public object InvokeSetProperty(ISynchronizeInvoke obj, string objId, string propertyName, params object[] paramValues)
        {
            return InvokeMethod(obj, objId, "set_" + propertyName, paramValues);
        }
        public object InvokeGetProperty(ISynchronizeInvoke obj, string objId, string propertyName)
        {
            return InvokeMethod(obj, objId, "get_" + propertyName, null);
        }

        public object InvokeMethod(ISynchronizeInvoke obj, string objId, string methodName, params object[] paramValues)
        {
            lock (_lockObj)
            {
                try
                {
                    Delegate del = null;
                    Type invokeeType = obj.GetType();
                    string typeMethodKey = string.Format(DelegateMethodFmt, invokeeType.FullName, methodName);

                    // #1. Check and prepare the Delegate Type map
                    Type delType = null;
                    if (_delegateTypeMap.ContainsKey(typeMethodKey))
                    {
                        delType = _delegateTypeMap[typeMethodKey];
                    }
                    else
                    {
                        delType = _delegatesBuilder.BuildDelegateType(invokeeType, methodName);
                        if (delType == null)
                            throw new Exception(string.Format(BindMethodFailureMsgFmt, methodName));
 
                        _delegateTypeMap.Add(typeMethodKey, delType);
                    }

                    // #2. Check and prepare the Delegate instance map
                    if (!_delegateInstanceMap.ContainsKey(typeMethodKey))
                        _delegateInstanceMap.Add(typeMethodKey, new Dictionary<string, Delegate>());

                    // #3. Check if instance needs added.
                    string objKey = objId;
                    Dictionary<string, Delegate> objMap = _delegateInstanceMap[typeMethodKey];
                    if (objMap.ContainsKey(objKey))
                    {
                        del = objMap[objKey];
                    }
                    else
                    {
                        del = MulticastDelegate.CreateDelegate(delType, obj, methodName);
                        objMap.Add(objKey, del);
                    }

                    return obj.Invoke(del, paramValues);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void ClearInstanceMap()
        {
            foreach (KeyValuePair<string, Dictionary<string, Delegate>> item in _delegateInstanceMap)
                item.Value.Clear();

            _delegateInstanceMap.Clear();
        }

        #region private data
        private DynModuleBlder _delegatesBuilder = new DynModuleBlder("Anoymous.Invoker");
        private Dictionary<string, Type> _delegateTypeMap = new Dictionary<string, Type>();
        private Dictionary<string, Dictionary<string, Delegate>> _delegateInstanceMap = new Dictionary<string, Dictionary<string, Delegate>>();

        private object _lockObj = new object();
        #endregion
    }
}