using System;

namespace TS.CodeGenerator
{
    public interface ITSGenerator : IGenerateTS
    {
        string GenerateLookupTypeName(Type type);
        void AddInterface(Type type);
    }
}