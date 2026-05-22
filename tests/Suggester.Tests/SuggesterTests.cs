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
}
