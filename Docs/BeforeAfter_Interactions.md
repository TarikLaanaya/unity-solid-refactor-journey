# Before & After – Interaction System Refactor

This document showcases changes from a `switch-based` interaction system
to an `interface-driven` system using **SOLID principles** — specifically focusing on the **Open/Closed Principle**.

---

## 🔁 Old Switch-Based System vs New Interface-Driven System

### ❌ Original Switch Statement
The original interaction logic relied on checking tag values in a growing switch block:

```csharp
switch (hit.transform.tag)
{
    case "ShawarmaKnife" :
        ActivateObjectOutline(hit.transform);
        if (CheckIfHoldingItem()) break;
        IsKnifeInteractedWith(hit.transform.gameObject, shawarmaKnife);
        break;

    case "ShawarmaStack" :
        ActivateObjectOutline(hit.transform);
        IsItemPickedUp(hit.transform);
        break;

    case "Lettuce" :
        ActivateObjectOutline(hit.transform);
        IsItemPickedUp(hit.transform);
        break;

    case "RedCabbage" :
        ActivateObjectOutline(hit.transform);
        IsItemPickedUp(hit.transform);
        break;

    case "Cucumbers" :
        ActivateObjectOutline(hit.transform);
        IsItemPickedUp(hit.transform);
        break;
    
    // ...it keeps going 👎👎
}
```
### ✅ New System
That approach was not very readable and hard to maintain. The first step to fix this was to create an interface script: `IInteractable`

```csharp
using UnityEngine;

public interface IInteractable
{
    void OnInteract();
}
```

This allows me to call the method: `OnInteract()` and trigger any classes which implement the `IInteractable` interface. As an example, I created a seperate script for the `"ShawarmaKnife"` switch case. I chose to call this script: `KnifePickUp`

```csharp
using UnityEngine;

public class KnifePickUp : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject knifeInHand;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerItemState playerItemState;
    
    public void OnInteract()
    {
        if (playerItemState.IsHoldingItem()) return;
        
        playerAnimator.SetBool("CloseHandKnife", true);
        gameObject.SetActive(false);
        knifeInHand.SetActive(true);

        playerItemState.SetItem("Knife", false);
    }
}
```

As you can notice, I implemented the `IInteractable` Interface into this script. So now when I call `OnInteract()` It will trigger. This essentially dissolves my switch case into this simple if statement:

```csharp
if(hit.transform.TryGetComponent<IInteractable>(out var interactable))
{
    ActivateObjectOutline(hit.transform);
    if (Input.GetKeyDown(KeyCode.Mouse0))
    {
        interactable.OnInteract();
    }
}
else
{
    DeactivateObjectOutline();
}
```
By doing this, the interaction logic becomes clean and decoupled.

---

## 📝 Final Notes

Using interfaces has significantly improved both the readability and scalability of my interaction system. Interfaces are a crucial and extremely useful part of the SOLID toolkit. Long gone are the days of sifting through a lengthy switch statement. We only need to check if the interactable object has an `IInteractable` script implemented, and call it.

Discovering Interfaces has been an amazing and eye-opening gateway into the world of programming patterns and im eager to find out more. This refactor marks a key step in my growth as a Unity developer — from just "making it work" to making it right.
