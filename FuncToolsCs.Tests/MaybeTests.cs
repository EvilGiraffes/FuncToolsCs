using FuncToolsCs.Errors;

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
        Func<int, bool> predicate = x => x > 0;
        systemUnderTest
            .Filter(predicate)
            .Should()
            .BeNone();
    }
    [Fact]
    public void Filter_SomePredicateFalse_None()
    {
        Maybe<int> systemUnderTest = Maybe.Some(0);
        Func<int, bool> predicate = x => x > 0;
        systemUnderTest
            .Filter(predicate)
            .Should()
            .BeNone();
    }
    [Fact]
    public void Filter_SomePredicateTrue_SomeT()
    {
        Maybe<int> systemUnderTest = Maybe.Some(1);
        Func<int, bool> predicate = x => x > 0;
        systemUnderTest
            .Filter(predicate)
            .Should()
            .BeSome()
            .And
            .Contain(1);
    }
    [Fact]
    public void Zip_NoneNone_None()
    {
        Maybe<int> left = Maybe.None<int>();
        Maybe<int> right = Maybe.None<int>();
        Maybe<(int, int)> actual = left.Zip(right);
        actual.Should().BeNone();
    }
    [Theory]
    [MemberData(nameof(ZipData))]
    public void Zip_OneNone_None(Maybe<int> left, Maybe<int> right)
    {
        Maybe<(int, int)> actual = left.Zip(right);
        actual.Should().BeNone();
    }
    [Fact]
    public void Zip_SomeSome_SomeZipped()
    {
        Maybe<int> left = Maybe.Some(1);
        Maybe<int> right = Maybe.Some(2);
        Maybe<(int, int)> actual = left.Zip(right);
        actual.Should()
            .BeSome()
            .And
            .Contain((1, 2));
    }
    [Fact]
    public void Zip_SomeSome_DoesNotThrow()
    {
        Maybe<int> left = Maybe.Some(1);
        Maybe<int> right = Maybe.Some(2);
        Action act = () => left.Zip(right);
        act.Should().NotThrow();
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
    public void Expect_None_Throws()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        Action act = () => systemUnderTest.Expect();
        act.Should().Throw<MaybeIsNone>();
    }
    [Fact]
    public void Expect_Some_DoesNotThrow()
    {
        Maybe<int> systemUnderTest = Maybe.Some(1);
        Action act = () => systemUnderTest.Expect();
        act.Should().NotThrow();
    }
    [Fact]
    public void Unwrap_None_GivesNull()
    {
        Maybe<string> systemUnderTest = Maybe.None<string>();
        systemUnderTest
            .Unwrap()
            .Should()
            .BeNull();
    }
    [Fact]
    public void Unwrap_Some_GivesT()
    {
        Maybe<string> systemUnderTest = Maybe.Some("Hello world!");
        systemUnderTest
            .Unwrap()
            .Should()
            .Be("Hello world!");
    }
    [Fact]
    public void TryUnwrap_None_False()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        systemUnderTest
            .TryUnwrap(out _)
            .Should()
            .BeFalse();
    }
    [Fact]
    public void TryUnwrap_None_Null()
    {
        Maybe<string> systemUnderTest = Maybe.None<string>();
        _ = systemUnderTest.TryUnwrap(out string? actual);
        actual.Should().BeNull();
    }
    [Fact]
    public void TryUnwrap_Some_True()
    {
        Maybe<int> systemUnderTest = Maybe.Some(1);
        systemUnderTest
            .TryUnwrap(out _)
            .Should()
            .BeTrue();
    }
    [Fact]
    public void TryUnwrap_Some_Value()
    {
        Maybe<string> systemUnderTest = Maybe.Some("Hello World!");
        _ = systemUnderTest.TryUnwrap(out string? actual);
        actual.Should().Be("Hello World!");
    }
    [Fact]
    public void UnwrapResult_None_CallsFactory()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        systemUnderTest
            .UnwrapResult()
            .Should()
            .Fail();
    }
    [Fact]
    public void UnwrapResult_Some_DoesNotCallFactory()
    {
        Maybe<int> systemUnderTest = Maybe.Some(1);
        systemUnderTest
            .UnwrapResult()
            .Should()
            .NotFail();
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
    [Fact]
    public void Take_None_Unchanged()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        Maybe.Take(ref systemUnderTest);
        systemUnderTest.Should().BeNone();
    }
    [Fact]
    public void Take_None_None()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        Maybe.Take(ref systemUnderTest).Should().BeNone();
    }
    [Fact]
    public void Take_Some_ChangedToNone()
    {
        Maybe<int> systemUnderTest = Maybe.Some(5);
        Maybe.Take(ref systemUnderTest);
        systemUnderTest.Should().BeNone();
    }
    [Fact]
    public void Take_Some_SomeSameValue()
    {
        Maybe<int> systemUnderTest = Maybe.Some(5);
        Maybe<int> actual = Maybe.Take(ref systemUnderTest);
        actual.Should().Contain(5);
    }
    [Fact]
    public void TakeIf_None_Unchanged()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        Maybe<int> actual = Maybe.TakeIf(ref systemUnderTest, _ => true);
        systemUnderTest.Should().BeNone();
    }
    [Fact]
    public void TakeIf_None_None()
    {
        Maybe<int> systemUnderTest = Maybe.None<int>();
        Maybe.TakeIf(ref systemUnderTest, _ => true)
           .Should()
           .BeNone();
    }
    [Fact]
    public void TakeIf_SomePredicateFalse_Unchanged()
    {
        Maybe<int> systemUnderTest = Maybe.Some(5);
        Maybe<int> actual = Maybe.TakeIf(ref systemUnderTest, _ => false);
        systemUnderTest.Should().BeSome();
    }
    [Fact]
    public void TakeIf_SomePredicateTrue_ChangedToNone()
    {
        Maybe<int> systemUnderTest = Maybe.Some(5);
        Maybe<int> actual = Maybe.TakeIf(ref systemUnderTest, _ => true);
        systemUnderTest.Should().BeNone();
    }
    [Fact]
    public void TakeIf_SomePredicateFalse_None()
    {
        Maybe<int> systemUnderTest = Maybe.Some(5);
        Maybe.TakeIf(ref systemUnderTest, _ => false)
            .Should()
            .BeNone();
    }
    [Fact]
    public void TakeIf_SomePredicateTrue_SomeSameValue()
    {
        Maybe<int> systemUnderTest = Maybe.Some(5);
        Maybe.TakeIf(ref systemUnderTest, _ => true)
            .Should()
            .BeSome()
            .And
            .Contain(5);
    }
    public static IEnumerable<object[]> ZipData()
    {

        yield return new object[] { Maybe.None<int>(), Maybe.Some(1) };
        yield return new object[] { Maybe.Some(1), Maybe.None<int>() };
    }
}
