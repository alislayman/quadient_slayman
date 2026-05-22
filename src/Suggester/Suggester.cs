using System.Collections.Generic;
using System.Linq;
using Suggester.Interfaces;

namespace Suggester;

public sealed class Suggester : ISuggester
{
    private readonly IDifferenceScorer _differenceScorer;

    public Suggester(IDifferenceScorer differenceScorer)
    {
        _differenceScorer = differenceScorer;
    }

    public IEnumerable<string> GetSuggestions(string term, IEnumerable<string> choices, int numberOfSuggestions)
    {
        if (numberOfSuggestions <= 0)
        {
            return Enumerable.Empty<string>();
        }
        return choices.Where(choice => choice.Length >= term.Length).Select(choice => new { ChoiceValue = choice, MinimumScore = _differenceScorer.GetDifferenceScore(term, choice.Substring(0, term.Length)) }).OrderBy(rankedChoice => rankedChoice.MinimumScore).Take(numberOfSuggestions).Select(rankedChoice => rankedChoice.ChoiceValue);
    }
}
