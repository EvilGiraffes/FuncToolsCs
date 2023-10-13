using FluentAssertions.Execution;

namespace FuncToolsCs.Tests.AssertionHelpers;
static class AssertionExt
{
    public static AssertionScope ForCondition(this AssertionScope scope, Func<bool> predicate)
        => scope.ForCondition(predicate());
}
