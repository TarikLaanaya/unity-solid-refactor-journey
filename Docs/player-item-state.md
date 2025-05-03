# Player Item State Script

This document showcases a new script I added which essentially works as an **Inventory System**. This solution was a lot more elegant compared to when I was previously **Reading** and **Writing** this information from various scripts.

---

## 🔁 Old System vs New Inventory System

### ❌ Original
The original logic was messy and meant copy and pasting code relating to player inventory:

```csharp
if (Input.GetKeyDown(KeyCode.Mouse0))
{
    if(useItemScript.itemInHand == "PackagedWrap")
    {
        //Do stuff...

        useItemScript.isItemInHand = false;
        useItemScript.itemInHand = null;
    }
}
```
### ✅ New System
In order to simplify this I created the `PlayerItemState` script to handle the player inventory:

```csharp
public class PlayerItemState : MonoBehaviour
{
    public string itemInHand;
    public bool isItemInHand;
    public bool itemInUse;
    public bool holdingFood;

    public bool IsHoldingItem() => isItemInHand;

    public void ClearItem()
    {
        itemInHand = null;
        isItemInHand = false;
        itemInUse = false;
        holdingFood = false;
    }

    public void SetItem(string newItem, bool isFoodItem)
    {
        itemInHand = newItem;
        isItemInHand = true;
        itemInUse = false;
        holdingFood = isFoodItem;
    }
}
```

Now I have two handy methods: `ClearItem()` and `SetItem()` which allow me to easily manage inventory

---

## 📝 Final Notes

This represents the ***Single Responsibility*** principle from the SOLID design principles. Making sure that no one script is pulling all the strings. By separating my code into the `PlayerItemState` script, If I wanted to make a change to the inventory behavior I would
only have to edit that one script. This also means I have less work to do regarding copying and pasting code. The final benefit is the general readability, I genuinely feel as if I've just cleaned my room and have more space to think.

> Although currently my code's better off at an artisnal spaghetti competition. Lets just say i've cleaned a ***corner*** of my room.

I have recently started reading ***Game Programming Patterns by Robert Nystrom***. A great read and one of the things talked about is when to use these kinds of tools. When making a quick prototype worrying too much about following the ***SOLID Principles*** can seriously
slow down your progress. We essentially have to predict the future, setting up these systems to eventually be useful. One part of creating a ***SOLID System*** that I assume gets overlooked is learning what system actually needs to be SOLID. Things like an inventory
system, or an interaction system definitely can take advantage. Will that be the case for every other system? I think not. I look forward to finding that out for myself and discovering the full potential (and flaws) of the ***SOLID principles***.
