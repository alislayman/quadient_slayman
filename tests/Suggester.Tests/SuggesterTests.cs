using FluentAssertions;
using Suggester.Interfaces;
using Xunit;

namespace Suggester.Tests;

public class SuggesterTests
{
    [Fact]
    public void Empty_choices_returns_empty_result()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        ISuggester suggester = new Suggester(differenceScorer);
        IEnumerable<string> actualSuggestions = suggester.GetSuggestions("gros", Array.Empty<string>(), 2);
        actualSuggestions.Should().BeEmpty();
    }

    [Fact]
    public void Zero_number_of_suggestions_returns_empty_result()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        ISuggester suggester = new Suggester(differenceScorer);
        string[] availableChoices = new[] { "gros", "gras" };
        IEnumerable<string> actualSuggestions = suggester.GetSuggestions("gros", availableChoices, 0);
        actualSuggestions.Should().BeEmpty();
    }

    [Fact]
    public void Choices_shorter_than_term_are_excluded()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        ISuggester suggester = new Suggester(differenceScorer);
        string[] availableChoices = new[] { "go", "ros", "gro" };
        IEnumerable<string> actualSuggestions = suggester.GetSuggestions("gros", availableChoices, 3);
        actualSuggestions.Should().BeEmpty();
    }

    [Fact]
    public void Exact_match_scores_zero_and_ranks_first()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        ISuggester suggester = new Suggester(differenceScorer);
        string[] availableChoices = new[] { "gras", "gros" };
        IEnumerable<string> actualSuggestions = suggester.GetSuggestions("gros", availableChoices, 1);
        actualSuggestions.Should().ContainSingle().Which.Should().Be("gros");
    }

    [Fact]
    public void Brief_example_returns_gros_and_gras()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        ISuggester suggester = new Suggester(differenceScorer);
        string[] availableChoices = new[] { "gros", "gras", "graisse", "agressif", "go", "ros", "gro" };
        IEnumerable<string> actualSuggestions = suggester.GetSuggestions("gros", availableChoices, 2);
        actualSuggestions.Should().Equal("gros", "gras");
    }

    [Fact]
    public void Sliding_window_finds_match_inside_longer_string()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        ISuggester suggester = new Suggester(differenceScorer);
        string[] availableChoices = new[] { "xxxx", "agressif" };
        IEnumerable<string> actualSuggestions = suggester.GetSuggestions("gres", availableChoices, 1);
        actualSuggestions.Should().ContainSingle().Which.Should().Be("agressif");
    }
}
