using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using dnlib.DotNet;

namespace NetPositive.Scanner.Generic
{
    internal class GenericDefinition
    {
        List<GenericSignature> signatureList = new();
        public GenericDefinition(JsonElement definition) 
        {
            if(definition.ValueKind != JsonValueKind.Array)
                throw new ArgumentException("Definition Element must be a JSON array of signatures");
            for(int i=0; i<definition.GetArrayLength(); i++)
            {
                signatureList.Add(new GenericSignature(definition[i]));
            }
        }

        public void Scan(IResultContainer results, IMethod method)
        {
            for(int i=0; i<signatureList.Count; i++)
            {
                if (signatureList[i].Scan(results, method))
                    break;
            }
        }
    }
}
