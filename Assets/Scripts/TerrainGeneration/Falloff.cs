using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Falloff
{
    public static float[,] generateFalloffMap2D(int width, int height, float a, float b) {
        float[,] falloffMap = new float[width, height];

        for (int x = 0; x < width; x ++) {
            float xValue = x / (float)width * 2 - 1;
            for (int y = 0; y < height; y ++) {
                float yValue = y / (float)height * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(xValue), Mathf.Abs(yValue));
                falloffMap[x, y] = evaluate(value, a, b);
            }
        }

        return falloffMap;
    }

    private static float evaluate(float value, float a, float b) {
        if (a == 0f) {
            return 0.5f;
        }

        if (b == 0f) {
            return 1f;
        }
        
        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b*value, a));
    }

}
