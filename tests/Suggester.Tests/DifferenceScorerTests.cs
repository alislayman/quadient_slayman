using System;
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

    [Fact]
    public void Gros_versus_gras_scores_one()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        int actualDifferenceScore = differenceScorer.GetDifferenceScore("gros", "gras");
        actualDifferenceScore.Should().Be(1);
    }

    [Fact]
    public void Grai_versus_gros_scores_two()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        int actualDifferenceScore = differenceScorer.GetDifferenceScore("grai", "gros");
        actualDifferenceScore.Should().Be(2);
    }

    [Fact]
    public void Empty_strings_score_zero()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        int actualDifferenceScore = differenceScorer.GetDifferenceScore(string.Empty, string.Empty);
        actualDifferenceScore.Should().Be(0);
    }

    [Fact]
    public void Mismatched_lengths_throw_argument_exception()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        Action invocationWithMismatchedLengths = () => differenceScorer.GetDifferenceScore("abc", "abcd");
        invocationWithMismatchedLengths.Should().Throw<ArgumentException>();
    }
}
