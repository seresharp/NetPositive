using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPositive.Scanner
{
    interface IScanner
    {
        void InitFromSpec(string spec);
        void Scan(IResultContainer results, string filePath);
    }
}
