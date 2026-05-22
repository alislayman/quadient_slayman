using System;

namespace Suggester;

public sealed class DifferenceScorer : IDifferenceScorer
{
    public int GetDifferenceScore(string destinationText, string sourceText)
    {
        if (destinationText.Length != sourceText.Length) throw new ArgumentException(GlobalConst.StringsMustHaveSameLengthMessage, nameof(sourceText));
        int differingCharacterCount = 0;
        for (int characterIndex = 0; characterIndex < destinationText.Length; characterIndex++)
        {
            if (destinationText[characterIndex] != sourceText[characterIndex]) differingCharacterCount++;
        }
        return differingCharacterCount;
    }
}
