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
        return Enumerable.Empty<string>();
    }
}
