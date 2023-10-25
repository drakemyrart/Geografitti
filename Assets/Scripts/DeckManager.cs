using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;
    public static List<CardData> deck;
    public static List<CardData> discardPile;
    public static CardData deckCard;

    //Card prefabs
    [SerializeField]
    GameObject cardTypeCraft;
    [SerializeField]
    GameObject cardTypeCharacter;
    [SerializeField]
    GameObject cardTypeFormation;
    [SerializeField]
    GameObject cardTypeEvent;
    CardInfo cardInfo;

    //PlayfieldAreas
    [SerializeField]
    GameObject drawDeck;
    [SerializeField]
    GameObject playerHand;
    [SerializeField]
    GameObject playArea;
    
    Button drawBtn;

    //Game mechanics objects
    GameObject inPlayFormation;
    GameObject inPlayCraft;
    GameObject inPlayCharacter;

    
    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        deck = new List<CardData>();
        discardPile = new List<CardData>();
        drawDeck = GameObject.Find("DrawDeck");
        playerHand = GameObject.Find("PlayerHand");
        playArea = GameObject.Find("PlayerArea");
        drawBtn = drawDeck.GetComponentInChildren<Button>();
        drawBtn.onClick.AddListener(DrawCard);
    }

    private void Update()
    {
        
    }

    public void LoadCardToDeck(CardData card)
    {
        deck.Add(card);
    }

    public void ShuffleDeck()
    {
        CardData temp = ScriptableObject.CreateInstance<CardData>();
        for (int i = 0; i < deck.Count; i++)
        {            
            temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        Destroy(temp);
    }

    public void DrawCard()
    {
        GameObject playerCard = Instantiate(cardTypeCraft);
        playerCard.transform.SetParent(playerHand.transform, false);
        cardInfo = playerCard.GetComponent<CardInfo>();
        cardInfo.cardInfo = deck[0];
        deck.Remove(deck[0]);

    }

    public void DiscardCard(CardData card)
    {
        discardPile.Add(card);
    }

    public void ReShuffleDeck()
    {
        for (int i = 0; i < discardPile.Count; i++)
        {
            deck.Add(discardPile[i]);
        }
        ShuffleDeck();
    }

    public void PlayCardMechanic(GameObject cardObject)
    {

    }
}
