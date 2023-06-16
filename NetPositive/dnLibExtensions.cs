using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.Text.RegularExpressions;

namespace NetPositive
{
    public static class dnLibExtensions
    {
        private static Dictionary<IMethod, List<string>> CallCache = new Dictionary<IMethod, List<string>>();

        private static void GenerateCallCache(IMethod method)
        {
            List<string> calls;
            if(!CallCache.TryGetValue(method, out calls))
            {
                calls = new List<string>();
            }
            var methodDef = method.ResolveMethodDef();
            if(methodDef != null && methodDef.HasBody)
            {
                foreach(var inst in methodDef.Body.Instructions)
                {
                    var opCode = inst.GetOpCode();
                    if(opCode == OpCodes.Call || opCode == OpCodes.Calli || opCode == OpCodes.Callvirt)
                    {
                        calls.Add(inst.GetOperand().ToString());
                    }
                }
            }
            CallCache[method] = calls;
        }

        private static List<string> GetCalls(IMethod method)
        {
            if(!CallCache.ContainsKey(method))
                GenerateCallCache(method);
            return CallCache[method];
        }

        public static void WipeCache()
        {
            CallCache.Clear();
        }

        public static bool CallsMethod(this IMethod method, Regex nameRegex)
        {
            var calls = GetCalls(method);
            foreach(var call in calls)
            {
                if(nameRegex.IsMatch(call))
                    return true;
            }
            return false;
        }
    }
}
