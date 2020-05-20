using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData2D generateTerrainMesh(float[,] heightMap, float heightMultiplier, 
        AnimationCurve heightCurve) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData2D meshData = new MeshData2D(width, height);
        int vertexIndex = 0;

        for (int y = 0; y < height; y ++) {
            for (int x = 0; x < width; x ++) {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, 
                    heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);

                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);
                
                if (x < width - 1 && y < height - 1) {
                    meshData.addTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.addTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }
                
                vertexIndex ++;
            }
        }

        return meshData;
    }

    
}

public class MeshData2D {
    public Vector3[] vertices;
    public Vector2[] uvs;
    public int[] triangles;
    // index of current triangle cursor
    private int cursor;

    public MeshData2D(int meshWidth, int meshHeight) {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void addTriangle(int a, int b, int c) {
        triangles[cursor] = a;
        triangles[cursor + 1] = b;
        triangles[cursor + 2] = c; 
        cursor += 3;
    }

    public Mesh createMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

}
