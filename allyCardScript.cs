using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class allyCardScript : cardScript {
    public int power;
    public int bid;
    public bool hasAbility;
    public string ability;
    SpriteRenderer imgDisplay;

    // Use this for initialization
    public void createCard(string name, string type, int powerValue, int bidValue, bool hasAbil)
    {
        cardName = name;
        cardType = type;
        power = powerValue;
        bid = bidValue;
        hasAbility = hasAbil;

    }
    public void setCardImage()
    {
        imgDisplay = GetComponent<SpriteRenderer>();
        imgDisplay.sprite = front;
    }


}
