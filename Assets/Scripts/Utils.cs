using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static double SampleNormal(double mean, double std)
    {
        double u1 = UnityEngine.Random.value;
        double u2 = UnityEngine.Random.value;
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        double randNormal = mean + std * randStdNormal;
        return randNormal;
    }
}
