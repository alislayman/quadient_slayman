using System;
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
        return choices.Where(choice => choice.Length >= term.Length).Select(choice => new { ChoiceValue = choice, MinimumScore = ComputeMinimumWindowScore(term, choice), LengthDelta = Math.Abs(choice.Length - term.Length) }).OrderBy(rankedChoice => rankedChoice.MinimumScore).ThenBy(rankedChoice => rankedChoice.LengthDelta).Take(numberOfSuggestions).Select(rankedChoice => rankedChoice.ChoiceValue);
    }

    private int ComputeMinimumWindowScore(string term, string choice)
    {
        if (term.Length == 0)
        {
            return 0;
        }
        int minimumScore = int.MaxValue;
        int lastWindowStartIndex = choice.Length - term.Length;
        for (int windowStartIndex = 0; windowStartIndex <= lastWindowStartIndex; windowStartIndex++)
        {
            string currentWindow = choice.Substring(windowStartIndex, term.Length);
            int currentWindowScore = _differenceScorer.GetDifferenceScore(term, currentWindow);
            if (currentWindowScore < minimumScore)
            {
                minimumScore = currentWindowScore;
            }
        }
        return minimumScore;
    }
}
