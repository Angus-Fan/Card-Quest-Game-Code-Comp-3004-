using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A4_qGuinevereScript : allyCardScript {

	// Use this for initialization
	void Start () {
        createCard("Queen Guinevere", "Ally", 0,3, false);
        setCardImage();
        cardWidth = this.gameObject.GetComponent<SpriteRenderer>().size.x;
        cardLength = this.gameObject.GetComponent<SpriteRenderer>().size.y;
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(cardWidth, cardLength, 0);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
