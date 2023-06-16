using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPositive.Scanner
{
    class DebugPathPrintScanner:IScanner
    {
        public void InitFromSpec(string spec) { }
        public void Scan(IResultContainer results, string filePath)
        {
            Console.WriteLine(filePath);
        }
    }
}
