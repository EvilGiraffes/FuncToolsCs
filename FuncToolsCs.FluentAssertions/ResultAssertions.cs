using FluentAssertions;
using FluentAssertions.Execution;

namespace FuncToolsCs.FluentAssertions;
public sealed class ResultAssertions<T, TError>
    where TError : Exception
{
    public Result<T, TError> Subject { get; }
    AndConstraint<ResultAssertions<T, TError>> AndConstraint
        => new(this);
    public ResultAssertions(Result<T, TError> subject)
    {
        Subject = subject;
    }
    public AndConstraint<ResultAssertions<T, TError>> NotFail(string? because = null, params object?[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsOk)
            .FailWith("Expected result not to be an error{reason}.");
        return AndConstraint;
    }
    public AndConstraint<ResultAssertions<T, TError>> Fail(string? because = null, params object?[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsError)
            .FailWith("Expected result to be an error{reason}.");
        return AndConstraint;
    }
    public AndConstraint<ResultAssertions<T, TError>> Contain(T value, string? because = null, params object?[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.Match(
                inner => inner?.Equals(value) ?? false,
                _ => false))
            .FailWith("Expected result to match {0}{reason}.", value);
        return AndConstraint;
    }
    public AndConstraint<ResultAssertions<T, TError>> ContainError(TError error, string? because = null, params object?[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.Match(
                _ => false,
                inner => inner.Equals(error)));
        return AndConstraint;
    }
}

public static class ResultAssertionAttachment
{
    public static ResultAssertions<T, TError> Should<T, TError>(this Result<T, TError> result)
        where TError : Exception
        => new(result);
}
