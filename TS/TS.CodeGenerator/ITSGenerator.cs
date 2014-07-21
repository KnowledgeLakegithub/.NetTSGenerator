using System;

namespace TS.CodeGenerator
{
    public interface ITSGenerator
    {
        string GenerateLookupTypeName(Type type);
        void AddInterface(Type type);
        string ToTSString();
    }
}