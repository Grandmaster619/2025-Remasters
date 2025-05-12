using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIDisplay : MonoBehaviour
{
    public HealthSystem playerHealthSystem;

    public List<GameObject> healthBarSprites; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(playerHealthSystem.CurHealth)
        {
            case 5:
                SwitchToHealth(5);
                break;
            case 4:
                SwitchToHealth(4);
                break;
            case 3:
                SwitchToHealth(3);
                break;
            case 2:
                SwitchToHealth(2);
                break;
            case 1:
                SwitchToHealth(1);
                break;
            case 0:
                SwitchToHealth(0);
                break;
            default:
                break;
        }
    }

    private void SwitchToHealth(int health)
    {
        foreach(GameObject obj in healthBarSprites)
        {
            if (healthBarSprites.IndexOf(obj) != health)
                obj.SetActive(false);
            else
                obj.SetActive(true);
        }
    }
}
