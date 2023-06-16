using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace NetPositive.Scanner
{
    internal static class ScannerHelper
    {
        static Dictionary<string, ConstructorInfo> scannerConstructors = new Dictionary<string, ConstructorInfo>();
        static ScannerHelper()
        {
            //Use Reflection to initialize Scanners
            var baseModule = typeof(ScannerHelper).Module;
            var iScannerType = typeof(IScanner);
            foreach(var c in baseModule.GetTypes())
            {
                if(c.IsAssignableTo(iScannerType) && c!=iScannerType)
                {
                    try
                    {
                        var constructor = c.GetConstructor(new Type[0]);
                        if (constructor != null)
                            scannerConstructors.Add(c.Name, constructor);
                        else
                            Console.WriteLine(string.Format("Warning: Couldn't find parameterless constructor for {0}", c.Name));
                    }
                    catch
                    {
                        Console.WriteLine(string.Format("Warning: Error while finding parameterless constructor for {0}", c.Name));
                    }
                }
            }
        }

        public static IEnumerable<IScanner> GetScannersFromSpec(IEnumerable<string> spec)
        {
            List<IScanner> scanners = new();
            foreach (string s in spec)
            {
                scanners.Add(GetScannerFromSpec(s));
            }
            return scanners;
        }

        public static IScanner GetScannerFromSpec(string spec)
        {
            var split = spec.Split(new char[] { ':' },2);
            var name = split[0];
            var args = split.Length>1?split[1]:"";
            ConstructorInfo constructor;
            if(!scannerConstructors.TryGetValue(name, out constructor))
            {
                throw new Exception(string.Format("Couldn't find constructor for class {0}", name));
            }
            //We check assignableTo IScanner and parameterless when registering constructors,
            //so no need to catch potential Exception here
            IScanner scanner = (IScanner)constructor.Invoke(new object[0]);
            scanner.InitFromSpec(args);
            return scanner;
        }
    }
}
