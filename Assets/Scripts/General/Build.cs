using UnityEngine;

public class Build : MonoBehaviour
{
    bool hasbuilt;
    GameObject Tower;
    GameObject Instant;
    public TowerOption towerOption;

    // Start is called before the first frame update
    void Start()
    {
        hasbuilt = false;
    }




    public void buildT()
    {
        if (Tower != null && !hasbuilt && placement.toPlace.towerPrefab != null)
        {
            if (placement.toPlace.hit.transform.tag == "walls")
            {
                if (placement.toPlace.allyData.isType(AllyType.Blocker))
                {
                    print("cannot place blocker on walls!");
                    return;
                }
                //GameObject instant = Instantiate(Tower, transform.position + placement.toPlace.wallOffset, Quaternion.identity) as GameObject;
                Quaternion rotate = new Quaternion();
                rotate.eulerAngles = placement.toPlace.rotate;
                Instant = Ally.spawn(Tower, placement.toPlace.allyData, transform.position + placement.toPlace.wallOffset, rotate);
                Instant.GetComponent<Ally>().enabled = true;
                Instant.GetComponent<Collider>().enabled = true;
                Instant.tag = "Ally";
                Destroy(placement.toPlace.towerToFollow);
                placement.toPlace.rotate = new Vector3(0, 0, 0);
                placement.toPlace.towerToFollow = null;
                placement.toPlace.towerPrefab = null;
                Tower = null;
                hasbuilt = true;
                towerOption.isCd = true;

            }

            if (placement.toPlace.hit.transform.tag == "valley")
            {
                if (!placement.toPlace.allyData.isType(AllyType.Blocker))
                {
                    print("cannot place ranger class on valleys!");
                    return;
                }

                //GameObject instant = Instantiate(Tower, transform.position + placement.toPlace.valleyOffset, Quaternion.identity) as GameObject;
                Quaternion rotate = new Quaternion();
                rotate.eulerAngles = placement.toPlace.rotate;
                Instant = Ally.spawn(Tower, placement.toPlace.allyData, transform.position + placement.toPlace.valleyOffset, rotate);
                Instant.GetComponent<Ally>().enabled = true;
                Instant.GetComponent<Collider>().enabled = true;
                Instant.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
                Instant.tag = "Ally";
                Destroy(placement.toPlace.towerToFollow);
                placement.toPlace.rotate = new Vector3(0, 0, 0);
                placement.toPlace.towerToFollow = null;
                placement.toPlace.towerPrefab = null;
                Tower = null;
                hasbuilt = true;
                towerOption.isCd = true;
            }
        }
    }

    /*void OnMouseUp()
    {
        if (Tower != null && !hasbuilt && placement.toPlace.towerModel != null)
        {
            if (placement.toPlace.hit.transform.tag == "walls")
            {
                if (placement.toPlace.allyData.isType(AllyType.Blocker))
                {
                    print("cannot place blocker on walls!");
                    return;
                }
                //GameObject instant = Instantiate(Tower, transform.position + placement.toPlace.wallOffset, Quaternion.identity) as GameObject;
                Quaternion rotate = new Quaternion();
                rotate.eulerAngles = placement.toPlace.rotate;
                Instant = Ally.spawn(Tower, placement.toPlace.allyData, transform.position + placement.toPlace.wallOffset, rotate);
                Instant.GetComponent<Ally>().enabled = true;
                Instant.GetComponent<Collider>().enabled = true;
                Instant.tag = "Ally";
                Destroy(placement.toPlace.towerToFollow);
                placement.toPlace.rotate = new Vector3(0, 0, 0);
                placement.toPlace.towerToFollow = null;
                placement.toPlace.towerModel = null;
                Tower = null;
                hasbuilt = true;

            }

            if (placement.toPlace.hit.transform.tag == "valley")
            {
                if (!placement.toPlace.allyData.isType(AllyType.Blocker))
                {
                    print("cannot place ranger class on valleys!");
                    return;
                }
                if (Validbuild == false)
                {
                    print("intercept with walls!");
                    return;
                }

                //GameObject instant = Instantiate(Tower, transform.position + placement.toPlace.valleyOffset, Quaternion.identity) as GameObject;
                Quaternion rotate = new Quaternion();
                rotate.eulerAngles = placement.toPlace.rotate;
                Instant = Ally.spawn(Tower, placement.toPlace.allyData, transform.position + placement.toPlace.valleyOffset, rotate);
                Instant.GetComponent<Ally>().enabled = true;
                Instant.GetComponent<Collider>().enabled = true;
                Instant.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
                Instant.tag = "Ally";
                Destroy(placement.toPlace.towerToFollow);
                placement.toPlace.rotate = new Vector3(0, 0, 0);
                placement.toPlace.towerToFollow = null;
                placement.toPlace.towerModel = null;
                Tower = null;
                hasbuilt = true;
            }
        }

    }*/

    // Update is called once per frame
    void Update()
    {
        if (placement.toPlace.towerPrefab != null)
        {
            Tower = placement.toPlace.towerPrefab;
        }

        if (Instant == null) hasbuilt = false;
    }
}
