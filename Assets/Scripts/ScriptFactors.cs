using System;
using System.Collections.Generic;
using UnityEngine;

public class ScriptFactors : MonoBehaviour
{
    void Awake()
    {
        List<FactorResult> f = FindFuzzyCommonFactors(640, 720, 960, 1080, 50);

        foreach (FactorResult factor in f)
        {
            if (factor.Factor > 359 || factor.Factor < 250 || factor.Factor % (9*2) != 0)
                continue;
            
            string s = $"{factor.Factor}: ";
            
            foreach ((int x, int y) pair in factor.ExamplePairs)
                s += $"{pair.x}, {pair.y}; ";

            print (s);
        }
    }
    public class FactorResult
    {
        public int Factor;
        public List<(int x, int y)> ExamplePairs = new List<(int x, int y)>();
    }

    /// <summary>
    /// Find fuzzy common factors between ranges [aMin,aMax] and [bMin,bMax].
    /// Returns a list of FactorResult where each FactorResult.Factor is a factor
    /// that can divide some x in the first range and some y in the second range.
    /// ExamplePairs contains up to maxPairsPerFactor concrete (x,y) pairs that realize the factor.
    /// </summary>
    public static List<FactorResult> FindFuzzyCommonFactors(
        int aMin, int aMax, int bMin, int bMax,
        int maxPairsPerFactor = 10)
    {
        if (aMin > aMax) throw new ArgumentException("aMin > aMax");
        if (bMin > bMax) throw new ArgumentException("bMin > bMax");
        var results = new List<FactorResult>();
        int maxF = Math.Min(aMax, bMax);

        for (int f = 1; f <= maxF; f++)
        {
            // first multiple of f in range A
            int firstA = ((aMin + f - 1) / f) * f;
            if (firstA > aMax) continue;

            // first multiple of f in range B
            int firstB = ((bMin + f - 1) / f) * f;
            if (firstB > bMax) continue;

            var res = new FactorResult { Factor = f };

            // enumerate pairs but cap to maxPairsPerFactor to avoid explosion
            int count = 0;
            for (int x = firstA; x <= aMax && count < maxPairsPerFactor; x += f)
            {
                for (int y = firstB; y <= bMax && count < maxPairsPerFactor; y += f)
                {
                    res.ExamplePairs.Add((x, y));
                    count++;
                }
            }

            results.Add(res);
        }

        return results;
    }
}
