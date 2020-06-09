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
        if (Tower != null && !hasbuilt && Placement.toPlace.towerPrefab != null)
        {
            if (Placement.toPlace.hit.transform.tag == "walls")
            {
                if (Placement.toPlace.allyData.isType(AllyType.Blocker))
                {
                    print("cannot place blocker on walls!");
                    return;
                }
                //GameObject instant = Instantiate(Tower, transform.position + Placement.toPlace.wallOffset, Quaternion.identity) as GameObject;
                Quaternion rotate = new Quaternion();
                rotate.eulerAngles = Placement.toPlace.rotate;
                Instant = Ally.spawn(Tower, Placement.toPlace.allyData, transform.position + Placement.toPlace.wallOffset, rotate);
                Instant.GetComponent<Ally>().enabled = true;
                Instant.GetComponent<Collider>().enabled = true;
                Instant.tag = "Ally";
                Destroy(Placement.toPlace.towerToFollow);
                Placement.toPlace.Mana -= Placement.toPlace.allyData.allyLevelData.cost;
                Placement.toPlace.rotate = new Vector3(0, 0, 0);
                Placement.toPlace.towerToFollow = null;
                Placement.toPlace.towerPrefab = null;
                Tower = null;
                hasbuilt = true;
                towerOption.isCd = true;

            }

            if (Placement.toPlace.hit.transform.tag == "valley")
            {
                if (!Placement.toPlace.allyData.isType(AllyType.Blocker))
                {
                    print("cannot place ranger class on valleys!");
                    return;
                }

                //GameObject instant = Instantiate(Tower, transform.position + Placement.toPlace.valleyOffset, Quaternion.identity) as GameObject;
                Quaternion rotate = new Quaternion();
                rotate.eulerAngles = Placement.toPlace.rotate;
                Instant = Ally.spawn(Tower, Placement.toPlace.allyData, transform.position + Placement.toPlace.valleyOffset, rotate);
                Instant.GetComponent<Ally>().enabled = true;
                Instant.GetComponent<Collider>().enabled = true;
                Instant.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
                Instant.tag = "Ally";
                Destroy(Placement.toPlace.towerToFollow);
                Placement.toPlace.Mana -= Placement.toPlace.allyData.allyLevelData.cost;
                Placement.toPlace.rotate = new Vector3(0, 0, 0);
                Placement.toPlace.towerToFollow = null;
                Placement.toPlace.towerPrefab = null;
                Tower = null;
                hasbuilt = true;
                towerOption.isCd = true;
            }
        }
    }

    /*void OnMouseUp()
    {
        if (Tower != null && !hasbuilt && Placement.toPlace.towerModel != null)
        {
            if (Placement.toPlace.hit.transform.tag == "walls")
            {
                if (Placement.toPlace.allyData.isType(AllyType.Blocker))
                {
                    print("cannot place blocker on walls!");
                    return;
                }
                //GameObject instant = Instantiate(Tower, transform.position + Placement.toPlace.wallOffset, Quaternion.identity) as GameObject;
                Quaternion rotate = new Quaternion();
                rotate.eulerAngles = Placement.toPlace.rotate;
                Instant = Ally.spawn(Tower, Placement.toPlace.allyData, transform.position + Placement.toPlace.wallOffset, rotate);
                Instant.GetComponent<Ally>().enabled = true;
                Instant.GetComponent<Collider>().enabled = true;
                Instant.tag = "Ally";
                Destroy(Placement.toPlace.towerToFollow);
                Placement.toPlace.rotate = new Vector3(0, 0, 0);
                Placement.toPlace.towerToFollow = null;
                Placement.toPlace.towerModel = null;
                Tower = null;
                hasbuilt = true;

            }

            if (Placement.toPlace.hit.transform.tag == "valley")
            {
                if (!Placement.toPlace.allyData.isType(AllyType.Blocker))
                {
                    print("cannot place ranger class on valleys!");
                    return;
                }
                if (Validbuild == false)
                {
                    print("intercept with walls!");
                    return;
                }

                //GameObject instant = Instantiate(Tower, transform.position + Placement.toPlace.valleyOffset, Quaternion.identity) as GameObject;
                Quaternion rotate = new Quaternion();
                rotate.eulerAngles = Placement.toPlace.rotate;
                Instant = Ally.spawn(Tower, Placement.toPlace.allyData, transform.position + Placement.toPlace.valleyOffset, rotate);
                Instant.GetComponent<Ally>().enabled = true;
                Instant.GetComponent<Collider>().enabled = true;
                Instant.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
                Instant.tag = "Ally";
                Destroy(Placement.toPlace.towerToFollow);
                Placement.toPlace.rotate = new Vector3(0, 0, 0);
                Placement.toPlace.towerToFollow = null;
                Placement.toPlace.towerModel = null;
                Tower = null;
                hasbuilt = true;
            }
        }

    }*/

    // Update is called once per frame
    void Update()
    {
        if (Placement.toPlace.towerPrefab != null)
        {
            Tower = Placement.toPlace.towerPrefab;
        }

        if (Instant == null) hasbuilt = false;
    }
}
