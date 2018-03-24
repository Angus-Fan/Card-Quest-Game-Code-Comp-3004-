using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_plague : eventCardScript
{
    string description = "Drawer loses 2 shields if possible";
    public void abilityFunction(GameObject[] players, int currPlayerNum, GameObject pAdvDeck) { 

        if(players[currPlayerNum].GetComponent<playerScript>().shieldNum >= 2)
        {
            players[currPlayerNum].GetComponent<playerScript>().shieldNum = players[currPlayerNum].GetComponent<playerScript>().shieldNum - 2;
        }
        else if(players[currPlayerNum].GetComponent<playerScript>().shieldNum == 1)
        {
            players[currPlayerNum].GetComponent<playerScript>().shieldNum = 0;
        }
        else
        {
            Debug.Log("Not enough shields");
        }

    }
    // Use this for initialization
    void Start()
    {
        createCard("Plague", "Event", description);
        setCardImage();
        cardWidth = this.gameObject.GetComponent<SpriteRenderer>().size.x;
        cardLength = this.gameObject.GetComponent<SpriteRenderer>().size.y;
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(cardWidth, cardLength, 0);
    }

}
