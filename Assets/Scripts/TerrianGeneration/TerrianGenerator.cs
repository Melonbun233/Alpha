using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TerrianGenerator : MonoBehaviour
{
    public enum DrawMode {
        NoiseMap, Terrian
    }

    [Header("Map Display Settings")]
    public DrawMode drawMode;

    [Header("Noise Map Settings")]
    // Map width
    public int width;
    public int height;
    public int seed;
    public float noiseScale;
    public int octaves;

    [Tooltip("How amplitude changes between each octaves")]
    [Range(0, 1)]
    public float persistance;

    [Tooltip("How frequency changes between each octaves")]
    public float lacunarity;
    public Vector2 offset;
    public bool autoUpdate;

    [Header("Mesh Settings")]
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;


    [Header("Display Settings")]
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Renderer miniMapRenderer;

    [Header("Terrian Region Settings")]
    public TerrianRegion region;

    float[,] _noiseMap;
    MeshData2D _meshData;
    Texture2D _texture;

    public void generateMap() {
        _noiseMap = Noise.generateNoiseMap2D(width, height, seed, noiseScale,
            octaves, persistance, lacunarity, offset);
        _meshData = MeshGenerator.generateTerrianMesh(_noiseMap, meshHeightMultiplier, meshHeightCurve);

        drawMap();
        drawMesh();
    }


    // Start is called before the first frame update
    void Start()
    {
        generateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate() {
        if (width < 1) {
            width = 1;
        }

        if (height < 1) {
            height = 1;
        }

        if (lacunarity < 1) {
            lacunarity = 1;
        }

        if (octaves < 0) {
            octaves = 0;
        }
    }

    private void drawMap() {
        if (_noiseMap == null) {
            return;
        }

        _texture = new Texture2D(width, height);

        switch (drawMode) {
            case DrawMode.Terrian:
                Color[] colorMap = new Color[width * height];
                for (int y = 0; y < height; y ++) {
                    for (int x = 0; x < width; x ++) {
                        colorMap[y * width + x] = TerrianRegion.getTerrianTypeColor(
                            _noiseMap[x, y], region);
                    }
                }
                _texture = TextureGenerator.textureFromColorMap(colorMap, width, height);
                break;
            
            case DrawMode.NoiseMap:
                _texture = TextureGenerator.textureFromHeightMap(_noiseMap);
                break;              
        }

        if (miniMapRenderer == null) {
            return;
        }

        miniMapRenderer.sharedMaterial.SetTexture("_UnlitColorMap", _texture);
        miniMapRenderer.transform.localScale = new Vector3(width, height, 1);
    }

    private void drawMesh() {
        if (meshFilter != null && meshRenderer != null) {
            meshFilter.sharedMesh = _meshData.createMesh();
            meshRenderer.sharedMaterial.SetTexture("_BaseColorMap", _texture);
            // meshRenderer.transform.localScale = new Vector3(1, 1, 1);
        }    
    }

    public void clearMesh() {
        meshFilter.sharedMesh = new Mesh();
    }
}

[CustomEditor (typeof (TerrianGenerator))]
public class TerrianGeneratorEditor : Editor
{
    public override void OnInspectorGUI() {
        TerrianGenerator generator = (TerrianGenerator)target;
        
        if (DrawDefaultInspector()) {
            if (generator.autoUpdate) {
                generator.generateMap();
            }
        }

        if (GUILayout.Button ("Clear Mesh")) {
            generator.clearMesh();
        }

        if (GUILayout.Button ("Sort Terrian Types")) {
            TerrianRegion.sortTerrianTypes(generator.region);
        }

        if (GUILayout.Button ("Generate Map")) {
            generator.generateMap();
        }

 
    }
}

