using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject interactButton;

    public void ShowInteractButton()
    {
        interactButton.SetActive(true);
    }

    public void HideInteractButton()
    {
        interactButton.SetActive(false);
    }

    private void Awake()
    {
        interactButton = GameObject.Find("InteractButton");
        HideInteractButton();
    }
}
