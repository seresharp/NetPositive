using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;


namespace NetPositive.Scanner
{
    internal class GenericFileMethodScanner:IScanner
    {
        List<IMethodScanner> methodScanners = new List<IMethodScanner>();
        ModuleContext moduleContext = ModuleDef.CreateModuleContext();

        public void InitFromSpec(string spec)
        {
            var methodScannerNames = spec.Split(":");
            methodScanners.AddRange(MethodScannerHelper.GetMethodScannersByName(methodScannerNames));
        }

        void IScanner.Scan(IResultContainer results, string filePath)
        {
            try
            {
                using (ModuleDef moduleDef = ModuleDefMD.Load(filePath, moduleContext))
                {
                    dnLibExtensions.WipeCache();
                    foreach (var type in moduleDef.Types)
                        foreach (var method in type.Methods)
                            foreach (var scanner in methodScanners)
                            {
                                try
                                {
                                    scanner.Scan(results, method);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.ToString());
                                }
                            }
                }
            }
            catch(BadImageFormatException bife)
            {
                //Do nothing
            }
            catch (Exception ex)
            {
                //Do nothing, presumable if things fail it's because the module couldn't be loaded
            }
        }
    }
}
