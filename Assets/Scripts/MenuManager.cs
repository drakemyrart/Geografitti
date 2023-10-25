using Assets.Scripts.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField]
    Button play;

    [Header("PlayerData")]
    public Text username;
    public Text level;
    public Text gold;
    
    private void Awake()
    {
        instance = this;
        UpdateUI();
        play.interactable = true;
        play.onClick.AddListener(GraffitiMode);
        
    }

    public void UpdateUI()
    {
        username.text = GameManager.instance.username;
        level.text = GameManager.instance.level.ToString();
        gold.text = GameManager.instance.gold.ToString();

    }

    public void GraffitiMode()
    {
        Debug.Log("Button Pressed");
         GameManager.instance.Loading(2);
    }    
}
