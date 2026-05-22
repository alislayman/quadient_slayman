using Suggester;
using Suggester.Interfaces;

IDifferenceScorer differenceScorer = new DifferenceScorer();
ISuggester suggester = new Suggester.Suggester(differenceScorer);
string[] demoChoices = new[] { "gros", "gras", "graisse", "agressif", "go", "ros", "gro" };
IEnumerable<string> demoSuggestions = suggester.GetSuggestions(GlobalConst.Demo.DemoTerm, demoChoices, GlobalConst.Demo.DemoNumberOfSuggestions);
Console.WriteLine(GlobalConst.Demo.SuggestionsOutputPrefix + string.Join(GlobalConst.Demo.SuggestionsJoinSeparator, demoSuggestions));
