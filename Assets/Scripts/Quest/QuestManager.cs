using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private Npc[] npcPerson;
    public Npc[] NPCPerson { get { return npcPerson; } set { npcPerson = value; } }

    [SerializeField]
    private QuestData[] questData;
    public QuestData[] QuestData { get { return questData; } set { questData = value; } }

    [SerializeField]
    private Npc curNpc;
    public Npc CurNpc { get { return curNpc; } set { curNpc = value; } }

    [SerializeField]
    private Quest curQuest;
    public Quest CurQuest { get { return curQuest; } set { curQuest = value; } }

    public static QuestManager instance;

    void Awake()
    {
        instance = this;
    }
    private void AddQuestToNPC(Npc npc, QuestData questData)
    {
        Quest quest = new Quest(questData);
        npc.QuestToGive.Add(quest);
    }

// Start is called once before the first execution of Update after the MonoBehaviour
    void Start()
    {
        AddQuestToNPC(npcPerson[0], questData[0]); //Give Golem - Give Potion Quest
    }
    public Quest CheckForQuest(Npc npc, QuestStatus status)
    {
        curNpc = npc;
        Quest quest = npc.CheckQuestList(status);
        curQuest = quest;
        return quest;
    }

    private bool CheckItemToDelivery()
    {
        return InventoryManager.instance.CheckPartyForItem(curQuest.QuestItemId);
    }

    public bool CheckIfFinnishQuest()
    {
        bool success = false;
        Debug.Log(curQuest.Type);

        switch(curQuest.Type)
        {
            case QuestType.Delivery:
                success = CheckItemToDelivery();
                break;
        }
        return success;
    }
}