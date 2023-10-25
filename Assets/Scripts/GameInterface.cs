using Assets.Scripts.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameInterface : MonoBehaviour
{

    public static GameInterface instance;
    public Text playerName;
    public Text opponentName;
    public Text turn;

    public static string playerN;
    public static string opponentN;
    public static string turnMsg;
    public static int playerOrder;
       

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        playerName.text = playerN;
        opponentName.text = opponentN;
        turn.text = turnMsg;
        playerOrder = 0;
    }

    public void ChangeTurn()
    {
        turn.text = turnMsg;
    }

    public void PlayCard(int cardnr)
    {
        //DataSender.SendPlayCard(cardnr);
    }
    
}
