using FluentAssertions;
using Xunit;

namespace Suggester.Tests;

public class DifferenceScorerTests
{
    [Fact]
    public void Identical_strings_score_zero()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        int actualDifferenceScore = differenceScorer.GetDifferenceScore("abc", "abc");
        actualDifferenceScore.Should().Be(0);
    }

    [Fact]
    public void Completely_different_strings_score_length()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        int actualDifferenceScore = differenceScorer.GetDifferenceScore("abcd", "wxyz");
        actualDifferenceScore.Should().Be(4);
    }
}
