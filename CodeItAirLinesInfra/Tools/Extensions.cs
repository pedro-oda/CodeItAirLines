using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CodeItAirLinesInfra
{
    public static class Extensions
    {
        private static Random rng = new Random();

        public static int RandomElement<T>(this IEnumerable<T> list)
        {
            return rng.Next(list.Count());
        }

        public static T ChangeList<T>( this List<T> origem, List<T> destino, int? posicao, bool removeAll = false)
        {
            if (removeAll)
            {
                destino.AddRange(origem);
                origem.Clear();
            }
            else if(posicao.HasValue)
            {
                destino.Add(origem[posicao.Value]);
                var elemento = origem.ElementAt(posicao.Value);
                origem.RemoveAt(posicao.Value);
                return elemento;
            }

            return default(T);
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, bool condition)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static bool AnyOrNotNull<T>(this IEnumerable<T> source)
        {
            if (source != null && source.Any())
                return true;
            else
                return false;
        }
    }
}
