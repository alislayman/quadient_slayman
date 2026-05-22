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

    [Fact]
    public void Tie_on_score_closer_length_wins()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        ISuggester suggester = new Suggester(differenceScorer);
        string[] availableChoices = new[] { "agressif", "gras" };
        IEnumerable<string> actualSuggestions = suggester.GetSuggestions("gros", availableChoices, 1);
        actualSuggestions.Should().ContainSingle().Which.Should().Be("gras");
    }

    [Fact]
    public void Tie_on_score_and_length_alphabetical_wins()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        ISuggester suggester = new Suggester(differenceScorer);
        string[] availableChoices = new[] { "zz", "yy" };
        IEnumerable<string> actualSuggestions = suggester.GetSuggestions("ab", availableChoices, 1);
        actualSuggestions.Should().ContainSingle().Which.Should().Be("yy");
    }

    [Fact]
    public void Number_of_suggestions_greater_than_matches_returns_all_matches()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        ISuggester suggester = new Suggester(differenceScorer);
        string[] availableChoices = new[] { "gros", "gras" };
        IEnumerable<string> actualSuggestions = suggester.GetSuggestions("gros", availableChoices, 99);
        actualSuggestions.Should().HaveCount(2);
    }

    [Fact]
    public void Duplicate_entries_in_choices_are_preserved()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        ISuggester suggester = new Suggester(differenceScorer);
        string[] availableChoices = new[] { "gros", "gros", "gras" };
        IEnumerable<string> actualSuggestions = suggester.GetSuggestions("gros", availableChoices, 2);
        actualSuggestions.Should().Equal("gros", "gros");
    }

    [Fact]
    public void Empty_term_scores_zero_for_all_then_orders_by_length_then_alphabetical()
    {
        IDifferenceScorer differenceScorer = new DifferenceScorer();
        ISuggester suggester = new Suggester(differenceScorer);
        string[] availableChoices = new[] { "bb", "aa", "ccc" };
        IEnumerable<string> actualSuggestions = suggester.GetSuggestions(string.Empty, availableChoices, 3);
        actualSuggestions.Should().Equal("aa", "bb", "ccc");
    }
}
