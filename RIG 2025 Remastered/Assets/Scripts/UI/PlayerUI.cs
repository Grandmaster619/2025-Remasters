using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;
    [SerializeField] private GameObject panel;
    public string noText = "";
    Image panelImage;

    private void Start()
    {
        promptText.alpha = 0f;
        panelImage = panel.GetComponent<Image>();
        Color32 color = panelImage.color;
        panel.GetComponent<Image>().color = new Color32(color.r, color.g, color.b, 0);

    }
    public void UpdateText(string promptMessage)
    {
        //Debug.Log("Update Text");
        panel.SetActive(true);
        promptText.text = promptMessage;
        promptText.alpha = 1f;
        Color32 color = panelImage.color;
        panelImage.color = new Color(color.r, color.g, color.b, 255f);
    }

    public void ResetText()
    {
        if (promptText.alpha >= 0.0f)
        {
            Color32 color = panelImage.color;
            promptText.alpha -= 0.032f;
            if (color.a <= 8)
            {
                panelImage.color = new Color32(color.r, color.g, color.b, 0);
            }
            else
            {
                panelImage.color = new Color32(color.r, color.g, color.b, color.a -= 8);
            } 
            //Debug.Log(promptText.alpha + " " + color.a);
        }
        else
        {
            promptText.alpha = 0f;
            Color32 color = panelImage.color;
            panelImage.color = new Color32(color.r, color.g, color.b, 0);
        }
    }


}

