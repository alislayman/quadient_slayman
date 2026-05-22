# quadient_slayman

Solution to the **Suggestions de terme** technical test.

## Problem
Given a `term`, a list of lowercase alphanumeric `choices`, and an integer `N`,
return the `N` choices most similar to `term`. Similarity is the minimum number
of character substitutions (no insertions) needed to find `term` inside the
choice via a sliding window. Choices shorter than `term` are excluded. Ties
broken by length proximity, then alphabetically.

## Build & test

    dotnet build
    dotnet test

## Run the demo

    dotnet run --project src/Suggester.App

## Project layout
- `src/Suggester/`         — algorithm library
- `src/Suggester/Interfaces/` — public contracts (`IAmTheTest`, `ISuggester`, `IDifferenceScorer`)
- `src/Suggester.App/`     — console demo
- `tests/Suggester.Tests/` — unit tests (xUnit, FluentAssertions)

## Design notes

### Separation of concerns
The algorithm is split along the seam the brief itself hinted at. `DifferenceScorer`
answers a single, narrow question: "given two equal-length strings, how many
positions differ?" `Suggester` knows nothing about character comparison — it asks
`IDifferenceScorer` once per sliding window, takes the minimum, then ranks.
Each side is independently testable and replaceable.

### Why sliding window (not Levenshtein)
The brief explicitly excludes insertions and deletions: the only allowed edit is
substitution. That collapses the problem to "find the best alignment of `term`
inside `choice`," which a sliding window of size `term.Length` solves exactly.
Pulling in Levenshtein here would over-model the problem and break the spec
(it would accept shifted alignments the brief forbids).

### Ranking rules
Sort keys, applied in order:
1. minimum window score (lower is better)
2. absolute length difference `|choice.Length − term.Length|` (closer is better)
3. ordinal alphabetical order (stable, locale-independent)

LINQ's `OrderBy`/`ThenBy` is stable, so duplicates and equal-rank entries keep
their original relative order — matching the brief's "no dedupe" expectation.

### Edge cases handled
- Empty `choices` → empty result
- `numberOfSuggestions ≤ 0` → empty result
- Choices shorter than `term` → filtered out
- `numberOfSuggestions` larger than the candidate set → returns the whole set
  (`Take(N)` doesn't pad)
- Duplicates in `choices` → preserved in output
- Empty `term` → every choice scores 0; tie-broken by length (shorter first),
  then alphabetical

### Complexity
For each choice of length `L_c` and term length `L_t`, the inner loop performs
`(L_c − L_t + 1) × L_t` character comparisons. Across `C` choices the total is
`O(C × (L_c − L_t + 1) × L_t)`. The final sort is `O(C log C)`. Memory is
`O(C)` for the ranked projection — no persistent windows are stored.

### SOLID and DI
`Suggester` depends on `IDifferenceScorer`, not the concrete class. This keeps
the algorithm composable (you can swap in a different scoring strategy without
touching `Suggester`), keeps each unit test focused, and satisfies the
Dependency Inversion Principle. `ISuggester` extends `IAmTheTest` — the brief's
literal interface name is preserved verbatim while client code can use the
cleaner alias.
