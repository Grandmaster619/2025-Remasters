using TMPro;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    public int HurtAmount = 10;
    public int HealAmount = 10;

    private bool OpenDebugMenu = false;
    private GameObject DebugMenuScreen;
    private TMP_Text[] newText; 
    private TMP_Text PlayerHealthText;
    private TMP_Text PlayerStaminaText;
    private Health health;
    private Stamina stamina;

    // Start is called before the first frame update
    void Start()
    {
        health = gameObject.GetComponent<Health>();
        stamina = gameObject.GetComponent<Stamina>();
        DebugMenuScreen = GameObject.FindWithTag("DebugMenuScreen");
        newText = DebugMenuScreen.GetComponentsInChildren<TMP_Text>();
        PlayerHealthText = newText[3];
        PlayerStaminaText = newText[5];
        DebugMenuScreen.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(OpenDebugMenu)
            {
                OpenDebugMenu = false;
                DebugMenuScreen.SetActive(false);
            }
            else
            {
                OpenDebugMenu = true;
                DebugMenuScreen.SetActive(true);
            }   
        }

        if(OpenDebugMenu)
        {
            DebugCommands();
            PlayerHealthText.SetText(health.currentHealth.ToString());
            PlayerStaminaText.SetText(stamina.stamina_level.ToString());
        }

        

        // TEMP
        /*if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Debug command for damaging the player has been executed");
            health.TakeDamage(HurtAmount);
        }*/
    }

    void DebugCommands()
    {
        // K - Die 
        // Set player health to 0
        if (Input.GetKeyDown(KeyCode.K))
        {
            health.currentHealth = 0;
        }
        // L - Hurt
        // Player will take damage equal to HurtAmount
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Debug command for damaging the player has been executed");
            health.TakeDamage(HurtAmount);
        }
        // H - Heal
        // Player will recover health equal to HealAmount
        else if (Input.GetKeyDown(KeyCode.H))
        {
            health.Heal(HealAmount);
        }
    }
}
