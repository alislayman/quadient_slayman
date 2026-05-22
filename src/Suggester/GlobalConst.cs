namespace Suggester;

public static class GlobalConst
{
    public static class Messages
    {
        public const string StringsMustHaveSameLengthMessage = "Strings must have the same length.";
    }

    public static class Demo
    {
        public const string DemoTerm = "gros";
        public const int DemoNumberOfSuggestions = 2;
        public const string SuggestionsOutputPrefix = "Suggestions for 'gros' (N=2): ";
        public const string SuggestionsJoinSeparator = ", ";
    }
}
