using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Function to generate noise
public static class Noise
{
    // octaves is the number of samples added
    // persistance is how amplitude change between each octave
    // lacunarity is how frequency change between each octave
    public static float[,] generateNoiseMap2D (int mapWidth, int mapHeight, int seed, float scale, 
        int octaves, float persistance, float lacunarity, Vector2 offset) {

        System.Random prng = new System.Random(seed);

        // Set the sample location for each octave
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i ++) {
            float offsetX = prng.Next (-10000, 10000) + offset.x;
            float offsetY = prng.Next (-10000, 10000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        float[,] noiseMap = new float[mapWidth, mapHeight];
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        if (scale <= 0) {
            scale = 0.0001f;
        }

        float halfWidth = mapWidth/2;
        float halfHeight = mapHeight/2;

        for (int x = 0; x < mapWidth; x ++) {
            for (int y = 0; y < mapHeight; y ++) {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i ++) {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float noise = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += noise * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap [x, y] = noiseHeight;
            }
        }

        // normalize noise map
        for (int x = 0; x < mapWidth; x ++) {
            for (int y = 0; y < mapHeight; y ++) {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
