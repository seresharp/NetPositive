using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPositive.Scanner.Generic.Markers
{
    internal class NotMarker:IGenericMarker
    {
        private IGenericMarker marker;
        public NotMarker(IGenericMarker marker)
        {
            this.marker = marker;
        }

        public bool Check(IMethod method)
        {
            return !this.marker.Check(method);
        }
    }
}
