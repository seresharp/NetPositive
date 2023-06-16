using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;

namespace NetPositive.Scanner
{
    internal interface IMethodScanner
    {
        void InitFromString(string str);
        void Scan(IResultContainer results, IMethod method);
    }
}
