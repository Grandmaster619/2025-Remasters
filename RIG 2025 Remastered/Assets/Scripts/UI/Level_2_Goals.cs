using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level_2_Goals : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private ItemSO handleItem;
    [SerializeField] private ItemSO gasCanItem;
    [SerializeField] private ItemSO filledGasCanItem;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private string firstGoal;
    [SerializeField] private string secondGoal;
    [SerializeField] private string thirdGoal;
    [SerializeField] private string fourthGoal;
    [SerializeField] private string fiveGoal;
    [SerializeField] private string sixthGoal;
    [SerializeField] private string sevenGoal;

    // Start is called before the first frame update
    void Start()
    {
        goalText.text = firstGoal;
        StartCoroutine(goalChecker());
    }

    IEnumerator goalChecker()
    {
        while(playerInventory.ContainsAmount(handleItem) == 0)
        {
            yield return new WaitForSeconds(0.1f);
        }

        goalText.text = secondGoal;

        while (playerInventory.ContainsAmount(handleItem) <= 1)
        {
            yield return new WaitForSeconds(0.1f);
        }

        goalText.text = thirdGoal;

        while (playerInventory.ContainsAmount(handleItem) <= 2)
        {
            yield return new WaitForSeconds(0.1f);
        }

        goalText.text = fourthGoal;

        while (!GameManagerScene2.GetInstance().IsDrillControlPanelActive())
        {
            yield return new WaitForSeconds(1f);
        }

        goalText.text = fiveGoal;

        while (!playerInventory.Contains(gasCanItem))
        {
            yield return new WaitForSeconds(0.1f);
        }

        goalText.text = sixthGoal;

        while (!playerInventory.Contains(filledGasCanItem))
        {
            yield return new WaitForSeconds(1f);
        }

        goalText.text = sevenGoal;
    }
}
