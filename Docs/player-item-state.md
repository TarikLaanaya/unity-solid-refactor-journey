# Player Item State Script

This document showcases a new script I added, which essentially functions as a basic inventory system. This solution is far more elegant than my previous approach, where data was passed and managed across several unrelated scripts.

---

## 🔁 Old System vs New Inventory System

### ❌ Original
The original logic was messy and relied on hardcoded checks and copy-pasted inventory code scattered throughout the project:

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
To simplify this I created the `PlayerItemState` script which handles the player inventory:

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

Now I have two handy methods: `ClearItem()` and `SetItem()` which make managing the player’s inventory significantly cleaner and more intuitive.

---

## 📝 Final Notes

This refactor highlights the ***Single Responsibility Principle*** from the SOLID design principles. Making sure that no one script is pulling all the strings. By offloading inventory responsibility to a single script, I’ve made the system:

- Easier to update

- Easier to read

- Easier to reuse

If I want to tweak inventory behavior in the future, I only need to modify one class. No more duplicated logic or scattered inventory state. I genuinely feel as if I've just cleaned my room and have more space to think.

> Although currently my code's better off at an artisnal spaghetti competition. Lets just say i've cleaned a ***corner*** of my room.

I have recently started reading ***Game Programming Patterns by Robert Nystrom***. A great read and one of the things talked about is when to use these kinds of tools. When making a quick prototype worrying too much about following the ***SOLID Principles*** can seriously
slow down your progress. We essentially have to predict the future, setting up these systems to eventually be useful. One part of creating a ***SOLID System*** that I assume gets overlooked is learning what system actually needs to be SOLID. Things like an inventory
system, or an interaction system definitely can take advantage. Will that be the case for every other system? I think not. I look forward to finding that out for myself and discovering the full potential (and flaws) of the ***SOLID principles***.
