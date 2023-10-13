using System;

namespace FuncToolsCs.Errors;
public class UnReachable : Exception
{
    public UnReachable()
    {
    }

    public UnReachable(string message) : base(message)
    {
    }

    public UnReachable(string message, Exception inner) : base(message, inner)
    {
    }
}
