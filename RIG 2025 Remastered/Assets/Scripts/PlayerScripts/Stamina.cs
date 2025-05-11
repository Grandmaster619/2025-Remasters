using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour

{
    public float stamina_level;
    public float MAX_STAMINA = 100.0f;
    bool isDrainedEnergyState = false;
    public float stamina_regeneration_speed = 0.4f;


    // Start is called before the first frame update
    void Start()
    {
        stamina_level = MAX_STAMINA;
    }

    // Update is called once per frame
    void Update()
    {
        if(stamina_level <= 0.0f)
        {
            SetIsDrainedEnergyState(true);
        }
        if(stamina_level >= 30.0f)
        {
            SetIsDrainedEnergyState(false );
        }
    }

    //stamina consumption
    public void ConsumeStamina(float amount)
    {
        if (Time.timeScale == 1f)
        {
            stamina_level -= amount;
            stamina_level = Mathf.Clamp(stamina_level, 0, MAX_STAMINA);
        }
    }
    //stamina generation
    public void RegenerateStamina(float amount)
    {
        if (Time.timeScale == 1f)
        {
            stamina_level += amount;
            stamina_level = Mathf.Clamp(stamina_level, 0, MAX_STAMINA);
        }
    }
    //Drained Energy state
    public void SetIsDrainedEnergyState(bool isDrainedEnergyState)
    {
        this.isDrainedEnergyState=isDrainedEnergyState;
    }
    public bool GetIsDrainedEnergyState() { return isDrainedEnergyState; }
}
