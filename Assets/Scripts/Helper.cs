using UnityEngine;

public static class Helper
{
    public static T СhoiceArray<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}
