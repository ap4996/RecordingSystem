using System.Collections.Generic;
using UnityEngine;

namespace RS.Extensions
{
    public static class Extensions
    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            if(list == null || list.Count == 0) { Debug.LogError($"List is empty"); return default; }
            return list[Random.Range(0, list.Count)];
        }
    }

}
