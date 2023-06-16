using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPositive.Scanner.Generic
{
    internal interface IGenericMarker
    {
        bool Check(IMethod method);
    }
}
