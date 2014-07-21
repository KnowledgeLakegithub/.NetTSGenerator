using System.IO;

namespace TS.CodeGenerator
{
    public interface IAssemblyReader
    {
        Stream GenerateTypingsStream();
        string GenerateTypingsString();
    }
}