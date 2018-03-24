using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adventureDeckScript : deckScript {
    public List<GameObject> getDeck()
    {
        return deck;
    }
    [SerializeField]
    //Have variables for each prefab
    //Weapons
    public GameObject excalibur;
    public GameObject lance;
    public GameObject battleAx;
    public GameObject sword;
    public GameObject horse;
    public GameObject dagger;
    //Foes
    public GameObject dragon;
    public GameObject giant;
    public GameObject mordred;
    public GameObject greenKnight;
    public GameObject blackKnight;
    public GameObject evilKnight;
    public GameObject saxonKnight;
    public GameObject robberKnight;
    public GameObject saxons;
    public GameObject boar;
    public GameObject thieves;
    //Tests
    public GameObject testOfValor;
    public GameObject testOfTemptation;
    public GameObject testOfMorganLeFey;
    public GameObject testOfTheQuestingBeast;
    //Allies
    public GameObject sirGalahad;
    public GameObject sirLancelot;
    public GameObject kingArthur;
    public GameObject sirTristan;
    public GameObject sirPellinore;
    public GameObject sirGawain;
    public GameObject sirPercival;
    public GameObject queenGuinevere;
    public GameObject queenIseult;
    public GameObject merlin;
    public GameObject amour;
    //Frequencies
    int numExcalibur = 2;
    int numLance = 6;
    int numBattleAx = 8;
    int numSword = 16;
    int numHorse = 11;
    int numDagger = 6;
    int numDragon = 1;
    int numGiant = 2;
    int numMordred = 4;
    int numGreenKnight = 2;
    int numBlackKnight = 3;
    int numEvilKnight = 6;
    int numSaxonKnight = 8;
    int numRobberKnight = 7;
    int numSaxons = 5;
    int numBoar = 4;
    int numTheives = 8;
    int numTestOfValor = 2;
    int numTestOfTemptation = 2;
    int numTestOfMorganLeFey = 2;
    int numTestOfTheQuestingBeast = 2;
    int numSirGalahad = 1;
    int numSirLancelot = 1;
    int numKingArthur = 1;
    int numSirTristan = 1;
    int numSirPellinore = 1;
    int numSirGawain = 1;
    int numSirPercival = 1;
    int numQueenGuinevere = 1;
    int numQueenIseult = 1;
    int numMerlin = 1;
    int numAmour = 8;

    // Use this for initialization
    void Start () {
        
        deckName = "Adventure Deck";
        //initiate the deck with the prefabs
        addWeaponCards();
        addFoeCards();
        addTestCards();
        addAllyCards();
        
      
		
	}
	
	// Update is called once per frame
	void Update () {
      //  drawCard();
        
	}
    void addWeaponCards()
    {
        //add #excaliburCards to deck
        addToDeck(excalibur, numExcalibur);
        //add #lance to deck
        addToDeck(lance, numLance);
        //add #battleAx to deck
        addToDeck(battleAx, numBattleAx);
        //add #sword to deck
        addToDeck(sword, numSword);
        //add #horse to deck
        addToDeck(horse, numHorse);
        //add #dagger to deck
        addToDeck(dagger, numDagger);
    }
    void addFoeCards()
    {
        
        addToDeck(dragon, numDragon);
        addToDeck(giant, numGiant);
        addToDeck(mordred, numMordred);
        addToDeck(greenKnight, numGreenKnight);
        addToDeck(blackKnight, numBlackKnight);
        addToDeck(evilKnight, numEvilKnight);
        addToDeck(saxonKnight, numSaxonKnight);
        addToDeck(robberKnight, numRobberKnight);
        addToDeck(saxons, numSaxons);
        addToDeck(boar, numBoar);
        addToDeck(thieves, numTheives);
    }
    void addTestCards()
    {
        
        addToDeck(testOfValor, numTestOfValor);
        addToDeck(testOfTemptation, numTestOfTemptation);
        addToDeck(testOfMorganLeFey, numTestOfMorganLeFey);
        addToDeck(testOfTheQuestingBeast, numTestOfTheQuestingBeast);
    }
    void addAllyCards()
    {
        
        addToDeck(sirGalahad, numSirGalahad);
        addToDeck(sirLancelot, numSirLancelot);
        addToDeck(kingArthur, numKingArthur);
        addToDeck(sirTristan, numSirTristan);
        addToDeck(sirPellinore, numSirPellinore);
        addToDeck(sirGawain, numSirGawain);
        addToDeck(sirPercival, numSirPercival);
        addToDeck(queenGuinevere, numQueenGuinevere);
        addToDeck(queenIseult, numQueenIseult);
        addToDeck(merlin, numMerlin);
        
        addToDeck(amour, numAmour);
    }
        
    //Add Loop
    public void addToDeck(GameObject gameObj,int num) { 
    
        //Initialize the cards out of bounds so they dont show up 
      
        
        int counter;
        for (counter = 0; counter < num; counter++)
        {

            GameObject cardObject = Instantiate(gameObj,new Vector3(-100,-100,-100),new Quaternion(0,0,0,0));
           
                
                deck.Add(cardObject);
                

        }
        //Debug.Log("added " + num + "" + gameObj);
    }
    public int randomNumberGen()
    {

        int randomNum = Random.Range(0, deck.Count);
        return randomNum;
    }
    public GameObject drawCard()
    {
        //Debug.Log(deck.Count);
        GameObject drawnCard;
        if (deck.Count > 0)
        {
            
            int indexToDraw = randomNumberGen();
            drawnCard = deck[indexToDraw];
            deck.Remove(drawnCard);
           // Debug.Log("Randomed index" + indexToDraw);
            //Debug.Log("drawn card is" + drawnCard);
        }
        else
        {
            Debug.Log("outta cards");
            return null;
        }

       
        return drawnCard;
    }
}
