using Suggester.Interfaces;

namespace Suggester;

public sealed class Suggester : ISuggester
{
    private readonly IDifferenceScorer _differenceScorer;

    public Suggester(IDifferenceScorer differenceScorer)
    {
        _differenceScorer = differenceScorer ?? throw new ArgumentNullException(nameof(differenceScorer));
    }

    private readonly struct Candidate
    {
        public readonly string Choice;
        public readonly int Score;
        public readonly int LengthDelta;

        public Candidate(string choice, int score, int lengthDelta)
        {
            Choice = choice;
            Score = score;
            LengthDelta = lengthDelta;
        }
    }

    public IEnumerable<string> GetSuggestions(string term, IEnumerable<string> choices, int numberOfSuggestions)
    {
        if (term == null)
            throw new ArgumentNullException(nameof(term));

        if (choices == null)
            throw new ArgumentNullException(nameof(choices));

        if (numberOfSuggestions <= 0)
            return Array.Empty<string>();

        List<Candidate> candidates = new List<Candidate>();
        foreach (string choice in choices)
        {
            if (choice == null)
                continue;

            if (choice.Length < term.Length)
                continue;

            int score = ComputeMinimumWindowScore(term, choice);
            candidates.Add(new Candidate(choice, score, Math.Abs(choice.Length - term.Length)));
        }

        candidates.Sort((a, b) =>
        {
            int comparison = a.LengthDelta.CompareTo(b.LengthDelta);
            if (comparison != 0)
                return comparison;

            comparison = a.Score.CompareTo(b.Score);
            if (comparison != 0)
                return comparison;

            return string.CompareOrdinal(a.Choice, b.Choice);
        });

        int count = Math.Min(numberOfSuggestions, candidates.Count);
        string[] result = new string[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = candidates[i].Choice;
        }

        return result;
    }

    private int ComputeMinimumWindowScore(string term, string choice)
    {
        if (term.Length == 0)
            return 0;

        int minimumScore = int.MaxValue;
        int windowLength = term.Length;
        int lastWindowStart = choice.Length - windowLength;

        for (int start = 0; start <= lastWindowStart; start++)
        {
            string window = choice.Substring(start, windowLength);
            int score = _differenceScorer.GetDifferenceScore(term, window);

            if (score < minimumScore)
            {
                minimumScore = score;
                if (minimumScore == 0)
                    return 0;
            }
        }

        return minimumScore;
    }
}
