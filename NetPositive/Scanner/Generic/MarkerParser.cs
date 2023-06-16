using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetPositive.Scanner.Generic.Markers;

namespace NetPositive.Scanner.Generic
{
    static internal class MarkerParser
    {
        public static int FindParanthesesBalancedSeparator(string text)
        {
            int paranthesesState = 0;
            for(int i=0; i<text.Length; i++)
            {
                switch(text[i])
                {
                    case '(':
                        paranthesesState++;
                        break;
                    case ')':
                        paranthesesState--;
                        break;
                    case ',':
                        if (paranthesesState == 0)
                            return i;
                        break;
                }
            }
            return -1;
        }

        public static Tuple<string,string> ParseArgs(string text)
        {
            int paranthesesState = 0;
            int argsStart = 0;
            int argsEnd = 0;
            for (int i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '(':
                        if (paranthesesState == 0)
                            argsStart = i+1;
                        paranthesesState++;
                        break;
                    case ')':
                        paranthesesState--;
                        if (paranthesesState == 0)
                        {
                            argsEnd = i;
                            i = text.Length; //Dirty hack to break out of the for loop
                        }
                        break;
                }
            }
            if (paranthesesState != 0)
                throw new Exception(string.Format("Missing closing bracket in \"{0}\"",text));
            return new Tuple<string, string>(text.Substring(0, argsStart - 1), text.Substring(argsStart, argsEnd-argsStart));
        }

        public static string PreprocessText(string text)
        {
            //TODO: Transform text to allow the use of &&, || and ! for boolean operations in markers
            return text;
        }
        public static IGenericMarker MarkerFromString(string marker, string arguments)
        {
            var split = FindParanthesesBalancedSeparator(arguments);
            string left = null;
            string right = null;
            if (split >= 0)
            {
                left = arguments.Substring(0, split);
                right = arguments.Substring(split + 1);
            }
            switch (marker)
            {
                case "SignatureMatches":
                    return new SignatureMatchesMarker(arguments);
                case "Calls":
                    return new CallsMarker(arguments);
                case "ClassCalls":
                    return new ClassCallsMarker(arguments);
                case "And":
                    return new AndMarker(MarkerFromString(left), MarkerFromString(right));
                case "Or":
                    return new OrMarker(MarkerFromString(left), MarkerFromString(right));
                case "Not":
                    return new NotMarker(MarkerFromString(arguments));
                default:
                    throw new NotImplementedException(string.Format("Marker Type \"{0}\" unknown",marker));

            }
        }
        public static IGenericMarker MarkerFromString(string text)
        {
            text = PreprocessText(text);
            var (marker, args) = ParseArgs(text);
            return MarkerFromString(marker, args);
        }
    }
}
