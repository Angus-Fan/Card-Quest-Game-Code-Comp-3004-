using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour {
    [SerializeField]
    public string rankName;
    public int playerPower;
    public GameObject playerRank;
    public int shieldNum;
    public List<GameObject> hand ;
    public List<GameObject> playerField;
    public int numCardsinHand;
    public bool attemptingStage;
    public bool hasAnswered;
    

    // Use this for initialization
    public void setAttemptingStage(bool attemptingBool)
    {
        attemptingStage = attemptingBool;
    }
    void Start () {
        playerRank = null;
       
        numCardsinHand = 0;
        
	}
	
	// Update is called once per frame
	void Update () {

        setPowerOnField();
        setNumCardsInHand();
        

    }
    public void setNumCardsInHand()
    {
        numCardsinHand = hand.Count;
    }
    public void setPlayerRank(GameObject card)
    {
        playerRank = card;
        
    }
    public void setPowerOnField()
    
    {   
        
        int currentPower = 0;
        
        if (playerRank != null)
        {
            rankName = (playerRank.GetComponentInChildren<rankCardScript>().cardName);
            currentPower = currentPower + playerRank.GetComponentInChildren<rankCardScript>().power;
            
        }
        for(int i = 0; i < playerField.Count; i++)
        {
            GameObject currentCard = (playerField[i]);
            string strCardType = (currentCard.GetComponentInChildren<cardScript>().cardType);
           // Debug.Log(strCardType);
            if (strCardType == "Weapon")         
            {
               // Debug.Log("its a weapon");
                currentPower = currentPower + (currentCard.GetComponentInChildren<weaponCardScript>().power);
            }
            if (strCardType == "Foe")
            {
               // Debug.Log("its a foe");
                currentPower = currentPower +(currentCard.GetComponentInChildren<foeCardScript>().power);
            }
            if (strCardType == "Ally")
            {
                //Debug.Log("its an ally");
                currentPower = currentPower +(currentCard.GetComponentInChildren<allyCardScript>().power);
            }
        }
        playerPower = currentPower;
    }
 
    public void addToHand(GameObject card)
    {
        //there is currently no thing to check if hand is full add this later
        hand.Add(card);
        
     
        numCardsinHand++;
       
    }
    public void fromHandToField(GameObject card)
    {
        hand.Remove(card);
        playerField.Add(card);
        
    }
    public void increaseShield(int shieldIn)
    {
        shieldNum = shieldNum + shieldIn;
    }
  
}
