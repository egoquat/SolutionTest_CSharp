using System;
using System.Collections.Generic;
using System.Linq;

public static class Common
{
    public static bool AddNotOverlap<T>(this IList<T> container, T t)
    {
        if (false == container.Contains(t))
        {
            container.Add(t);
            return true;
        }
        return false;
    }
    public static T GetOrDefault<T>(this IList<T> container, int idx)
    {
        return container.ElementAtOrDefault<T>(idx);
    }
    public static string ToString<T>(this List<T> list)
    {
        string output = "list((" + list.Count + ")";
        for (int i = 0; i < list.Count; ++i)
        {
            output += " " + list[i].ToString();
        }
        output += ")";
        return output;
    }
}
