using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;

namespace NetPositive.Scanner.Generic.Markers
{ 
    internal class OrMarker:IGenericMarker
    {
        private IGenericMarker marker1, marker2;
        public OrMarker(IGenericMarker marker1, IGenericMarker marker2)
        {
            this.marker1 = marker1;
            this.marker2 = marker2;
        }

        public bool Check(IMethod method)
        {
            return this.marker1.Check(method) || this.marker2.Check(method);
        }
    }
}
