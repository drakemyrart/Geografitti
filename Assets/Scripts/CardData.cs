using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardData : ScriptableObject
{
    public string cardname;
    public int cardnr;
    public string cardtype;
    public string cardsubtype;
    public int cardrank;
    public string carddescription;
    public int ruleid;
    public int attack;
    public int defence;
    public int hull;

    public Sprite artwork;
}
