using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Author: Angus  
//Refactore by: Angus 
//Game Manager - overall controller for the game
public class gameManagerScript : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    [Header("GameDataStructures")]
    private Transform objectHit;
    //List of cards
    private List<GameObject> discardPile;
    private List<GameObject> questStage;
    //This holds the questStage
    private List<GameObject> currentStageInProgress;

    //List of a List of Cards (Cards sper Stage) (Stages per Quest)
    private List<List<GameObject>> questStages;

    //List of players
    
    private List<GameObject> playersAttemptingQuest;
    

    private bool thisStageHasFoeOrTest;
   
    private int currentStage;
    private int playerNumOfSponsor;
    private GameObject currentlyHoveredCard;
    private GameObject selectedCard;
    private GameObject storyCardDrawn;
    private string storyCardDrawnType;
    private int swapPlayerNum;
    public Camera gameCamera;
   

    private GameObject questSponsor;
    private bool viewNextStage;
    private int questSponsorNum;
    private bool hasWonRound = false;
    private int currentStageIterator;

    [Header("Deck Objects")]
    public GameObject advDeckObject;
    private GameObject advDeck;
    public GameObject strDeckObject;
    private GameObject strDeck;
    public GameObject rnkDeckObject;
    private GameObject rnkDeck;

    private int bonusShields;
    
    private int currentPlayerTurn;
    private int numPlayers;

    [Header("Game Players")]
    public GameObject player;
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;
    private GameObject[] players;   
    public GameObject currentPlayer;
    private int currentPlayerNum;


    [Header("Unity UI Buttons and Canvas's")]
    public GameObject gameStats;
    public GameObject startGameButton;
    public GameObject playerHandCanvas;
    public GameObject fieldCanvas;
    public GameObject stageCanvas;
    public GameObject viewNextStageButton;
    public GameObject attemptQuestButton;
    public GameObject sponsorButton;
    public GameObject nextStageButton;
    public GameObject swapPlayerButton;
    public GameObject swapHandAndFieldView;
    public GameObject endTurnButton;

    [Header("Misc bool")]
    private bool showHandInUIBool;
    private bool showFieldInUIBool;
    private bool noOneAttempted;
    private bool stagesAreDoneBeingSet;
    private bool isSponsored;
    private bool sponsorPhase;
    private bool discardPhase;
    private int skipCounter;

    [Header("UI Text Elements")]
    public Text player1Rank;
    public Text player1Power;
    public Text player1Shield;
    public Text player1NumCards;

    public Text player2Rank;
    public Text player2Power;
    public Text player2Shield;
    public Text player2NumCards;

    public Text player3Rank;
    public Text player3Power;
    public Text player3Shield;
    public Text player3NumCards;

    public Text player4Rank;
    public Text player4Power;
    public Text player4Shield;
    public Text player4NumCards;

    public Text currentPlayerTurnText;
    public Text currentPlayersHand;
    public Text currentStageBPText;
    public Text currentlyShownInUI;
   





    void Start () {
        //Starts at 5 for the reasoning that its not initially 0
        questSponsorNum = 5;
        showHandInUIBool = true;
        showFieldInUIBool = false;
        viewNextStage = false;
        numPlayers = 4;
        instantiateDecks();
        thisStageHasFoeOrTest = false;
       
        instantiatePlayers();
        players = new GameObject[4] { player1, player2, player3, player4 };
        currentPlayerNum = 0;
        currentPlayer = players[currentPlayerNum];
       

    }
	
    public void gameInitalizer()
    {
       
        giveSquireCards();
        currentPlayerTurn = 0;
        startGameButton.GetComponentInParent<Canvas>().enabled = false;
        gameStats.GetComponent<Canvas>().enabled = true;
        fieldCanvas.GetComponent<Canvas>().enabled = true;
        playerHandCanvas.GetComponent<Canvas>().enabled = true;
        discardPile = new List<GameObject>();
        questStage = new List<GameObject>();
        playersAttemptingQuest = new List<GameObject>();
        questStages= new List<List<GameObject>>();
       
        questStages = new List<List<GameObject>>();
        currentStageInProgress = new List<GameObject>();
        for (int i = 0; i < numPlayers; i++)
        {
            players[i].GetComponent<playerScript>().increaseShield(5);
            for(int t = 0; t < 10; t++)
            {
                players[i].GetComponent<playerScript>().addToHand(advDeck.GetComponentInChildren<adventureDeckScript>().drawCard());
            }
        }
        
        
        storyCardDrawn = drawFromStoryDeck();
        showDrawnCardFromStoryDeckInfo(storyCardDrawn);
       if(storyCardDrawn.GetComponentInChildren<cardScript>().cardType == "Event")
        {
            runEvents();
        }
       if(storyCardDrawn.GetComponentInChildren<cardScript>().cardType == "quest")
        {
            runQuests();
        }
    }
    public void runQuests()
    {
        isSponsored = false;
        sponsorButton.GetComponent<Button>().interactable = true;
        

    }
    

    //This is where the meat of the game is 
    void Update () {
        
        //UI Update Stuff
        updatePlayerInfoTop();
        updateCardHover();
        if (selectedCard != null && sponsorPhase==false)
        {
            //This function allows players to add the selected card to the field
            //This does not yet have the checks for card types
            addToPlayerField();
        }
        if (isSponsored == true && stagesAreDoneBeingSet!=true)
        {
            //This allows the sponsor of a quest to add cards to the stage
            //Checks implemented: Double Weapon Clause, Foe/Test Clause
            setQuestStages();
        }
        //If stages are done being set and the quest is sponsored then the players will attempt
        if (isSponsored == true && stagesAreDoneBeingSet == true)
        {
            if (checkIfEveryoneHasAnsweredIfTheyWillAttemptQuest())
            {   
                if (currentStageIterator != storyCardDrawn.GetComponentInChildren<questCardScript>().stages)
                {
                    currentStageInProgress = questStages[currentStageIterator];
                }
                showCurrentStageInUI();
               
                
                playersVsStage();
                            }
        }
    }
    //Check the set the boolean value to true with the button
    public void viewNextStageToTrue()
    {
        viewNextStage = true;
    }
    public void revertPlayerAttempts()
    {
        foreach(GameObject player in players)
        {
            player.GetComponent<playerScript>().attemptingStage = false;
            player.GetComponent<playerScript>().hasAnswered = false;
        }
    }
    public void playersVsStage()
    {

        List<GameObject> playersThatLost = new List<GameObject>();
        int stagepower = calculateStagePower(currentStageInProgress);
        foreach (GameObject player in playersAttemptingQuest)
        {

            if (player.GetComponent<playerScript>().playerPower >= stagepower )
            {
                if (hasWonRound == false)
                {
                    Debug.Log("player beat the stage");
                    player.GetComponent<playerScript>().shieldNum = player.GetComponent<playerScript>().shieldNum + 1;
                }
            }
            else
            {
                Debug.Log("player lost");
                playersThatLost.Add(player);
            }
            
        }
        hasWonRound = true;
        foreach(GameObject playerThatLost in playersThatLost)
        {
            playersAttemptingQuest.Remove(playerThatLost);
        }
    
       
         
            if (currentStageIterator >= storyCardDrawn.GetComponentInChildren<questCardScript>().stages)
            {


            hideCurrentStageInUI();
            viewNextStage = false;
            hasWonRound = false;
            nextTurn();

            }
            else if(viewNextStage)
            {
                
                    currentStageIterator++;
                    viewNextStage = false;
                    hasWonRound = false;


        }
        else if (playersAttemptingQuest.Count == 0)
        {
            Debug.Log("SPONSOR WON");
            hideCurrentStageInUI();
            questSponsor.GetComponent<playerScript>().shieldNum = questSponsor.GetComponent<playerScript>().shieldNum + storyCardDrawn.GetComponentInChildren<questCardScript>().stages;
            viewNextStage = false;
            hasWonRound = false;
            nextTurn();

        }

    }
    public bool checkIfEveryoneHasAnsweredIfTheyWillAttemptQuest()
    {
        bool verdict = true;
        for(int i = 0; i < numPlayers; i++)
        {
            if (i != questSponsorNum)
            {
                verdict = verdict & players[i].GetComponent<playerScript>().hasAnswered;
            }
        }
        Debug.Log(verdict);
        return verdict;
    }
    public void thisPlayerWillAttemptTheQuest()
    {
        currentPlayer.GetComponent<playerScript>().attemptingStage = true;
        currentPlayer.GetComponent<playerScript>().hasAnswered = true;
        playersAttemptingQuest.Add(currentPlayer);
        swapPlayer();
    }
   
    
    public GameObject drawFromStoryDeck()
    {
        GameObject drawnCardFromDeck;
        drawnCardFromDeck = strDeck.GetComponentInChildren<storyDeckScript>().drawCard();
        return drawnCardFromDeck;
    }
    public void showDrawnCardFromStoryDeckInfo(GameObject strCard)
    {
        float displayX = fieldCanvas.GetComponent<RectTransform>().position.x;
        float displayY = fieldCanvas.GetComponent<RectTransform>().position.y;
        float displayZ = fieldCanvas.GetComponent<RectTransform>().position.z;
        float cardWidth;
        if (strCard != null)
        {
            cardWidth = strCard.GetComponentInChildren<SpriteRenderer>().size.x;
            strCard.GetComponent<Transform>().position = new Vector3(displayX - (fieldCanvas.GetComponent<RectTransform>().rect.width/2)+(cardWidth)+10, displayY, displayZ-10);
            strCard.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(4, 4, 4);
        }
    }
    public void hideDrawnCardFromStoryDeckInfo(GameObject strCard)
    {
        
            strCard.GetComponent<Transform>().position = new Vector3(-100,-100,-100);
            strCard.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(4, 4, 4);
        
    }
    public void instantiatePlayers()
    {
        player1 = Instantiate(player);
        player2 = Instantiate(player);
        player3 = Instantiate(player);
        player4 = Instantiate(player);
    }
    
    public void instantiateDecks()
    {
        advDeck = Instantiate(advDeckObject,new Vector3(-100,-100,-100),new Quaternion(0,0,0,0));
        strDeck = Instantiate(strDeckObject, new Vector3(-100, -100, -100), new Quaternion(0, 0, 0, 0));
        rnkDeck = Instantiate(rnkDeckObject, new Vector3(-100, -100, -100), new Quaternion(0, 0, 0, 0));
    }
    

    public void giveSquireCards()
    {
        for (int i = 0; i < numPlayers; i++)
        {
          
            players[i].GetComponent<playerScript>().setPlayerRank(rnkDeck.GetComponentInChildren<rankDeckScript>().drawSquire());
        }
    }
    //hideThisUIPlayerHandOrField
    //Input: GameObject current player , int thingToHide = 1 for hand = 2 for field
    public void hideThisFromTheUIPlayerHandOrField(GameObject playerToHide,int thingToHide )
    {
        
        if (thingToHide == 1)
        {
            foreach (GameObject playerCard in playerToHide.GetComponent<playerScript>().hand)
            {

                //Debug.Log("PLACED CARD IN X:" + playerCard.GetComponentInChildren + " Y IN:" + canvasYPos + " Z in:" + canvasZPos);
                playerCard.GetComponent<Transform>().position = new Vector3(0, -10, 0);

            }
        }
        else if(thingToHide == 2){
            foreach (GameObject playerCard in playerToHide.GetComponent<playerScript>().playerField)
            {

                //Debug.Log("PLACED CARD IN X:" + playerCard.GetComponentInChildren + " Y IN:" + canvasYPos + " Z in:" + canvasZPos);
                playerCard.GetComponent<Transform>().position = new Vector3(0, -10, 0);

            }

        }
        else
        {
            Debug.Log("The int to specify hand or field is incorrect : " + thingToHide);
        }
    }
    //Only for testing
    public void debugLogTheQuestStages()
    {
        foreach(List<GameObject> quest in questStages)
        {
            Debug.Log("NEW QUEST STAGE");
            foreach( GameObject card in quest)
            {
                Debug.Log(card);
            }
        }
    }
    
    public void nextTurn()
    {
        endTurnButton.GetComponent<Button>().interactable = true;
        if (currentPlayerTurn == numPlayers-1)
        {
            currentPlayerTurn = 0;
        }
        else
        {
            currentPlayerTurn++;
          
        }
        

        hideDrawnCardFromStoryDeckInfo(storyCardDrawn);
        questStages = new List<List<GameObject>>();
        hideUIInNextandSwapTurn();
        currentPlayerNum = currentPlayerTurn;
        stagesAreDoneBeingSet = false;
        revertPlayerAttempts();
        Debug.Log(currentPlayerTurn);
        currentPlayer = players[currentPlayerTurn];
        // players[currentPlayerNum].GetComponent<playerScript>().addToHand(advDeck.GetComponentInChildren<adventureDeckScript>().drawCard());
        displayUIInNextandSwapTurn();
        storyCardDrawn = null;
        storyCardDrawn = drawFromStoryDeck();
        showDrawnCardFromStoryDeckInfo(storyCardDrawn);
        attemptQuestButton.GetComponent<Button>().interactable = false;
        sponsorButton.GetComponent<Button>().interactable = false;
        if (storyCardDrawn != null)
        {
            if (storyCardDrawn.GetComponentInChildren<cardScript>().cardType == "Event")
            {
                runEvents();
            }
            if (storyCardDrawn.GetComponentInChildren<cardScript>().cardType == "quest")
            {
                runQuests();
            }
        }
    }
    public void swapPlayer()
    {
        hideUIInNextandSwapTurn();
        if (stagesAreDoneBeingSet == true && currentPlayer.GetComponent<playerScript>().attemptingStage!=true)
        {
            currentPlayer.GetComponent<playerScript>().attemptingStage = false;
            currentPlayer.GetComponent<playerScript>().hasAnswered = true;
        }
        if (currentPlayerNum == numPlayers - 1)
        {
            currentPlayerNum = 0;
        }
        else
        {                       
            currentPlayerNum++;         
        }
        currentPlayer = players[currentPlayerNum];
        if (currentPlayerNum == questSponsorNum)
        {
            swapPlayer();
        }
        displayUIInNextandSwapTurn();
      
    }
    public void displayUIInNextandSwapTurn()
    {
        if (showHandInUIBool == true)
        {
            showThisToUIPlayerHandOrField(currentPlayer, 1);
        }
        else
        {
            showThisToUIPlayerHandOrField(currentPlayer, 2);
        }

    }
    public void hideUIInNextandSwapTurn()
    {
        if (showHandInUIBool == true)
        {
            hideThisFromTheUIPlayerHandOrField(currentPlayer, 1);
        }
        else
        {
            hideThisFromTheUIPlayerHandOrField(currentPlayer, 2);
        }
    }
  

