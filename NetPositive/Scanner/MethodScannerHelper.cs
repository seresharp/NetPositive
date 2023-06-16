using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace NetPositive.Scanner
{
    internal static class MethodScannerHelper
    {
        static Dictionary<string, Tuple<ConstructorInfo, string>> methodScannerConstructors = new();

        static MethodScannerHelper()
        {
            //Use Reflection to initialize Scanners
            var baseModule = typeof(MethodScannerHelper).Module;
            var iScannerType = typeof(IScanner);
            var genericType = typeof(GenericMethodScanner);
            var genericConstructor = genericType.GetConstructor(new Type[0]);
            foreach (var c in baseModule.GetTypes())
            {
                if (c.IsAssignableTo(iScannerType) && c != iScannerType && c!=genericType)
                {
                    try
                    {
                        var constructor = c.GetConstructor(new Type[0]);
                        if (constructor != null)
                            methodScannerConstructors.Add(c.Name, new Tuple<ConstructorInfo, string>(constructor, null));
                        else
                            Console.WriteLine(string.Format("Warning: Couldn't find parameterless constructor for {0}", c.Name));
                    }
                    catch
                    {
                        Console.WriteLine(string.Format("Warning: Error while finding parameterless constructor for {0}", c.Name));
                    }
                }
            }
            //Scan File System to find generic scanner definitions
            string baseDir = Path.GetDirectoryName(baseModule.Assembly.Location);
            string[] files = Directory.GetFiles(Path.Join(baseDir,"./Definitions/Methods"));
            foreach (string file in files)
            {
                var defName = Path.GetFileNameWithoutExtension(file);
                methodScannerConstructors.Add(defName, new Tuple<ConstructorInfo, string>(genericConstructor, file));
            }
        }

        public static IEnumerable<IMethodScanner> GetMethodScannersByName(IEnumerable<string> scannerNames)
        {
            List<IMethodScanner> scanners = new List<IMethodScanner>();
            foreach (string scannerName in scannerNames)
            {
                scanners.Add(GetMethodScannerFromName(scannerName));
            }
            return scanners;
        }

        static IMethodScanner GetMethodScannerFromName(string name)
        {
            Tuple<ConstructorInfo,string> constructorTuple;
            if (!methodScannerConstructors.TryGetValue(name, out constructorTuple))
            {
                throw new Exception(string.Format("Couldn't find constructor for method scanner {0}", name));
            }
            //We check assignableTo IMethodScanner and parameterless when registering constructors,
            //so no need to catch potential Exception here
            var constructor = constructorTuple.Item1;
            var args = constructorTuple.Item2;
            IMethodScanner scanner = (IMethodScanner)constructor.Invoke(new object[0]);
            scanner.InitFromString(args);
            return scanner;
        }
    }
}
