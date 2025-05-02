# unity-solid-refactor-journey
This repo documents my journey improving a Unity project by applying **SOLID principles** and general clean code architecture techniques.

---

## 🎯 Goal

Take an existing spaghetti-style gameplay system and refactor it using best practices to make the project more modular, readable, and scalable.

---

## 🛠️ Changes Made

### ✅ Implemented Interface-Driven Interactions
- Replaced giant `switch` statements with `IInteractable` interface system.
- Each object now handles its own logic, reducing coupling.

### ✅ Created `PlayerItemState` Manager
- Centralized all "item in hand" logic (SRP-compliant).
- Makes inventory state globally accessible and modular.

### ✅ Working Toward Modular Animation Controller
- Avoiding repeated animation bool setting
- Aiming for fully encapsulated `PlayerAnimatorController`

---

## 🔁 Before vs After

### ❌ Before (switch-based interaction):
```csharp
switch (hit.transform.tag) {
    case "Knife":
        IsKnifeInteractedWith();
        break;
    // ...many more
}
```

### ✅ After (interface-based interaction):
```csharp
if (hit.transform.TryGetComponent<IInteractable>(out var interactable)) {
    interactable.OnInteract();
}
```
