using UnityEngine;

public class ElevOpen : MonoBehaviour
{
    [SerializeField]
    private GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        door.GetComponent<Animator>().SetBool("IsOpen", true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}