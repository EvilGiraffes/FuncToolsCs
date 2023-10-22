namespace FuncToolsCs.Tests;
public class MaybeExtTests
{
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
    public static IEnumerable<object[]> ZipData()
    {
        yield return new object[] { Maybe.None<int>(), Maybe.Some(1) };
        yield return new object[] { Maybe.Some(1), Maybe.None<int>() };
    }
}
