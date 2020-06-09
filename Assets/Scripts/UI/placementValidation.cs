using UnityEngine;

public class PlacementValidation : MonoBehaviour
{

    public bool Validbuild;
    public GameObject invalid_indication;
    public GameObject core;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "walls")
        {
            core.SetActive(false);
            invalid_indication.SetActive(true);
            Validbuild = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "walls")
        {
            core.SetActive(true);
            invalid_indication.SetActive(false);
            Validbuild = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Validbuild = true;
        invalid_indication.SetActive(false);
    }


}
