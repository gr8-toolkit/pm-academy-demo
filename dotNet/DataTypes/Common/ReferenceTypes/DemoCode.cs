using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ReferenceTypes
{
    internal class DemoCode
    {
        public object ObjExample1Value => 12243;
        public object ObjExample2Value => 22 / 7;
        public object ObjExample3Value => "3.14";
        public object ObjExample4Value => null;
        public string StringExampleValue => "text";
        public string StringNullValue => null;

        public dynamic DynExample1Value => 12243;
        public dynamic DynExample2Value => "dyn";

        delegate void DelegateExample();
        delegate void DelegateExampleWithStr(string name);

        public int[] Int32ArrayExample1 => new int[] { 1, 4, 5 };           // 1,4,5
        public int[] Int32ArrayExample2 => new int[3];                      // 0, 0, 0

    }
}
