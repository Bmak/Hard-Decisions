using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameCtrl : MonoBehaviour {

	[Header("UI References")]
	public Transform panelQueue;
	public Transform panelCardsTraits;
	public Transform panelCardsLoot;
    public Transform[] cardSlots;
	public Transform[] gameUI;
	public Transform heartPool;
	public Text verdictText;
	public Button btnStartNewDay;
	public Button btnLetIn;
	public Button btnKickOut;
	public GameObject prefabHeartUI;
	
	[Header("Game Settings")]
    public int refugeesDailyAve = 4;
    public int refugeesDailyFlux = 1;
    public int infectedDailyAve = 2;
    public int infectedDailyFlux = 0;
           int infectedDailyCurrent;
	public int heartsAmount = 2;
    public int cardsDrawnDaily = 4;
	public int refugeeCardsOpenLimit = 2;
	// private int cardsOpenedCurrent = 0;
    private List<GameObject> refugeeCardsOpened = new List<GameObject>();
	public CharacterScrObj[] playingCharsList;
	[HideInInspector]
    public CharController currentCharacter = null;


    [Header("Gen: Settings")]
    public int traitCardsLimit = 5;
    public int lootCardsLimit = 3;
    [Header("Gen/Set: Infection")]
    public float infectionChance = 0.5f;
    public float fakeHealthCardChance = 0.12f;
    public float healthyCardChance = 0.12f; 

    [Header("Gen: Reference")]
	public GameObject prefabCharUI = null;
	public GameObject prefabCardUI = null;
	public CharacterScrObj[] charInfoList;
    public CardScrObj[] traitCards;
    public CardScrObj[] backCards;
    public CardScrObj[] lootCards;
    public string[] charFirstNamesM = {"Jack","John","Mike","Alex","Sam","Samuel","Chris","Marc","Vick","Cole","TJ","Cory","Krzysztof","Ismael","Tyler","Damian","Lito","Kirk","Rupert"};
    public string[] charFirstNamesF = {"Jill","Jane","Michelle","Alex","Samantha","Sam","Kris","Mary","Vicky","Carol","Liz","Maya","Miriam","Gabriela","Bettany","Selma","Greta","Lucy","Casey"};
    public string[] charLastName = {"Smith","Cooper","Johnson","Galestein","Sadowski","Nichiporchik","DeSouza","Ishimura","Lee","Gonzalez","Kolonski","Shemberg","Boon","Wisemahn","White","Scott","Shazell","Palmer","Kruchek"};
	public Sprite[] portraitsMale;
	public Sprite[] portraitsFemale;
    [Header("Gen/Ref: Infection")]
    public CardScrObj infectedCard = null;
	public CardScrObj healthyCard = null;
    public CardScrObj[] healthCards;


    bool queueEmpty = false;
    bool handEmpty = false;


    void Start () 
	{
        SetGameUIActive(false);
		//CleanTheTable();
	}


    // START NEW DAY====================================================================================================

    public void StartNewDay()
    {
        // 1. CleanTheTable
        // 2. Draw Cards To Hand
        // 3. GenerateDailyRefugees
        // 3. SpawnCharUI
        // 4. ShowGameUI (counters, hearts)

        btnStartNewDay.interactable = false;

        print(name + ": Started Game; Ready to Clean The Table");

        // StartGame-1.
        CleanTheTable();
    }

	void CleanTheTable()
    {
        CleanCharPanel();
        CleanCardPanels();

        print(name + ": Table Clean; Ready to Start New Day");

        // StartGame-2.
        GetComponent<DeckCtrl>().DrawCardsToHand(cardsDrawnDaily);
        // StartGame-3.
        GenerateDailyRefugees();
    }

    public void GenerateDailyRefugees() 
	{
        playingCharsList = new CharacterScrObj[Random.Range(
            refugeesDailyAve-refugeesDailyFlux, 
            refugeesDailyAve+refugeesDailyFlux
            )];

        infectedDailyCurrent = Random.Range(
            infectedDailyAve-infectedDailyFlux, 
            infectedDailyAve+infectedDailyFlux
            );

        for (int i = 0; i < playingCharsList.Length; i++)
        {
            if (playingCharsList.Length - 1 - i <= infectedDailyCurrent) infectionChance = 1.0f;

            CharacterScrObj newChar = GenerateCharacter();
            playingCharsList[i] = newChar;
        }

        print(name + ": Playing Characters Generated; Ready to Spawn Char Cards");

        // StartGame-4.
        SpawnCharUI();
    }

	public void SpawnCharUI()
    {

        for (int i = 0; i < playingCharsList.Length; i++)
        {
            GameObject character = GameObject.Instantiate(prefabCharUI, Vector3.zero, Quaternion.identity, panelQueue);
            character.name = "Character_" + i.ToString();

            character.GetComponent<CharController>().charInfo = playingCharsList[i];
			character.GetComponent<CharController>().DisplayCard();

        }

        SetCharCardsInteractable(false, exceptionIndexList: new int[] {panelQueue.childCount-1});

        print(name + ": Char Cards Spawned; Ready to Show Game UI");

        // StartGame-5.
        ShowGameUI();

    }


    private void ShowGameUI()
    {
        SetHeartAmount(heartsAmount);
		
		SetGameUIActive(true);
        SetCharButtonsInteractable(false);

        print(name + ": Game UI Shown; Standing by for player");

    }

    public void OnCharCardOpen(GameObject openCard)
    {

        // memorize the current character
        currentCharacter = openCard.GetComponent<CharController>();

        // Disable the rest of the Char Cards
        // int openCardIndex = openCard.transform.GetSiblingIndex();
		int[] openCardIndexList = {openCard.transform.GetSiblingIndex()};
        SetCharCardsInteractable(false, openCardIndexList);

        // Spawn this character's trait and loot cards
        CharacterScrObj charInfo = openCard.GetComponent<CharController>().charInfo;

        for (int i = 0; i < charInfo.cardsTraits.Count; i++)
        {
            GameObject card = GameObject.Instantiate(prefabCardUI, Vector3.zero, Quaternion.identity, panelCardsTraits);
			card.transform.SetSiblingIndex(Random.Range(0,card.transform.parent.childCount));

            card.GetComponent<CardController>().cardInfo = charInfo.cardsTraits[i];
            card.GetComponent<CardController>().DisplayCard();
            openCard.GetComponent<CharController>().myExistingCards.Add(card.GetComponent<CardController>());

            card.name = "CardTrait_" + card.GetComponent<CardController>().cardInfo.name;
        }
        print(name + ": Trait Cards for " + openCard.name + " built; Ready to build loot");

        for (int i = 0; i < charInfo.cardsLoot.Count; i++)
        {
            GameObject card = GameObject.Instantiate(prefabCardUI, Vector3.zero, Quaternion.identity, panelCardsLoot);
            card.transform.SetSiblingIndex(Random.Range(0, card.transform.parent.childCount));

            card.GetComponent<CardController>().cardInfo = charInfo.cardsLoot[i];
            card.GetComponent<CardController>().DisplayCard();
            openCard.GetComponent<CharController>().myExistingCards.Add(card.GetComponent<CardController>());


            card.name = "CardLoot_" + card.GetComponent<CardController>().cardInfo.name;
        }

		refugeeCardsOpened.Clear();
        print(name + ": Loot Cards for " + openCard.name + " built; Standing by");

        // Activate LetIn/KickOut buttons
        SetCharButtonsInteractable(true);

    }

    public void OnCardOpen (GameObject card) {
		refugeeCardsOpened.Add(card);
		if (refugeeCardsOpened.Count >= refugeeCardsOpenLimit) {
			SetCloseCardsInteractable(false);
		}
	}

	public void LetCurrentCharIn() {
		if (currentCharacter == null) return;

		print(name + ": Letting " + currentCharacter.charInfo.firstName + " in!");

		SetCharButtonsInteractable(false);

		string verdict;
		int hearts;
		if (currentCharacter.charInfo.infected) {
			verdict = currentCharacter.charInfo.firstName + " is infected. They take from your cause.";
			hearts = heartsAmount-1;
		} else {
            verdict = currentCharacter.charInfo.firstName + " is healthy. They add to your cause.";
			hearts = heartsAmount+1;
        }

		GetComponent<CardManager>().AllocateNewCharsCards(new List<CharController> {currentCharacter});
		SetCharCardsInteractable(false, exceptionIndexList: new int[] {panelQueue.childCount-1});
        //GameObject.Destroy(currentCharacter.gameObject);
		//CleanCardPanels();
        currentCharacter = null;

        verdictText.text = verdict;
		SetHeartAmount(hearts);
        IsNewDayReady();
    }

	public void KickCurrentCharOut() {
        if (currentCharacter == null) return;

        print(name + ": Kicking " + currentCharacter.charInfo.firstName + " out!");

        SetCharButtonsInteractable(false);

        string verdict = "Fate of " + currentCharacter.charInfo.firstName + " remains unknown to you.";

        // Deleting the chars cards from the checkpoint's panels explicitly
        // (because cleaning them now does other things)
        currentCharacter.transform.SetParent(panelQueue.parent);
        Destroy(currentCharacter.gameObject);
        for (int i = 0; i < panelCardsTraits.childCount; i++) {
            Destroy(panelCardsTraits.GetChild(i).gameObject);
        }
        for (int i = 0; i < panelCardsLoot.childCount; i++) {
            Destroy(panelCardsLoot.GetChild(i).gameObject);
        }

        SetCharCardsInteractable(false, exceptionIndexList: new int[] { panelQueue.childCount-1 });
        currentCharacter = null;

        verdictText.text = verdict;
        IsNewDayReady();
    }

    public void IsNewDayReady() {
        queueEmpty = (panelQueue.childCount == 0);
        handEmpty = (GetComponent<DeckCtrl>().handCardholder.childCount == 0);

        btnStartNewDay.interactable = (queueEmpty);
    }

    private void EndGame()
    {
		// SetCharCardsInteractable(false);
        // SetCharButtonsInteractable(false);

		CleanTheTable();

        string verdict = "The infected have taken your ship. RELOAD. RELOAD. RELOAD. RELOAD. ";
        verdictText.text = verdict;
    }

    // UTILS ==============================================================================================================

    private void SetGameUIActive(bool value)
    {
		foreach (var t in gameUI) {
            t.gameObject.SetActive(value);
        }

        // btnLetIn.gameObject.SetActive(value);
        // btnKickOut.gameObject.SetActive(value);
    }

	private void SetHeartAmount(int amount){
		heartsAmount = amount;
		
		for (int i = 0; i < heartPool.childCount; i++)
		{
			GameObject.Destroy(heartPool.GetChild(i).gameObject);
		}

		if (heartsAmount <= 0)
        {
            EndGame();
        } else {
            for (int j = 0; j < heartsAmount; j++)
            {
                GameObject.Instantiate(prefabHeartUI, Vector3.zero, Quaternion.identity, heartPool);
            }
        }
	}

    public void SetCharButtonsInteractable(bool value)
	{
		btnLetIn.interactable = value;
		btnKickOut.interactable = value;
	}

    private void SetCharCardsInteractable(bool value, int[] exceptionIndexList = null)
    {
        for (int i = 0; i < panelQueue.childCount; i++)
        {
            if (exceptionIndexList == null) {
                panelQueue.GetChild(i).GetComponent<Button>().interactable = value;

            } else {
                if (!exceptionIndexList.Contains(i))
                {
                    panelQueue.GetChild(i).GetComponent<Button>().interactable = value;
                }
                else if (exceptionIndexList.Contains(i))
                {
                    print(name + ": Char Card '" + panelQueue.GetChild(i).gameObject.name + "' is exceptional!");
                    // set opposite of the value
                    panelQueue.GetChild(i).GetComponent<Button>().interactable = !value;
                }

            }
        }
		exceptionIndexList = null;
    }

	private void SetCloseCardsInteractable(bool value) {
        for (int i = 0; i < panelCardsLoot.childCount; i++)
        {
            var t = panelCardsLoot.GetChild(i);
            var f = t.GetComponent<Flippable>();
            var b = t.GetComponent<Button>();

            if (f.enabled && f.currentlyClosed)
            {
                b.interactable = value;
            }
        }
        for (int i = 0; i < panelCardsTraits.childCount; i++)
        {
            var t = panelCardsTraits.GetChild(i);
            var f = t.GetComponent<Flippable>();
            var b = t.GetComponent<Button>();

            if (f.enabled && f.currentlyClosed)
            {
                b.interactable = value;
            }
        }
    }

    private void CleanCharPanel() {

        List<CharController> chars = new List<CharController>();
        chars.AddRange(panelQueue.GetComponentsInChildren<CharController>());

        GetComponent<UnitManager>().SendCharsToHold(chars);
        
        // for (int i = 0; i < panelQueue.childCount; i++) {
        //     Destroy(panelQueue.GetChild(i).gameObject);
        // }

    }

    private void CleanCardPanels() {
        List<CardController> toDiscard = new List<CardController>();

        foreach (var s in cardSlots) {
            toDiscard.AddRange(s.GetComponentsInChildren<CardController>());
        }

        GetComponent<DeckCtrl>().SendCardsToDeck(toDiscard, true);

        // toDiscard.AddRange(panelCardsTraits.GetComponentsInChildren<CardController>());
        // toDiscard.AddRange(panelCardsLoot.GetComponentsInChildren<CardController>());

        // for (int i = 0; i < panelCardsTraits.childCount; i++)
        // {
        //     Destroy(panelCardsTraits.GetChild(i).gameObject);
        // }
        // for (int i = 0; i < panelCardsLoot.childCount; i++)
        // {
        //     Destroy(panelCardsLoot.GetChild(i).gameObject);
        // }
    }

    public void ReloadGame() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    // GEN ================================================================================================================

    public CharacterScrObj GenerateCharacter() 
	{
		//CharacterScrObj tempChar = CharacterScrObj.CreateInstance<CharacterScrObj>();
		//tempChar.CopyFrom(charInfoList[Random.Range(0, charInfoList.Length)]);

        CharacterScrObj tempChar = charInfoList[Random.Range(0, charInfoList.Length)].DeepCopy();

		// Randomize sex and name
		bool male = (Random.value <= 0.5f);
		tempChar.firstName = (male) 
			? charFirstNamesM[Random.Range(0, charFirstNamesM.Length)] 
			: charFirstNamesF[Random.Range(0, charFirstNamesF.Length)];
		tempChar.lastName = charLastName[Random.Range(0, charLastName.Length)];

        // Randomize artwork
        tempChar.artwork = (male)
            ? portraitsMale[Random.Range(0, portraitsMale.Length)]
            : portraitsFemale[Random.Range(0, portraitsFemale.Length)];

        // Randomize infection
        tempChar.infected = (infectedDailyCurrent > 0) ? (Random.value <= infectionChance) : false;
		if (tempChar.infected) 
		{
            infectedDailyCurrent--;
			print(name + ": " + tempChar.firstName + " is INFECTED! Infected left: " + infectedDailyCurrent.ToString());

			// Randomize loot and traits because of infection
			for (int i = 0; i < tempChar.cardsLoot.Count; i++) {
				tempChar.cardsLoot[i] = lootCards[Random.Range(0, lootCards.Length)];
			}

            tempChar.cardsTraits[0] = (Random.value > fakeHealthCardChance) 
                ? infectedCard 
                : ProBro.PickRandom<CardScrObj>(healthCards);
            tempChar.cardsTraits[1] = backCards[Random.Range(0, backCards.Length)];
            tempChar.cardsTraits[2] = traitCards[Random.Range(0, traitCards.Length)];
            tempChar.cardsTraits[3] = traitCards[Random.Range(0, traitCards.Length)];
            tempChar.cardsTraits[4] = traitCards[Random.Range(0, traitCards.Length)];
        } else {
            print(name + ": " + tempChar.firstName + " is healthy! Infected left: " + infectedDailyCurrent.ToString());
            //tempChar.cardsTraits[0] = healthyCard;
            tempChar.cardsTraits[0] = (Random.value < healthyCardChance)
                ? healthyCard
                : ProBro.PickRandom<CardScrObj>(healthCards);

        }

        // trim the traits cards by infoCardsLimit
        //print(name + ": TRAIT CARDS; length: " + tempChar.cardsTraits.Length);
        //foreach (var card in tempChar.cardsTraits) {
            //print("TRAIT \"" + card.name + "\"");
        //}
        //print(name + ": >>>>> shuffling traits >>>>>");
        
        CardScrObj[] onlyTraits = new CardScrObj[tempChar.cardsTraits.Count-2];
        for (int i = 0; i < onlyTraits.Length; i++) {
            onlyTraits[i] = tempChar.cardsTraits[i+2];
        }
        ProBro.ShuffleArray(onlyTraits).CopyTo(tempChar.cardsTraits.ToArray(), 2);

        // print(name + ": TRAIT CARDS; length: " + tempChar.cardsTraits.Length);
        // foreach (var card in tempChar.cardsTraits) {
            //print("TRAIT \"" + card.name + "\"");
        // }
        // print(name + ": >>>>> trimming >>>>>");

        while (tempChar.cardsTraits.Count > traitCardsLimit) {
            int lastIndex = tempChar.cardsTraits.Count - 1;
            tempChar.cardsTraits.RemoveAt(lastIndex);
        }

        // print(name + ": TRAIT CARDS; length: " + tempChar.cardsTraits.Length);
        // foreach (var card in tempChar.cardsTraits) {
            //print("TRAIT \"" + card.name + "\"");
        // }

        // trim the loot cards by lootCardsLimit
        // print(name + ": LOOT CARDS; length: " + tempChar.cardsLoot.Length);
        // foreach (var card in tempChar.cardsLoot) {
            //print("LOOT \"" + card.name + "\"");
        // }
        // print(name + ": >>>>> shuffling & trimming >>>>>");

        ProBro.ShuffleArray(tempChar.cardsLoot.ToArray());
        while (tempChar.cardsLoot.Count > lootCardsLimit)
        {
            int lastIndex = tempChar.cardsLoot.Count - 1;
            tempChar.cardsLoot.RemoveAt(lastIndex);
        }

        // print(name + ": LOOT CARDS; length: " + tempChar.cardsLoot.Length);
        // foreach (var card in tempChar.cardsLoot) {
            // print("LOOT \"" + card.name + "\"");
        // }


        // return the generated character
        return tempChar;
	}

}
