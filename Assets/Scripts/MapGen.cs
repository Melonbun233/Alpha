using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    //public parameters for UI
    public int rows;
    public int columns;
    public GameObject[] Base;
    public GameObject[] midTiles;
    public GameObject[] sideTiles;
    public GameObject[] enemySpawn;

    //An empty to hold all maptiles
    private Transform mapHolder;

    private List<Vector3> gridPositions = new List<Vector3>();

    //Mark base location for setting up map reasonablly.
    private Vector3 _baseloc;
    private int baselocMark;


    void InitialiseList()
    {
        gridPositions.Clear();
        int row = rows * 10;
        int colum = columns * 10;

        for (int c = 0; c < colum; c=c+10)
        {
            for (int r = 0; r < row; r=r+10)
            {
                gridPositions.Add(new Vector3(c, 0f, r));
            }
        }
    }

    //convert unhuman quaternion to euler rotation.
    Quaternion ranRotation() {
        Quaternion temp = new Quaternion(0, 0, 0, 0);
        int temp2 = Random.Range(0, 4);
        temp = Quaternion.Euler(0, 90 * temp2, 0);
        return temp;
    }

    //Mark base location for setting up map reasonablly.
    int markBase() {
        int tempC = Random.Range(0, columns);
        int tempR = Random.Range(0, rows);
        _baseloc = new Vector3(tempC * 10, 0f, tempR * 10);
        return tempC * tempR;
    }

    //set up base tiles.
    void baseSetup() {
        
        int row = rows * 10;
        int colum = columns * 10;
        baselocMark = markBase();

        for (int c = 0; c < colum; c = c + 10)
        {
            for (int r = 0; r < row; r = r + 10)
            {
                if (new Vector3(c,0f,r).Equals(_baseloc))
                {
                    GameObject toInstantiate = Base[Random.Range(0, Base.Length)];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(c, 0f, r), ranRotation()) as GameObject;
                    break;
                }
            }
        }
    }

    //set up enemy spawn tiles.
    void spawnSetup() { 
    
    
    
    
    
    }


    //fill out the rest of the map tiles.
    void mapSetup()
    {
        mapHolder = new GameObject("Map").transform;
        int row = rows * 10;
        int colum = columns * 10;

        for (int c = 0; c < colum; c = c+10)
        {
            for (int r = 0; r < row; r = r+10)
            {
                Vector3 loc = new Vector3(c, 0f, r);
                if (!loc.Equals(_baseloc))
                {
                    GameObject toInstantiate = midTiles[Random.Range(0, midTiles.Length)];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(c, 0f, r), ranRotation()) as GameObject;
                    instance.transform.SetParent(mapHolder);
                }
            }
        }
    }

    void Start()
    {

        
        
        baseSetup();
        mapSetup();
    }


    void Update()
    {
        
    }
}
