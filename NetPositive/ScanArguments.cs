using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPositive
{
    class ScanArguments
    {
        public bool recursive = false;
        public bool allowPathEscape = false;
        public List<string> basePaths = new();
        public List<string> scanSpec = new();
        public string outputPath = null;
    }
}
