using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NetPositive
{
    static class ArgumentParser
    {
        public static ScanArguments ParseArguments(string[] argv)
        {
            ScanArguments result = new ScanArguments();
            for(int i=0; i<argv.Length; i++)
            {
                if( argv[i]=="-r" )
                {
                    result.recursive = getNextArgAsBool(argv, true, ref i);
                }
                else if(argv[i] == "-t")
                {
                    result.basePaths.Add(getNextArgAsString(argv, "", ref i));
                }
                else if(argv[i] == "--allowPathEscape")
                {
                    result.allowPathEscape = getNextArgAsBool(argv, false, ref i);
                }
                else if (argv[i] == "-S")
                {
                    result.scanSpec.AddRange(parseNextArgAsSpec(argv, ref i));
                }
                else if(argv[i] == "-O")
                {
                    result.outputPath = getNextArgAsString(argv, null, ref i);
                }
            }
            return result;
        }

        private static bool getNextArgAsBool(string[] argv, bool defaultVal, ref int idx)
        {
            if(idx+1 < argv.Length)
            {
                try
                {
                    bool result = bool.Parse(argv[idx + 1]);
                    idx += 1;
                    return result;
                }
                catch
                {
                    //Fall through to default value
                }
            }
            return defaultVal;
        }

        private static string getNextArgAsString(string[] argv, string defaultVal, ref int idx)
        {
            if (idx + 1 < argv.Length)
            {
                try
                {
                    string result = argv[idx + 1];
                    idx += 1;
                    return result;
                }
                catch
                {
                    //Fall through to default value
                }
            }
            return defaultVal;
        }

        private static IEnumerable<string> parseNextArgAsSpec(string[] argv, ref int idx)
        {
            if(idx+1>=argv.Length)
            {
                return new string[0];
            }
            idx++;
            return argv[idx].Split(";");
        }
    }
}
