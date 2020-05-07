using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    //grid DataStructure
    public class grid
    {
        public GameObject Grid;
        public int index;
        public GameObject up;
        public GameObject down;
        public GameObject left;
        public GameObject right;

        public grid(Vector3 vector, int index) 
        {
            Grid = new GameObject("grid" + index);
            Grid.transform.position = vector;
            this.index = index;
            up = null;
            down = null;
            left = null;
            right = null;
        }

        public grid(GameObject Object,GameObject up,GameObject down,GameObject left, GameObject right)
        {
            Grid = Object;
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
        }

        public GameObject setleft(GameObject left)
        {
            this.left = left;
            return this.left;
        }

        public GameObject setright(GameObject right)
        {
            this.right = right;
            return this.right;
        }

        public GameObject setup(GameObject up)
        {
            this.up = up;
            return this.up;
        }

        public GameObject setdown(GameObject down)
        {
            this.down = down;
            return this.down;
        }

    }



    //public parameters for UI
    public int rows;
    public int columns;
    public GameObject[] Base;
    public GameObject[] midTiles;
    public GameObject[] sideTiles;
    public GameObject[] enemySpawn;
    public int spawnNumber;

    //An empty to hold all maptiles
    private Transform mapHolder;
    private Transform spawnHolder;
    private Transform baseHolder;

    private List<Vector3> gridPositions = new List<Vector3>();

    //Mark base location for setting up map reasonablly.
    public Vector3 baseVector;
    private int baselocMark;

    private Vector3 mapCenter;
    private int mapCenterMark;

    public List<Vector3> spawnVectors = new List<Vector3>();



    //get center Vector3;
    public Vector3 getCenter() {
        return mapCenter;
    }


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

    List<grid> SetUpGridSystem()
    {
        int i = 0;
        Transform gridHolder = new GameObject("Grids").transform;
        List<grid> Grid = new List<grid>();

        for (int c = 0; c < columns*10; c = c + 10)
        {
            for (int r = 0; r < rows*10; r = r + 10)
            {
                Grid.Add(new grid(new Vector3(c, 0f, r), i));
                Grid[i].Grid.transform.SetParent(gridHolder);

                if (r!= 0)
                {
                    Grid[i].left = GameObject.Find("grid" + (i - 1));
                }

                i++;
                }
            }

            for(int count = 0; count < i; count++)
            {
                Grid[count].right = GameObject.Find("grid" + (count + 1));

            if (Grid[count].down == null && count <= i-rows) 
            { 
                Grid[count].down = GameObject.Find("grid" + (count + rows));
            }

            if (Grid[count].up == null && count >= rows) 
            {
                Grid[count].up = GameObject.Find("grid" + (count - rows));
            }

            }

        return Grid;
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
        baseVector = new Vector3(tempC * 10, 0f, tempR * 10);
        return ((tempC) * rows) + tempR + 1;
    }

    //set up base tiles.
    void baseSetup() {

        baseHolder = new GameObject("Base").transform;
        int row = rows * 10;
        int colum = columns * 10;
        baselocMark = markBase();

        for (int c = 0; c < colum; c = c + 10)
        {
            for (int r = 0; r < row; r = r + 10)
            {
                if (new Vector3(c,0f,r).Equals(baseVector))
                {
                    GameObject toInstantiate = Base[Random.Range(0, Base.Length)];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(c, 0f, r), ranRotation()) as GameObject;
                    instance.transform.SetParent(baseHolder);
                    break;
                }
            }
        }
    }

    //decide spawn locations according to base location
    List<int> markSpawn(int num)
    {
        if (num > mapCenterMark) throw new System.ArgumentException("Too many enemy spawns");
        List<int> locmark = new List<int>();

        if (baselocMark <= mapCenterMark-1) {
            for(int i = 0; i < num; i++)
            {
                int temp = Random.Range(mapCenterMark, rows*columns);
                while (locmark.Contains(temp))
                {
                    temp = Random.Range(mapCenterMark, rows * columns);
                }
                locmark.Add(temp);
            }
        }
        else
        {
            for(int i = 0; i < num; i++)
            {
                int temp = Random.Range(0, mapCenterMark);
                while (locmark.Contains(temp))
                {
                    temp = Random.Range(0, mapCenterMark);
                }
                locmark.Add(temp);
            }
        }

        return locmark;
    }

    //set up enemy spawn tiles.
    void spawnSetup() {
        spawnHolder = new GameObject("Spawns").transform;
        List<int> locmark = markSpawn(spawnNumber);
        int row = rows * 10;
        int colum = columns * 10;

        int i = 0;
        for (int c = 0; c < colum; c = c + 10)
        {
            for (int r = 0; r < row; r = r + 10)
            {
                if (locmark.Contains(i))
                {
                    spawnVectors.Add(new Vector3(c, 0f, r));
                    GameObject toInstant = enemySpawn[Random.Range(0, enemySpawn.Length)];
                    GameObject instant = Instantiate(toInstant, new Vector3(c, 0f, r), ranRotation());
                    instant.transform.SetParent(spawnHolder);
                }
                i++;
            }
        }
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
                if (!loc.Equals(baseVector) && !spawnVectors.Contains(loc))
                {
                    GameObject toInstantiate = midTiles[Random.Range(0, midTiles.Length)];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(c, 0f, r), ranRotation()) as GameObject;
                    instance.transform.SetParent(mapHolder);
                }

                int i = 0;
                if (i == mapCenterMark) mapCenter = loc;
                i++;
            }
        }
    }

    void Start()
    {
        mapCenterMark = rows/2*rows + columns ;
        List<grid> grids = SetUpGridSystem();

        baseSetup();
        spawnSetup();
        mapSetup();
    }


    void Update()
    {
        
    }
}
