using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPositive.Scanner
{
    internal class CLIResultContainer:IResultContainer
    {
        void IResultContainer.AddResult(ScanResult result)
        {
            var filePath = Path.GetFullPath(result.Method.Module.Location);
            Console.WriteLine(filePath);
            Console.Write(result.Risk.ToUpper().PadRight(8));
            Console.Write("====== ");
            Console.Write(result.Method.ToString());
            Console.WriteLine(" ======");
            Console.WriteLine(result.Description);
            Console.WriteLine();
        }
    }
}
