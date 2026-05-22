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
- `src/Suggester/`        — algorithm library
- `src/Suggester.App/`    — console demo
- `tests/Suggester.Tests/` — unit tests (xUnit, FluentAssertions)

## Design notes
*(filled in as work progresses)*
