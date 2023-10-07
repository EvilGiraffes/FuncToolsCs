namespace FuncToolsCs.Tests;

public class ResultTests
{
    [Fact]
    public void Map_Error_DoesNotGetCalled()
    {
        Result<int, Exception> systemUnderTest = new Exception();
        Func<int, string> mapperMock = Substitute.For<Func<int, string>>();
        systemUnderTest.Map(mapperMock);
        mapperMock.DidNotReceive().Invoke(Arg.Any<int>());
    }
    [Fact]
    public void Map_NoError_DidGetCalled()
    {
        Result<int, Exception> systemUnderTest = 1;
        Func<int, string> mapperMock = Substitute.For<Func<int, string>>();
        systemUnderTest.Map(mapperMock);
        mapperMock.Received(1).Invoke(1);
    }
    [Fact]
    public void MapError_Error_DidGetCalled()
    {
        Result<int, IndexOutOfRangeException> systemUnderTest = new IndexOutOfRangeException();
        Func<IndexOutOfRangeException, InvalidOperationException> mapperMock = Substitute.For<Func<IndexOutOfRangeException, InvalidOperationException>>();
        mapperMock.Invoke(Arg.Any<IndexOutOfRangeException>()).Returns(new InvalidOperationException());
        systemUnderTest.MapError(mapperMock);
        mapperMock.Received(1).Invoke(Arg.Any<IndexOutOfRangeException>());
    }
    [Fact]
    public void MapError_NoError_DoesNotGetCalled()
    {
        Result<int, IndexOutOfRangeException> systemUnderTest = 1;
        Func<IndexOutOfRangeException, InvalidOperationException> mapperMock = Substitute.For<Func<IndexOutOfRangeException, InvalidOperationException>>();
        mapperMock.Invoke(Arg.Any<IndexOutOfRangeException>()).Returns(new InvalidOperationException());
        systemUnderTest.MapError(mapperMock);
        mapperMock.DidNotReceive().Invoke(Arg.Any<IndexOutOfRangeException>());
    }
    [Fact]
    public void Match_Error_OnErrorCalled()
    {
        Result<int, Exception> systemUnderTest = new Exception();
        Func<int, string> onSuccessMock = Substitute.For<Func<int, string>>();
        Func<Exception, string> onErrorMock = Substitute.For<Func<Exception, string>>();
        systemUnderTest.Match(onSuccessMock, onErrorMock);
        onSuccessMock.DidNotReceive().Invoke(Arg.Any<int>());
        onErrorMock.Received(1).Invoke(Arg.Any<Exception>());
    }
    [Fact]
    public void Match_NoError_OnOkCalled()
    {
        Result<int, Exception> systemUnderTest = 1;
        Func<int, string> onOkMock = Substitute.For<Func<int, string>>();
        Func<Exception, string> onErrorMock = Substitute.For<Func<Exception, string>>();
        systemUnderTest.Match(onOkMock, onErrorMock);
        onErrorMock.DidNotReceive().Invoke(Arg.Any<Exception>());
        onOkMock.Received(1).Invoke(Arg.Any<int>());
    }
    [Fact]
    public void Bind_Error_DoesNotGetCalled()
    {
        Result<int, Exception> systemUnderTest = new Exception();
        Func<int, Result<int, Exception>> bindMock = Substitute.For<Func<int, Result<int, Exception>>>();
        systemUnderTest.Bind(bindMock);
        bindMock.DidNotReceive().Invoke(Arg.Any<int>());
    }
    [Fact]
    public void Bind_NoError_DidGetCalled()
    {
        Result<int, Exception> systemUnderTest = 1;
        Func<int, Result<int, Exception>> bindMock = Substitute.For<Func<int, Result<int, Exception>>>();
        systemUnderTest.Bind(bindMock);
        bindMock.Received(1).Invoke(Arg.Any<int>());
    }
    [Fact]
    public void BindOrMapError_Error_OnErrorCalled()
    {
        Result<int, IndexOutOfRangeException> systemUnderTest = new IndexOutOfRangeException();
        Func<int, Result<int, InvalidOperationException>> bindMock = Substitute.For<Func<int, Result<int, InvalidOperationException>>>();
        Func<IndexOutOfRangeException, InvalidOperationException> onErrorMock = Substitute.For<Func<IndexOutOfRangeException, InvalidOperationException>>();
        systemUnderTest.BindOrMapError(bindMock, onErrorMock);
        bindMock.DidNotReceive().Invoke(Arg.Any<int>());
        onErrorMock.Received(1).Invoke(Arg.Any<IndexOutOfRangeException>());
    }
    [Fact]
    public void BindOrMapError_NoError_BinderCalled()
    {
        Result<int, IndexOutOfRangeException> systemUnderTest = 1;
        Func<int, Result<int, InvalidOperationException>> bindMock = Substitute.For<Func<int, Result<int, InvalidOperationException>>>();
        Func<IndexOutOfRangeException, InvalidOperationException> onErrorMock = Substitute.For<Func<IndexOutOfRangeException, InvalidOperationException>>();
        systemUnderTest.BindOrMapError(bindMock, onErrorMock);
        bindMock.Received(1).Invoke(Arg.Any<int>());
        onErrorMock.DidNotReceive().Invoke(Arg.Any<IndexOutOfRangeException>());
    }
    [Fact]
    public void Unwrap_Error_ThrowsError()
    {
        Result<int, Exception> systemUnderTest = new Exception();
        Action act = () => systemUnderTest.Unwrap();
        act.Should().Throw<Exception>();
    }
    [Fact]
    public void Unwrap_NoError_ReturnsTAndDoesNotThrow()
    {
        Result<int, Exception> systemUnderTest = 1;
        Func<int> act = systemUnderTest.Unwrap;
        act.Should().HaveValueBe(1).And.NotThrow();
    }
    [Fact]
    public void UnwrapOrThrow_Error_ThrowsMappedException()
    {
        Result<int, Exception> systemUnderTest = new Exception();
        Action act = () => systemUnderTest.UnwrapOrThrow(_ => new InvalidOperationException());
        act.Should().ThrowExactly<InvalidOperationException>();
    }
    [Fact]
    public void UnwrapOrThrow_NoError_ReturnsTAndDoesNotThrow()
    {
        Result<int, Exception> systemUnderTest = 1;
        Func<int> act = () => systemUnderTest.UnwrapOrThrow(_ => new InvalidOperationException());
        act.Should().HaveValueBe(1).And.NotThrow();
    }
    [Fact]
    public void UnwrapOrT_Error_ReturnsDefaultAndDoesNotThrow()
    {
        Result<int, Exception> systemUnderTest = new Exception();
        Func<int> act = () => systemUnderTest.UnwrapOr(5);
        act.Should().HaveValueBe(5).And.NotThrow();
    }
    [Fact]
    public void UnwrapOrT_NoError_ReturnsTAndDoesNotThrow()
    {
        Result<int, Exception> systemUnderTest = 1;
        Func<int> act = () => systemUnderTest.UnwrapOr(5);
        act.Should().HaveValueBe(1).And.NotThrow();
    }
    [Fact]
    public void UnwrapOrFuncT_Error_ReturnsDefaultAndDoesNotThrow()
    {
        Result<int, Exception> systemUnderTest = new Exception();
        Func<int> act = () => systemUnderTest.UnwrapOr(() => 5);
        act.Should().HaveValueBe(5).And.NotThrow();
    }
    [Fact]
    public void UnwrapOrFuncT_NoError_ReturnsT()
    {
        Result<int, Exception> systemUnderTest = 1;
        Func<int> act = () => systemUnderTest.UnwrapOr(() => 5);
        act.Should().HaveValueBe(1).And.NotThrow();

    }
}