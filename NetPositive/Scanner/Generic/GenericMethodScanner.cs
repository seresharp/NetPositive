using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using NetPositive.Scanner.Generic;

namespace NetPositive.Scanner
{
    class GenericMethodScanner:IMethodScanner
    {
        string defFile;
        List<GenericDefinition> genericDefinitions = new();
        public void InitFromString(string str)
        {
            this.defFile = str;
            using (var stream = File.OpenRead(this.defFile))
            {
                var definitionDocument = JsonDocument.Parse(stream);
                if (definitionDocument.RootElement.ValueKind != JsonValueKind.Array)
                    throw new Exception("Root element of a method scanner definition has to be a JSON array.");
                for (int i = 0; i<definitionDocument.RootElement.GetArrayLength(); i++)
                {
                    var curr = definitionDocument.RootElement[i];
                    if(curr.ValueKind == JsonValueKind.Array)
                        genericDefinitions.Add(new GenericDefinition(curr));
                    else if(curr.ValueKind == JsonValueKind.String)
                    {
                        //TODO: Handle potential of circular dependencies
                        //Handle Import directives to allow meta definitions that bundle others
                        string importDirective = curr.GetString();
                        if (!importDirective.StartsWith("Import:"))
                            throw new ArgumentException("The only valid string definitions are Imports");
                        string importTarget = importDirective.Replace("Import:", "");
                        string importPath = Path.Join(Path.GetDirectoryName(defFile), importTarget);
                        var loader = new GenericMethodScanner();
                        loader.InitFromString(importPath);
                        genericDefinitions.AddRange(loader.genericDefinitions);
                    }
                }
            }
        }
        public void Scan(IResultContainer results, IMethod method)
        {
            for(int i=0; i<genericDefinitions.Count; i++)
            {
                genericDefinitions[i].Scan(results, method);
            }
        }
    }
}
