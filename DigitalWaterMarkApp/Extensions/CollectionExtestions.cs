using System;
using System.Collections.Generic;
using System.Linq;

public static class CollectionExtestions {
    public static IEnumerable<T> TakeRandomEfficient<T>(this IEnumerable<T> source, int count) {
        Random random = new Random();
        HashSet<int> selectedIndices = new HashSet<int>();
        int sourceCount = source.Count();
        count = Math.Min(count, sourceCount);

        while (selectedIndices.Count < count) {
            selectedIndices.Add(random.Next(sourceCount));
        }

        return selectedIndices.Select(i => source.ElementAt(i));
    }
}