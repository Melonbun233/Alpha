using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Types of terrian on certain height
[System.Serializable]
public struct TerrianType
{
    public string name;
    public float height;
    public Color color;

    public static Color defaultColor = new Color(128, 0, 128, 256);

    public TerrianType(string name, float height, Color color) {
        this.name = name;
        this.height = height;
        this.color = color;
    }
}

[System.Serializable]
public class TerrianRegion {
    public static TerrianRegion island = new TerrianRegion( 
    new TerrianType[] {
        new TerrianType("water", 0.2f, Color.blue),
        new TerrianType("soil", 0.8f, Color.green),
        new TerrianType("snow", 1f, Color.white) 
    });

    public TerrianType[] terrianTypes;
    public TerrianRegion (TerrianType[] types) {
        this.terrianTypes = types;
    }

    public static Color getTerrianTypeColor(float height, TerrianRegion region) {
        TerrianType[] types = region.terrianTypes;
        for (int i = 0; i < types.Length; i ++) {
            if (height <= types[i].height) {
                return types[i].color;
            }
        }

        return TerrianType.defaultColor;
    }

    public static string getTerrianTypeName(float height, TerrianRegion region) {
        TerrianType[] types = region.terrianTypes;
        for (int i = 0; i < types.Length; i ++) {
            if (height <= types[i].height) {
                return types[i].name;
            }
        }

        return "Null";
    }

    public static void sortTerrianTypes(TerrianRegion region) {
        TerrianType[] types = region.terrianTypes;
        List<TerrianType> list = new List<TerrianType>(types);

        list.Sort(delegate(TerrianType x, TerrianType y) {
            return x.height.CompareTo(y.height);
        });

        for (int i = 0; i < list.Count; i ++) {
            types[i] = list[i];
        }
    }

}

