using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private Toggle togglePauseUnpause;
    [SerializeField]
    private Toggle[] toggleMagic;

    public Toggle[] ToggleMagic { get { return toggleMagic; } }

    [SerializeField]
    private int curToggleMagicID = -1;
    public RectTransform SelectionBox { get { return selectionBox; } }

    [SerializeField]
    private GameObject blackImage;

    [SerializeField]
    private GameObject inventoryPanel;
    [SerializeField]
    private GameObject grayImage;

    [SerializeField]
    private GameObject itemDialog;
    [SerializeField]
    private GameObject itemUIPrefab;

    [SerializeField]
    private GameObject[] slots;
    [SerializeField]
    private ItemDrag curItemDrag;

    [SerializeField]
    private int curSlotId;
    
    [SerializeField]
    private GameObject downPanel;
    [SerializeField]
    private GameObject npcDialoguePanel;
    [SerializeField]
    private Image npcImage;
    [SerializeField]
    private TMP_Text npcNameText;
    [SerializeField]
    private TMP_Text dialogueText;
    [SerializeField]
    private int index; //dialogue step

    [SerializeField]
    private GameObject btnNext;
    [SerializeField]
    private TMP_Text btnNextText;
    [SerializeField]
    private GameObject btnAccept;
    [SerializeField]
    private TMP_Text btnAcceptText;
    [SerializeField]
    private GameObject btnReject;
    [SerializeField]
    private TMP_Text btnRejectText;
    [SerializeField]
    private GameObject btnFinish;
    [SerializeField]
    private TMP_Text btnFinishText;
    [SerializeField]
    private GameObject btnNotFinish;
    [SerializeField]
    private TMP_Text btnNotFinishText;
    
    [SerializeField]
    private Toggle[] toggleAvatar;
    public Toggle[] ToggleAvatar { get { return toggleAvatar; } set { toggleAvatar = value; } }
    public static UIManager instance;

    void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void InitSlots()
    {
        for (int i = 0; i < InventoryManager.MAXSLOT; i++)
        {
            slots[i].GetComponent<InventorySlot>().ID = i;
        }
    }

// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    void Start()
    {
        InitSlots();
        MapToggleAvatar();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            togglePauseUnpause.isOn = !togglePauseUnpause.isOn;
        }
    }
    public void ToggleAI(bool isOn)
    {
        foreach (Character member in PartyManager.instance.Members)
        {
            AttackAI ai = member.gameObject.GetComponent<AttackAI>();
            if (ai != null)
            {
                ai.enabled = isOn;
            }
        }
    }
    public void SelectAll()
    {
        foreach (Character member in PartyManager.instance.Members)
        {
            if (member.CurHP > 0)
            {
                member.ToggleRingSelection(true);
                PartyManager.instance.SelectChars.Add(member);
            }
        }
    }

    public void PauseUnpause(bool isOn)
    {
        Time.timeScale = isOn ? 0 : 1;
    }
    public void ShowMagicToggles()
    {
        if (PartyManager.instance.SelectChars.Count <= 0)
            return;

        //Show Magic skill only the single selected hero
        Character hero = PartyManager.instance.SelectChars[0];

        for (int i = 0; i < hero.MagicSkills.Count; i++)
        {
            toggleMagic[i].interactable = true;
            toggleMagic[i].isOn = false;
            toggleMagic[i].GetComponentInChildren<Text>().text = hero.MagicSkills[i].Name;
            toggleMagic[i].targetGraphic.GetComponent<Image>().sprite = hero.MagicSkills[i].Icon;
        }
    }
    public void SelectMagicSkill(int i)
    {
        curToggleMagicID = i;
        PartyManager.instance.HeroSelectMagicSkill(i);
    }
    public void IsOnCurToggleMagic(bool flag)
    {
        toggleMagic[curToggleMagicID].isOn = flag;
    }
    public void ToggleInventoryPanel()
    {
        if (!inventoryPanel.activeInHierarchy)
        {
            inventoryPanel.SetActive(true);
            blackImage.SetActive(true);
            ShowInventory();
        }
        else
        {
            inventoryPanel.SetActive(false);
            blackImage.SetActive(false);
            ClearInventory();
        }
    }
    public void ClearInventory()
    {
        //Clear Slots
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].transform.childCount > 0)
            {
                Transform child = slots[i].transform.GetChild(0);
                Destroy(child.gameObject);
            }
        }
    }

    public void ShowInventory()
    {
        if (PartyManager.instance.SelectChars.Count <= 0)
            return;

        //Show Inventory only the single selected hero
        Character hero = PartyManager.instance.SelectChars[0];

        //Show items
        for (int i = 0; i < InventoryManager.MAXSLOT; i++)
        {
            if (hero.InventoryItems[i] != null)
            {
                GameObject itemObj = Instantiate(itemUIPrefab, slots[i].transform);
                ItemDrag itemDrag = itemObj.GetComponent<ItemDrag>();
                itemDrag.UIManager = this;
                itemDrag.Item = hero.InventoryItems[i];
                itemDrag.IconParent = slots[i].transform;
                itemDrag.Image.sprite = hero.InventoryItems[i].Icon;
            }
        }
    }
    public void SetCurItemInUse(ItemDrag itemDrag, int index)
    {
        curItemDrag = itemDrag;
        curSlotId = index;
    }

    public void ToggleItemDialog(bool flag)
    {
        grayImage.SetActive(flag);
        itemDialog.SetActive(flag);
    }

    public void DeleteItemIcon()
    {
        Destroy(curItemDrag.gameObject);//destroy Icon
    }

    public void ClickDrinkConsumable() //Map with Button "Use"
    {
        InventoryManager.instance.DrinkConsumableItem(curItemDrag.Item, curSlotId);
        DeleteItemIcon();
        ToggleItemDialog(false);
    }
    private void ClearDialogueBox()
    {
        npcImage.sprite = null;
        npcNameText.text = "";
        dialogueText.text = "";

        btnNextText.text = "";
        btnNext.SetActive(false);

        btnAcceptText.text = "";
        btnAccept.SetActive(false);

        btnRejectText.text = "";
        btnReject.SetActive(false);

        btnFinishText.text = "";
        btnFinish.SetActive(false);

        btnNotFinishText.text = "";
        btnNotFinish.SetActive(false);
    }

    private void StartQuestDialogue(Quest quest)
    {
        dialogueText.text = quest.QuestDialogue[index];
        btnNext.SetActive(true);
        btnNextText.text = quest.AnswerNext[index];

        btnAccept.SetActive(false);
        btnReject.SetActive(false);
    }
    private void SetupDialoguePanel(Npc npc)
    {
        index = 0;

        npcImage.sprite = npc.AvatarPic;
        npcNameText.text = npc.CharName;

        Quest inProgressQuest = QuestManager.instance.CheckForQuest(npc, QuestStatus.InProgress);
        if (inProgressQuest != null) //There is an In-Progress Quest going on
        {
            Debug.Log($"In-progress: {inProgressQuest}");
            dialogueText.text = inProgressQuest.QuestionInProgress;

            bool hasItem = QuestManager.instance.CheckIfFinishQuest();
            Debug.Log(hasItem);

            if (hasItem) //has item to finish quest
            {
                btnFinishText.text = inProgressQuest.AnswerFinish;
                btnFinish.SetActive(true);
            }
            else
            {
                btnNotFinishText.text = inProgressQuest.AnswerNotFinish;
                btnNotFinish.SetActive(true);
            }
        }
        else //Check for New Quest
        {
            Quest newQuest = QuestManager.instance.CheckForQuest(npc, QuestStatus.New);
            //Debug.Log(newQuest);

            if (newQuest != null) //There is a new Quest
                StartQuestDialogue(newQuest);
        }
    }

    private void ToggleDialogueBox(bool flag)
    {
        downPanel.SetActive(!flag);
        npcDialoguePanel.SetActive(flag);
        togglePauseUnpause.isOn = flag;
    }
    public void PrepareDialogueBox(Npc npc)
    {
        ClearDialogueBox();
        SetupDialoguePanel(npc);
        ToggleDialogueBox(true);
    }
    public void AnswerNext() //map with ButtonNext
    {
        index++;
        dialogueText.text = QuestManager.instance.NextDialogue(index);

        if (QuestManager.instance.CheckLastDialogue(index)) //last dialogue
        {
            btnNext.SetActive(false);

            btnAcceptText.text = QuestManager.instance.CurQuest.AnswerAccept;
            btnAccept.SetActive(true);

            btnRejectText.text = QuestManager.instance.CurQuest.AnswerReject;
            btnReject.SetActive(true);
        }
        else
        {
            btnNext.SetActive(true);
            btnNextText.text = QuestManager.instance.CurQuest.AnswerNext[index];
        }
    }
    public void AnswerReject() //map with ButtonReject
    {
        QuestManager.instance.RejectQuest();
        ToggleDialogueBox(false);
    }

    public void AnswerAccept() //map with ButtonAccept
    {
        QuestManager.instance.AcceptQuest();
        ToggleDialogueBox(false);
    }
    public void AnswerFinish() //map with ButtonFinish
    {
        Debug.Log("Can Finish Quest");
        bool success = QuestManager.instance.DeliverItem();

        if (success)
        {
            if (QuestManager.instance.NpcGiveReward())
            {
                Debug.Log("Quest Completed");
                ToggleDialogueBox(false);
            }
        }
    }

    public void AnswerNotFinish() //map with ButtonNotFinish
    {
        Debug.Log("Cannot Finish Quest");
        ToggleDialogueBox(false);
    }
    public void MapToggleAvatar()
    {
        foreach (Toggle t in toggleAvatar)
            t.gameObject.SetActive(false);

        for (int i = 0; i < PartyManager.instance.Members.Count; i++)
        {
            toggleAvatar[i].gameObject.SetActive(true);
        }
        toggleAvatar[0].isOn = true; //Select first hero
    }
    public void SelectHeroByAvatar(int i) //map with toggle
    {
        if (toggleAvatar[i].isOn)
        {
            //Debug.Log($"is On: {i}");
            PartyManager.instance.SelectSingleHeroByToggle(i);
        }
        else //isOn is false
        {
            //Debug.Log($"is Off: {i}");
            PartyManager.instance.UnSelectSingleHeroByToggle(i);
        }
    }
}
