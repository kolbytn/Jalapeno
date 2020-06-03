using System;

public class Utils
{
    public static double SampleNormal(double mean, double std)
    {
        double u1 = UnityEngine.Random.value;
        double u2 = UnityEngine.Random.value;
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        double randNormal = mean + std * randStdNormal;
        return randNormal;
    }

    public static T[] Flatten2dArray<T>(T[,] array)
    {
        T[] flattened = new T[array.Length];

        int write = 0;
        for (int i = 0; i <= array.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= array.GetUpperBound(1); j++)
            {
                flattened[write++] = array[i, j];
            }
        }

        return flattened;
    }

    public static T[,] Reshape2dArray<T>(T[] array, int width, int height)
    {
        T[,] reshaped = new T[width, height];

        int read = 0;
        for (int i = 0; i <= reshaped.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= reshaped.GetUpperBound(1); j++)
            {
                reshaped[i, j] = array[read++];
            }
        }

        return reshaped;
    }
}

public struct GridLocation
{    
    public int col;
    public int row;
}
