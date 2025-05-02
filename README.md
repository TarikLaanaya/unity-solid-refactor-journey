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
    // ...many more (atleasst 15 😬😅)
}
```

### ✅ After (interface-based interaction):
```csharp
if (hit.transform.TryGetComponent<IInteractable>(out var interactable)) {
    interactable.OnInteract();
}
```
---

## 📚 SOLID Principles Practiced

| Principle                | Applied?                                                           | How |
| ------------------------ | ------------------------------------------------------------------ | --- |
| Single Responsibility ✅  | Item state, animation, and interaction split into separate classes |     |
| Open/Closed ✅            | Interactions extended via interfaces, not modified                 |     |
| Liskov Substitution 🟡   | Planning to implement more reusable object types                   |     |
| Interface Segregation 🟡 | Just starting with basic `IInteractable`, will expand              |     |
| Dependency Inversion 🟡  | Future goal (event-based architecture, scriptable objects)         |     |

---

## 📈 What I’m Learning

* How to refactor real Unity gameplay code using SOLID

* How to separate logic into clean systems

* How to design scalable interaction systems

* How to document and share my dev journey
