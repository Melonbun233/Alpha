using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Types of terrain on certain height
[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;

    public static Color defaultColor = new Color(128, 0, 128, 256);

    public TerrainType(string name, float height, Color color) {
        this.name = name;
        this.height = height;
        this.color = color;
    }
}

[System.Serializable]
public class TerrainRegion {
    public static TerrainRegion island = new TerrainRegion( 
    new TerrainType[] {
        new TerrainType("water", 0.2f, Color.blue),
        new TerrainType("soil", 0.8f, Color.green),
        new TerrainType("snow", 1f, Color.white) 
    });

    public TerrainType[] terrainTypes;
    public TerrainRegion (TerrainType[] types) {
        this.terrainTypes = types;
    }

    public static Color getTerrainTypeColor(float height, TerrainRegion region) {
        TerrainType[] types = region.terrainTypes;
        for (int i = 0; i < types.Length; i ++) {
            if (height <= types[i].height) {
                return types[i].color;
            }
        }

        return TerrainType.defaultColor;
    }

    public static string getTerrainTypeName(float height, TerrainRegion region) {
        TerrainType[] types = region.terrainTypes;
        for (int i = 0; i < types.Length; i ++) {
            if (height <= types[i].height) {
                return types[i].name;
            }
        }

        return "Null";
    }

    public static void sortTerrainTypes(TerrainRegion region) {
        TerrainType[] types = region.terrainTypes;
        List<TerrainType> list = new List<TerrainType>(types);

        list.Sort(delegate(TerrainType x, TerrainType y) {
            return x.height.CompareTo(y.height);
        });

        for (int i = 0; i < list.Count; i ++) {
            types[i] = list[i];
        }
    }

}

