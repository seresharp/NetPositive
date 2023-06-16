using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace NetPositive.Scanner
{
    internal class FileResultContainer:IResultContainer
    {
        internal string outputDir;
        public FileResultContainer(string outDir)
        {
            this.outputDir = outDir;
            if(!Directory.Exists(this.outputDir))
            {
                Directory.CreateDirectory(this.outputDir);
            }
        }

        void IResultContainer.AddResult(ScanResult result)
        {
            var filePath = Path.GetFullPath(result.Method.Module.Location);
            var reducedPath = filePath.Replace('/', '_').Replace('\\', '_').Replace(':','_');
            var outPath = Path.Join(outputDir, reducedPath+".txt");
            bool writeHeader = !File.Exists(outPath);

            using(TextWriter writer = new StreamWriter(outPath, true))
            {
                if(writeHeader)
                    writer.WriteLine(filePath);
                writer.WriteLine();
                writer.Write(result.Risk.ToUpper().PadRight(8));
                writer.Write("====== ");
                writer.Write(result.Method.ToString());
                writer.WriteLine(" ======");
                writer.WriteLine(result.Description);
            }
        }
    }
}
