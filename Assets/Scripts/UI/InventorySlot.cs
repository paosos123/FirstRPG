using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private int id;
    public int ID { get { return id; } set { id = value; } }
    [SerializeField]
    private ItemType itemType;
    public ItemType ItemType { get { return itemType; } set { itemType = value; } }
    [SerializeField]
    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = InventoryManager.instance;
    }
   /* public void OnDrop(PointerEventData eventData)
    {
        //This instance is Slot B)
        //Get Item A
        GameObject objA = eventData.pointerDrag;
        ItemDrag itemDragA = objA.GetComponent<ItemDrag>();
        InventorySlot slotA = itemDragA.IconParent.GetComponent<InventorySlot>();

        if (itemType == ItemType.Shield) // เพิ่ม
        {
            if (itemDragA.Item.Type != ItemType.Shield) // เพิ่ม
                return;
        }

        //There is an Item B in Slot B
        if (transform.childCount > 0)
        {
            //Get Item B
            GameObject objB = transform.GetChild(0).gameObject;
            ItemDrag itemDragB = objB.GetComponent<ItemDrag>();

            if (slotA.ItemType == ItemType.Shield) // เพิ่ม
            {
                if (itemDragB.Item.Type != slotA.ItemType) // เพิ่ม
                    return;
            }

            //Remove Item A from Slot A // ย้ายมา
            inventoryManager.RemoveItemInBag(slotA.ID);

            //Set Item B on Slot A
            itemDragB.transform.SetParent(itemDragA.IconParent);
            itemDragB.IconParent = itemDragA.IconParent;
            inventoryManager.SaveItemInBag(slotA.ID, itemDragB.Item);

            //Remove Item B from Slot B // เพิ่ม
            inventoryManager.RemoveItemInBag(id);
        }
        else //Slot B is blank // ย้ายมา/เพิ่ม else
        {
            //Remove Item A from Slot A
            inventoryManager.RemoveItemInBag(slotA.ID);

            //Set Item A on Slot B
            itemDragA.IconParent = transform;
            inventoryManager.SaveItemInBag(id, itemDragA.Item);
        }
    }*/
   public void OnDrop(PointerEventData eventData)
   {
       //This instance is Slot B)
       //Get Item A
       GameObject objA = eventData.pointerDrag;
       ItemDrag itemDragA = objA.GetComponent<ItemDrag>();
       InventorySlot slotA = itemDragA.IconParent.GetComponent<InventorySlot>();

       if (itemType == ItemType.Shield)
       {
           if (itemDragA.Item.Type != ItemType.Shield)
               return;
       }
       else if (itemType == ItemType.Weapon)
       {
           if (itemDragA.Item.Type != ItemType.Weapon)
               return;
       }

       //There is an Item B in Slot B
       if (transform.childCount > 0)
       {
           //Get Item B
           GameObject objB = transform.GetChild(0).gameObject;
           ItemDrag itemDragB = objB.GetComponent<ItemDrag>();

           if (slotA.ItemType == ItemType.Shield)
           {
               if (itemDragB.Item.Type != slotA.ItemType)
                   return;
           }
           else if (slotA.ItemType == ItemType.Weapon)
           {
               if (itemDragB.Item.Type != slotA.ItemType)
                   return;
           }

           //Remove Item A from Slot A // ย้ายมา
           inventoryManager.RemoveItemInBag(slotA.ID);

           //Set Item B on Slot A
           itemDragB.transform.SetParent(itemDragA.IconParent);
           itemDragB.IconParent = itemDragA.IconParent;
           inventoryManager.SaveItemInBag(slotA.ID, itemDragB.Item);

           //Remove Item B from Slot B // เพิ่ม
           inventoryManager.RemoveItemInBag(id);
       }
       else //Slot B is blank // ย้ายมา/เพิ่ม else
       {
           //Remove Item A from Slot A
           inventoryManager.RemoveItemInBag(slotA.ID);

           //Set Item A on Slot B
           itemDragA.IconParent = transform;
           inventoryManager.SaveItemInBag(id, itemDragA.Item);
       }
   }
}