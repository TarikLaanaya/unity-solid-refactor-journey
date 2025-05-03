using System.Collections;
using System.Collections.Generic;
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
