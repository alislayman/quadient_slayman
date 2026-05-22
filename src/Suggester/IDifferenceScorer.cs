namespace Suggester;

public interface IDifferenceScorer
{
    int GetDifferenceScore(string destinationText, string sourceText);
}
