using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetPositive.Scanner.Generic.Markers
{
    internal class ClassCallsMarker: IGenericMarker
    {
        internal Regex nameRegEx;
        public ClassCallsMarker(string nameRegEx)
        {
            this.nameRegEx = new Regex(nameRegEx);
        }

        public bool Check(IMethod method)
        {
            var dt = method.DeclaringType;
            var resolvedDt = dt.ResolveTypeDef();
            if(resolvedDt == null)
            {
                return false;
            }
            foreach(var classMethod in resolvedDt.Methods)
            {
                if (classMethod.CallsMethod(this.nameRegEx))
                    return true;

            }
            return false;
        }
    }
}
