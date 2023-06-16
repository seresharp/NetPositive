using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPositive.Scanner
{
    internal interface IResultContainer
    {
        void AddResult(ScanResult result);
    }
}
