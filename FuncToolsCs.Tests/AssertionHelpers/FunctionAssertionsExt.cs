using FluentAssertions.Specialized;

namespace FuncToolsCs.Tests.AssertionHelpers;
static class FunctionAssertionsExt
{
    public static AndConstraint<FunctionAssertions<T>> HaveValueBe<T>(this FunctionAssertions<T> assertion, T expected, string? because = null, params object?[] becauseArgs)
    {
        T? value = default;
        try
        {
            value = assertion.Subject();
        }
        catch
        {
            return new AndConstraint<FunctionAssertions<T>>(assertion);
        }
        value.Should().Be(expected);
        return new AndConstraint<FunctionAssertions<T>>(assertion);
    }
}
