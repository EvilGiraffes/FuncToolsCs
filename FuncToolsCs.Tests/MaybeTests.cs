namespace FuncToolsCs.Tests;
public class MaybeTests
{
    [Fact]
    public void Map_None_DoesNotGetCalled()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        Func<int, string> mapperMock = Substitute.For<Func<int, string>>();
        systemUnderTest.Map(mapperMock);
        mapperMock.DidNotReceive().Invoke(Arg.Any<int>());
    }
    [Fact]
    public void Map_Some_DidGetCalled()
    {
        Maybe<int> systemUnderTest = Maybe.Some(1);
        Func<int, string> mapperMock = Substitute.For<Func<int, string>>();
        systemUnderTest.Map(mapperMock);
        mapperMock.Received(1).Invoke(1);
    }
    [Fact]
    public void Bind_None_DoesNotGetCalled()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        Func<int, Maybe<string>> binderMock = Substitute.For<Func<int, Maybe<string>>>();
        systemUnderTest.Bind(binderMock);
        binderMock.DidNotReceive().Invoke(Arg.Any<int>());
    }
    [Fact]
    public void Bind_Some_DidGetCalled()
    {
        Maybe<int> systemUnderTest = Maybe.Some(1);
        Func<int, Maybe<string>> binderMock = Substitute.For<Func<int, Maybe<string>>>();
        systemUnderTest.Bind(binderMock);
        binderMock.Received(1).Invoke(1);
    }
    [Fact]
    public void Filter_None_None()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        Predicate<int> predicate = x => x > 0;
        systemUnderTest
            .Filter(predicate)
            .IsNone
            .Should()
            .BeTrue();
    }
    [Fact]
    public void Filter_SomePredicateFalse_None()
    {
        Maybe<int> systemUnderTest = Maybe.Some(0);
        Predicate<int> predicate = x => x > 0;
        systemUnderTest
            .Filter(predicate)
            .IsNone
            .Should()
            .BeTrue();
    }
    [Fact]
    public void Filter_SomePredicateTrue_SomeT()
    {
        Maybe<int> systemUnderTest = Maybe.Some(1);
        Predicate<int> predicate = x => x > 0;
        systemUnderTest
            .Filter(predicate)
            .IsSome
            .Should()
            .BeTrue();

    }
    [Fact]
    public void MatchT_None_Default()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        Func<int, string> someMock = Substitute.For<Func<int, string>>();
        systemUnderTest
            .Match(someMock, "None")
            .Should()
            .Be("None");
    }
    [Fact]
    public void MatchT_Some_CallsSome()
    {
        Maybe<int> systemUnderTest = Maybe.Some(1);
        Func<int, string> someMock = Substitute.For<Func<int, string>>();
        systemUnderTest
            .Match(someMock, "None")
            .Should()
            .NotBe("None");
        someMock.Received(1).Invoke(1);
    }
    [Fact]
    public void MatchFuncT_None_CallsNone()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        Func<int, string> someMock = Substitute.For<Func<int, string>>();
        Func<string> noneMock = Substitute.For<Func<string>>();
        systemUnderTest.Match(someMock, noneMock);
        someMock.DidNotReceive().Invoke(Arg.Any<int>());
        noneMock.Received(1).Invoke();
    }
    [Fact]
    public void MatchFuncT_Some_CallsSome()
    {
        Maybe<int> systemUnderTest = Maybe.Some(1);
        Func<int, string> someMock = Substitute.For<Func<int, string>>();
        Func<string> noneMock = Substitute.For<Func<string>>();
        systemUnderTest.Match(someMock, noneMock);
        someMock.Received(1).Invoke(1);
        noneMock.DidNotReceive().Invoke();
    }
    [Fact]
    public void Unwrap_None_GivesNull()
    {
        Maybe<string> systemUnderTest = Maybe.None<string>();
        systemUnderTest.Unwrap().Should().BeNull();
    }
    [Fact]
    public void Unwrap_Some_GivesT()
    {
        Maybe<string> systemUnderTest = Maybe.Some("Hello world!");
        systemUnderTest.Unwrap().Should().Be("Hello world!");
    }
    [Fact]
    public void UnwrapResult_None_CallsFactory()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        systemUnderTest
            .UnwrapResult(() => new Exception())
            .IsError
            .Should()
            .BeTrue();
    }
    [Fact]
    public void UnwrapResult_Some_DoesNotCallFactory()
    {
        Maybe<int> systemUnderTest = Maybe.Some(1);
        systemUnderTest
            .UnwrapResult(() => new Exception())
            .IsOk
            .Should()
            .BeTrue();
    }
    [Fact]
    public void UnwrapOrThrow_None_Throws()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        Action act = () => systemUnderTest.UnwrapOrThrow(() => new Exception());
        act.Should().Throw<Exception>();
    }
    [Fact]
    public void UnwrapOrThrow_Some_DoesNotThrow()
    {
        Maybe<int> systemUnderTest = Maybe.Some(1);
        Action act = () => systemUnderTest.UnwrapOrThrow(() => new Exception());
        act.Should().NotThrow();
    }
    [Fact]
    public void UnwrapOrT_None_Default()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        systemUnderTest
            .UnwrapOr(5)
            .Should()
            .Be(5);
    }
    [Fact]
    public void UnwrapOrT_Some_Value()
    {
        Maybe<int> systemUnderTest = Maybe.Some(1);
        systemUnderTest
            .UnwrapOr(5)
            .Should()
            .Be(1);
    }
    [Fact]
    public void UnwrapOrFuncT_None_Default()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        systemUnderTest
            .UnwrapOr(() => 5)
            .Should()
            .Be(5);
    }
    [Fact]
    public void UnwrapOrFuncT_Some_Value()
    {
        Maybe<int> systemUnderTest = Maybe.Some(1);
        systemUnderTest
            .UnwrapOr(() => 5)
            .Should()
            .Be(1);
    }
}
