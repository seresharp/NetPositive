using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPositive.Scanner
{
    internal class ScanResult
    {
        public IMethod Method;
        public string Risk { get; set; }
        public string Description { get; set; }
    }
}