/// Used by event card for additional shields on quest completion
/// The additionalQuestShields is added to bonusShields and call this when a quest is completed
/// Remember to set bonus shield back to 0 afterwards
    public void addBonusShields(int additionalQuestShields)
    {
        bonusShields = additionalQuestShields + bonusShields;
    }
    //Run the eventCard's effect
    public void runEvents()
    {
        if (storyCardDrawn != null)
        {
            if (storyCardDrawn.GetComponentInChildren<cardScript>().cardName == "Chivalrous Deed")
            {
                storyCardDrawn.GetComponentInChildren<E1_deed>().abilityFunction(players, currentPlayerNum, advDeck);
            }
            else if (storyCardDrawn.GetComponentInChildren<cardScript>().cardName == "Court Called to Camelot")
            {
                storyCardDrawn.GetComponentInChildren<E2_court>().abilityFunction(players, currentPlayerNum, advDeck);
            }
            else if (storyCardDrawn.GetComponentInChildren<cardScript>().cardName == "King's Call to Arms")
            {
                storyCardDrawn.GetComponentInChildren<E3_kCallToArms>().abilityFunction(players, currentPlayerNum, advDeck);
            }
            else if (storyCardDrawn.GetComponentInChildren<cardScript>().cardName == "King's Recognition")
            {
                addBonusShields(2);
            }
            else if (storyCardDrawn.GetComponentInChildren<cardScript>().cardName == "Plague")
            {
                storyCardDrawn.GetComponentInChildren<E5_plague>().abilityFunction(players, currentPlayerNum, advDeck);
            }
            else if (storyCardDrawn.GetComponentInChildren<cardScript>().cardName == "Pox")
            {
                storyCardDrawn.GetComponentInChildren<E6_pox>().abilityFunction(players, currentPlayerNum, advDeck);
            }
            else if (storyCardDrawn.GetComponentInChildren<cardScript>().cardName == "Prospertiy Throughtout the Realm")
            {
                storyCardDrawn.GetComponentInChildren<E7_prosperity>().abilityFunction(players, currentPlayerNum, advDeck);
            }
            else if (storyCardDrawn.GetComponentInChildren<cardScript>().cardName == "Queen's Favor")
            {
                storyCardDrawn.GetComponentInChildren<E8_queen>().abilityFunction(players, currentPlayerNum, advDeck);
            }
        }
        
    }
    //Add the selectedCard to the player's Field
    public void addToPlayerField()
    {
        currentPlayer.GetComponent<playerScript>().fromHandToField(selectedCard);
        selectedCard.GetComponentInChildren<Transform>().position = new Vector3(-100, -100, -100);
        selectedCard = null;
    }
    //This is accessed by the sponsor quest button
    public void sponsorQuest()
    {
        
        questSponsor = currentPlayer;
        questSponsorNum = currentPlayerNum;
        currentStage = 1;
        isSponsored = true;
        sponsorPhase = true;
      
        questStage = new List<GameObject>();
        
        currentStageIterator = 0;
       
        sponsorButton.GetComponent<Button>().interactable = false;
        swapPlayerButton.GetComponent<Button>().interactable = false;
        endTurnButton.GetComponent<Button>().interactable = false;
        nextStageButton.GetComponent<Button>().interactable = true;
    }
    //Used to check what cards are displayed (used by the swap button)
    public void swapCardsInUI()
    {
        if(showHandInUIBool == true)
        {
            showHandInUIBool = false;
            showFieldInUIBool = true;
                
        }
        else if (showFieldInUIBool == true)
        {
            showFieldInUIBool = false;
            showHandInUIBool = true;
            
        }
        
    }

    public void showThisToUIPlayerHandOrField(GameObject playerToShow,int thingToShow)
    {
        float canvasXPos = playerHandCanvas.transform.position.x;
        //float canvasYPos = playerHandCanvas.transform.position.y;
        float canvasZPos = playerHandCanvas.transform.position.z;
        float canvasWidth = playerHandCanvas.GetComponent<RectTransform>().rect.width;
        float canvasHeight = playerHandCanvas.GetComponent<RectTransform>().rect.height;
        float cardWidth;
        float initialCardSpot;
        float counterX = 0;
        float counterY = 0;
        if (thingToShow == 1)
        {
            foreach (GameObject playerCard in playerToShow.GetComponent<playerScript>().hand)
            {

                cardWidth = playerCard.GetComponentInChildren<SpriteRenderer>().size.x;


                initialCardSpot = canvasXPos - (canvasWidth / 2) + cardWidth;
                //Debug.Log("PLACED CARD IN X:" + playerCard.GetComponentInChildren + " Y IN:" + canvasYPos + " Z in:" + canvasZPos);
                playerCard.GetComponent<Transform>().position = new Vector3(initialCardSpot + (canvasWidth / playerToShow.GetComponent<playerScript>().numCardsinHand) * counterX, 12 + counterY, canvasZPos);
                counterX++;
                counterY = counterY + (float)0.01;
            }
        }
        else if (thingToShow == 2)
        {
            foreach (GameObject playerCard in playerToShow.GetComponent<playerScript>().playerField)
            {

                cardWidth = playerCard.GetComponentInChildren<SpriteRenderer>().size.x;


                initialCardSpot = canvasXPos - (canvasWidth / 2) + cardWidth;
                //Debug.Log("PLACED CARD IN X:" + playerCard.GetComponentInChildren + " Y IN:" + canvasYPos + " Z in:" + canvasZPos);
                playerCard.GetComponent<Transform>().position = new Vector3(initialCardSpot + (canvasWidth / playerToShow.GetComponent<playerScript>().numCardsinHand) * counterX, 12 + counterY, canvasZPos);
                counterX++;
                counterY = counterY + (float)0.01;
            }
        }
    }
   
    public void hideCurrentStageInUI()
    {
        foreach (List<GameObject> stageIT in questStages)
        {
            foreach (GameObject card in stageIT)
            {

                card.GetComponentInChildren<cardScript>().transform.position = new Vector3(-100, -100, -100);
                card.GetComponentInChildren<cardScript>().transform.localScale = new Vector3(2, 2, 2);

            }
        }
        stageCanvas.GetComponent<Canvas>().enabled = false;
    }
    public void showCurrentStageInUI()
    {
        stageCanvas.GetComponent<Canvas>().enabled = true;
        float displayX = stageCanvas.GetComponent<RectTransform>().position.x;
       // float displayY = stageCanvas.GetComponent<RectTransform>().position.y;
        float displayZ = storyCardDrawn.GetComponent<Transform>().position.z;
        float differenceInX = stageCanvas.GetComponent<RectTransform>().rect.width;
        float counterX = 0;
        float counterY = currentStage;
        foreach (GameObject card in currentStageInProgress)
        {
            //Debug.Log(card);
            card.GetComponentInChildren<cardScript>().transform.position = new Vector3( (displayX-(differenceInX/2)) + (differenceInX / currentStageInProgress.Count) * counterX, 12+counterY, displayZ);
            card.GetComponentInChildren<cardScript>().transform.localScale = new Vector3(4, 4, 4);
            counterX = counterX + (float)0.5;
        }
        currentStageBPText.text =  calculateStagePower(currentStageInProgress).ToString();
    }
    public void nextStage()
    {
        questStages.Add(questStage);
        thisStageHasFoeOrTest = false;
        questStage = new List<GameObject>();
        
        if (currentStage == storyCardDrawn.GetComponentInChildren<questCardScript>().stages)
        {
            nextStageButton.GetComponent<Button>().interactable = false;
            /*
            foreach (List<GameObject> stage in questStages)
            {
                Debug.Log("NEWSTAGE");
                foreach(GameObject card in stage)
                {
                    Debug.Log(card);
                }
            }*/
            sponsorPhase = false;
            stagesAreDoneBeingSet = true;
            swapPlayer();
            swapPlayerButton.GetComponent<Button>().interactable = true;
            attemptQuestButton.GetComponent<Button>().interactable = true;

            debugLogTheQuestStages();
        }
        currentStage++;



    }

    //This is how the sponsor places cards into the quest stages
    public void setQuestStages()
    {
        bool existsInList = false;
        string cardType;
       
        if (selectedCard != null)
        {
            cardType = (selectedCard.GetComponentInChildren<cardScript>().cardType);
            if (thisStageHasFoeOrTest == false)
            {
              
                if (cardType == "Foe" || cardType == "Test" )
                {
                    selectedCard.GetComponentInChildren<BoxCollider>().enabled = false;
                    questStage.Add(selectedCard);
                    currentPlayer.GetComponent<playerScript>().hand.Remove(selectedCard);
                    Debug.Log("Added : " + selectedCard + " to the quest stage");
                    selectedCard.GetComponentInChildren<Transform>().position = new Vector3(-1000, -1000, -1000);
                    selectedCard = null;
                    thisStageHasFoeOrTest = true;
                }
               

            }
            else if (cardType == "Weapon")
            {
             //   Debug.Log("IT WAS A WEAPON");
                foreach(GameObject potentialWeapon in questStage)
                {
                  //  Debug.Log(potentialWeapon.GetComponentInChildren<cardScript>().cardName);
                   // Debug.Log(selectedCard.GetComponentInChildren<cardScript>().cardName);
                    if (potentialWeapon.GetComponentInChildren<cardScript>().cardName == selectedCard.GetComponentInChildren<cardScript>().cardName)
                    {
                        
                        existsInList = true;

                    }
                }
                if (existsInList == false)
                {
                    selectedCard.GetComponentInChildren<BoxCollider>().enabled = false;
                    questStage.Add(selectedCard);
                    currentPlayer.GetComponent<playerScript>().hand.Remove(selectedCard);
                    Debug.Log("Added : " + selectedCard + " to the quest stage");
                    selectedCard.GetComponentInChildren<Transform>().position = new Vector3(-1000, -1000, -1000);
                    selectedCard = null;
                }
            }


        }

    }
    //Display each player's stats
    public void updatePlayerInfoTop()
    {
        currentPlayersHand.text = "Player " + (currentPlayerNum + 1);
        if (showHandInUIBool == true)
        {
            hideThisFromTheUIPlayerHandOrField(currentPlayer, 2);
            showThisToUIPlayerHandOrField(currentPlayer, 1);
            currentlyShownInUI.text = "Currently displaying: hand";
        }
        else if (showFieldInUIBool == true)
        {
            hideThisFromTheUIPlayerHandOrField(currentPlayer, 1);
            showThisToUIPlayerHandOrField(currentPlayer, 2);
            currentlyShownInUI.text = "Currently displaying: field";
        }
        player1.GetComponent<playerScript>().setPowerOnField();
        player1Rank.text = player1.GetComponent<playerScript>().rankName;
        player1Power.text = player1.GetComponent<playerScript>().playerPower.ToString();
        player1Shield.text = player1.GetComponent<playerScript>().shieldNum.ToString();
        player1NumCards.text = player1.GetComponent<playerScript>().numCardsinHand.ToString();

        player2.GetComponent<playerScript>().setPowerOnField();
        player2Rank.text = player2.GetComponent<playerScript>().rankName;
        player2Power.text = player2.GetComponent<playerScript>().playerPower.ToString();
        player2Shield.text = player2.GetComponent<playerScript>().shieldNum.ToString();
        player2NumCards.text = player2.GetComponent<playerScript>().numCardsinHand.ToString();

        player3.GetComponent<playerScript>().setPowerOnField();
        player3Rank.text = player3.GetComponent<playerScript>().rankName;
        player3Power.text = player3.GetComponent<playerScript>().playerPower.ToString();
        player3Shield.text = player3.GetComponent<playerScript>().shieldNum.ToString();
        player3NumCards.text = player3.GetComponent<playerScript>().numCardsinHand.ToString();

        player4.GetComponent<playerScript>().setPowerOnField();
        player4Rank.text = player4.GetComponent<playerScript>().rankName;
        player4Power.text = player4.GetComponent<playerScript>().playerPower.ToString();
        player4Shield.text = player4.GetComponent<playerScript>().shieldNum.ToString();
        player4NumCards.text = player4.GetComponent<playerScript>().numCardsinHand.ToString();

        currentPlayerTurnText.text = "Current turn is player : " + (currentPlayerTurn+1).ToString();
        

    }
    //Visualization of the card you are mousing over
    public void updateCardHover()
    {
        for (int z = 0; z < numPlayers; z++)
        {
            int currShields = players[z].GetComponent<playerScript>().shieldNum;
            if (currShields >= 10 & currShields < 17)
            {

                players[z].GetComponent<playerScript>().setPlayerRank(rnkDeck.GetComponentInChildren<rankDeckScript>().deck[1]);
            }
            else if (currShields >= 17 && currShields < 27)
            {
                players[z].GetComponent<playerScript>().setPlayerRank(rnkDeck.GetComponentInChildren<rankDeckScript>().deck[2]);
            }
            else if (currShields >= 27)
            {
                Debug.Log("YOU WIN");
            }
        }
        RaycastHit hit;
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {

            objectHit = hit.transform;
            if (objectHit != currentlyHoveredCard & currentlyHoveredCard != null)
            {
                currentlyHoveredCard.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(2, 2, 2);
            }

            currentlyHoveredCard = objectHit.gameObject.transform.parent.gameObject;

            currentlyHoveredCard.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(4, 4, 4);
            if (Input.GetMouseButtonDown(0))
                selectedCard = currentlyHoveredCard;

        }
        else
        {
            if (currentlyHoveredCard != null)
            {
                currentlyHoveredCard.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(2, 2, 2);
                currentlyHoveredCard = null;
            }
        }
    }
    //Calculates the power of the stage placed in parameter
    public int calculateStagePower(List<GameObject> stageCards)
    {
        int StagePower = 0;
        string stageCardName;
     foreach(GameObject stageCard in stageCards)
        {
            stageCardName = stageCard.GetComponentInChildren<cardScript>().cardType;
            if (stageCardName == "Weapon")
            {
                StagePower = StagePower + stageCard.GetComponentInChildren<weaponCardScript>().power;


            }
            if (stageCardName == "Foe")
            {
                if (stageCard.GetComponentInChildren<foeCardScript>().powerVariation)
                {
                    StagePower = StagePower + stageCard.GetComponentInChildren<foeCardScript>().higherPower;
                }
                else
                {
                    StagePower = StagePower + stageCard.GetComponentInChildren<foeCardScript>().power;
                }


            }
        }
        return StagePower; 
    }

    

    
}