using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection of generic helper functions.
/// </summary>
public class Utilities
{
    public static A? MinBy<A, B>(IEnumerable<A> enumerable, System.Func<A, B> selector) where A : struct
    {
        return MinBy(enumerable, selector, Comparer<B>.Default);
    }

    public static A? MinBy<A, B>(IEnumerable<A> enumerable, System.Func<A, B> selector, IComparer<B> comparer) where A : struct
    {
        using (IEnumerator<A> enumerator = enumerable.GetEnumerator())
        {
            if (!enumerator.MoveNext())
            {
                return null;
            }

            (A, B) minimum = (enumerator.Current, selector(enumerator.Current));

            while (enumerator.MoveNext())
            {
                (A, B) current = (enumerator.Current, selector(enumerator.Current));

                if (comparer.Compare(current.Item2, minimum.Item2) < 0)
                {
                    minimum = current;
                }
            }

            return minimum.Item1;
        }
    }

    public static IEnumerable<(A, A)> Tuple<A>(IEnumerable<A> enumerable)
    {
        using (IEnumerator<A> enumerator = enumerable.GetEnumerator())
        {
            if (!enumerator.MoveNext())
            {
                yield break;
            }

            A i = enumerator.Current;

            while (enumerator.MoveNext())
            {
                yield return (i, i = enumerator.Current);
            }
        }
    }

    public static T[] Take<T>(IEnumerator<T> enumerator, int n)
    {
        List<T> values = new List<T>();
        
        while (values.Count < n && enumerator.MoveNext())
        {
            values.Add(enumerator.Current);
        }

        return values.ToArray();
    }

    public static int RandomSign()
    {
        return Random.Range(0, 2) == 0 ? -1 : 1;
    }
}
