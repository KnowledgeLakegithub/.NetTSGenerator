using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.test.a.b
{
    public interface ITestGenericClass<T>
    {
        T GetT(T input);

        IEnumerable<int> data { get; set; }
    }
}
