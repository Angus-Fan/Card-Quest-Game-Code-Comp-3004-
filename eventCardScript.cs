using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventCardScript : cardScript {
    public string cardDescript;
    SpriteRenderer imgDisplay;
    //public abstract abilityFunction(GameObject, int, GameObject) { }// Debug.Log("EVENTABILITYWOW"); }
    
    public void createCard(string name, string type, string description)
    {
        cardName = name;
        cardType = type;
        cardDescript = description;
    }
    public void setCardImage()
    {
        imgDisplay = GetComponent<SpriteRenderer>();
        imgDisplay.sprite = front;
    }
    
	// add a description
}
