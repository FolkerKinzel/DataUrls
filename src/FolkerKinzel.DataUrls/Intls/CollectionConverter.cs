using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.DataUrls.Intls;


internal static class CollectionConverter
{
    internal static ReadOnlySpan<T> AsSpan<T>(this IEnumerable<T>? coll)
    {
        if(coll is null)
        {
            return [];
        }

        ReadOnlySpan<T> span = coll switch {
            T[] arr => arr,
#if NET5_0_OR_GREATER
            List<T> list => CollectionsMarshal.AsSpan(list),
#endif
            _ => coll.ToArray(),
        };
        return span;
    }
}