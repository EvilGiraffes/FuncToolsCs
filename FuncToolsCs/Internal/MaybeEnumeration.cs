namespace FuncToolsCs.Internal;

static class MaybeEnumeration
{
    // TESTME: 
    public static IEnumerable<TReturnItem> Iterate<TItem, TReturnItem>(IEnumerable<TItem> enumerable, Func<TItem, Maybe<TReturnItem>> map)
        where TReturnItem : notnull
    {
        foreach (TItem item in enumerable)
        {
            Maybe<TReturnItem> result = map(item);
            if (!result.TryUnwrap(out TReturnItem? returnItem))
                yield break;
            yield return returnItem;
        }
    }
    // TESTME: Ensure the index actually increments.
    public static IEnumerable<TReturnItem> Iterate<TItem, TReturnItem>(IEnumerable<TItem> enumerable, Func<TItem, int, Maybe<TReturnItem>> map)
        where TReturnItem : notnull
    {
        int index = 0;
        return Iterate(enumerable, item => map(item, index++));
    }
}