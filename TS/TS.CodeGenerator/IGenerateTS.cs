using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.CodeGenerator
{
    public interface IGenerateTS
    {
        void Initialize();
        string ToTSString();
    }

}
