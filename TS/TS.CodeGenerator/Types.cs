namespace TS.CodeGenerator
{
    public static class Types
    {
        static Types()
        {

            String = "string";
            Number = "number";
            Boolean = "boolean";
            Void = "void";
            Any = "any";
        }

        public static string Any { get; set; }

        public static string Void { get; set; }

        public static string String { get; set; }
        public static string Number { get; set; }
        public static string Boolean { get; set; }
    }
}