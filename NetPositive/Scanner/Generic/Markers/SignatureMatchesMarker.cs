using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using dnlib.DotNet;

namespace NetPositive.Scanner.Generic.Markers
{
    internal class SignatureMatchesMarker: IGenericMarker
    {
        internal Regex nameRegEx;
        public SignatureMatchesMarker(string nameRegEx) 
        {
            this.nameRegEx = new Regex(nameRegEx);
        }

        public bool Check(IMethod method)
        {
            return this.nameRegEx.IsMatch(method.ToString());
        }
    }
}
