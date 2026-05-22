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
}
