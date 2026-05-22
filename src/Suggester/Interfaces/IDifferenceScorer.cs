namespace Suggester.Interfaces;

public interface IDifferenceScorer
{
    int GetDifferenceScore(string destinationText, string sourceText);
}
