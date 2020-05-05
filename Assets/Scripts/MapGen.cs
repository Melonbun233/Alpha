using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{

    public int rows;
    public int columns;
    public GameObject Base;
    public GameObject[] midTiles;
    public GameObject[] sideTiles;
    public GameObject[] enemySpawn;

    private Transform mapHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();

        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                gridPositions.Add(new Vector3(c, r, 0f));
            }
        }
    }

    void mapSetup()
    {
        mapHolder = new GameObject("Map").transform;

        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < columns; r++)
            {
                GameObject toInstantiate = midTiles[Random.Range(0, midTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(c, r, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(mapHolder);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        mapSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
