using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemScript : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject[] foodPiecesForWrap;

    [HideInInspector]
    public string itemInHand;
    [HideInInspector]
    public bool isItemInHand;

    [HideInInspector]
    public bool itemInUse;
    private SkillCheckScript skillCheckScript;

    [HideInInspector]
    public bool skillCheckComplete;

    [HideInInspector]
    public bool tryingToPlaceInWrap;

    [HideInInspector]
    
    [SerializeField] private GameObject playerHand;
    [SerializeField] private Transform stickOutTarget;
    [SerializeField] private Transform handOriginalPos;
    public float cupSize;
    private bool stickingOutCup;

    [HideInInspector]
    public GameObject cup;
    private LTDescr moveTween;

    [HideInInspector]
    public List<String> foodPiecesInsideWrap;
    private PlayerPickUpScript playerPickUpScript;
    private PlayerItemState playerItemState;

    void Start()
    {
        skillCheckScript = GetComponent<SkillCheckScript>();
        foodPiecesInsideWrap = new List<String>();
        playerPickUpScript = GetComponent<PlayerPickUpScript>();
        playerItemState = GetComponent<PlayerItemState>();
    }

    void Update()
    {
        bool leftClick = Input.GetKeyDown(KeyCode.Mouse0);
        bool rightClick = Input.GetKeyDown(KeyCode.Mouse1);

        //print(itemInUse);

        if ((playerItemState.IsHoldingItem() && playerItemState.itemInHand != null && (leftClick || rightClick)) || tryingToPlaceInWrap)
        {
            switch (playerItemState.itemInHand)
            {
                case "Knife":
                    swingKnife(rightClick);
                    break;
                
                case "ShawarmaStack" :
                    PlaceFoodInWrap(rightClick);
                    break;

                case "Lettuce" :
                    PlaceFoodInWrap(rightClick);
                    break;

                case "RedCabbage" :
                    PlaceFoodInWrap(rightClick);
                    break;

                case "Cucumbers" :
                    PlaceFoodInWrap(rightClick);
                    break;

                case "Tomatoes" :
                    PlaceFoodInWrap(rightClick);
                    break;

                case "Weed" :
                    HoldingDrugs(rightClick);
                    break;
                
                case "PackagedWrap" :
                    
                    break;
                
                case "Cup" :
                    cup = playerPickUpScript.cup;
                    StickOutCup(rightClick);
                    break;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && playerItemState.itemInHand == "Cup")
        {
            BringCupBackIn();
        }

        if (stickingOutCup) //Handle the cup sticking out of the player's hand and moving it out of the way of objects
        {
            Vector3 direction = (stickOutTarget.position - handOriginalPos.position).normalized;
            int layerToIgnore = LayerMask.NameToLayer("Player");
            int layerMask = ~(1 << layerToIgnore);

            Collider[] colliders = Physics.OverlapSphere(stickOutTarget.position, 0.08f, layerMask);
            if (colliders.Length > 0)
            {
                // Move the hand to the nearest point outside the object
                foreach (Collider collider in colliders)
                {
                    Vector3 closestPoint = collider.ClosestPoint(playerHand.transform.position);
                    if (Vector3.Distance(playerHand.transform.position, closestPoint) > 0.01f)
                    {
                        Vector3 projectedPoint = handOriginalPos.position + Vector3.Project(closestPoint - handOriginalPos.position, direction);
                        playerHand.transform.position = projectedPoint - direction * cupSize;
                        break;
                    }
                }
            }
            else
            {
                playerHand.transform.position = stickOutTarget.position;
            }
        }
    }

    void swingKnife(bool rightClick)
    {
        if (rightClick && !itemInUse)
        {
            playerAnimator.SetBool("BuildUpKnife", true);
            skillCheckScript.skillCheck.gameObject.SetActive(true);
            skillCheckScript.isActive = true;
            itemInUse = true;
        }
        else if (!rightClick && itemInUse && !playerAnimator.GetBool("KnifeSwing"))
        {
            playerAnimator.SetBool("KnifeSwing", true);
            playerAnimator.SetBool("BuildUpKnife", false);
        }
    }

    void StickOutCup(bool rightClick)
    {
        if (rightClick && !itemInUse && !stickingOutCup && cup.GetComponent<CupDataHolder>().pourLevel == -1)
        {
            if (moveTween != null)
            {
                LeanTween.cancel(moveTween.id);
            }

            // Use LeanTween.value to update the hand's position
            moveTween = LeanTween.value(gameObject, 0f, 1f, 0.1f)
            .setOnUpdate((float t) =>
            {
                // Dynamically update the player's hand position towards the current position of handOriginalPos
                playerHand.transform.position = Vector3.Lerp(playerHand.transform.position, stickOutTarget.position, t);
            })
            .setOnComplete(() =>
            {
                // Ensure the final position is set correctly
                playerHand.transform.position = stickOutTarget.position;
                stickingOutCup = true;
            });

            SetLayerRecursively(playerHand.gameObject, LayerMask.NameToLayer("Player"));

            itemInUse = true;
        }
    }

    void BringCupBackIn()
    {
        if (moveTween != null)
        {
            LeanTween.cancel(moveTween.id);
        }

        stickingOutCup = false;

        // Use LeanTween.value to update the hand's position
        moveTween = LeanTween.value(gameObject, 0f, 1f, 0.1f)
        .setOnUpdate((float t) =>
        {
            // Dynamically update the player's hand position towards the current position of handOriginalPos
            playerHand.transform.position = Vector3.Lerp(playerHand.transform.position, handOriginalPos.position, t);
        })
        .setOnComplete(() =>
        {
            // Ensure the final position is set correctly
            playerHand.transform.position = handOriginalPos.position;
        });

        SetLayerRecursively(playerHand.gameObject, LayerMask.NameToLayer("InFrontOfCamera"));

        itemInUse = false;
    }

    void PlaceFoodInWrap(bool rightClick)
    {
        if(!rightClick) return;

        if (tryingToPlaceInWrap)
        {
            for (int i = 0; i < foodPiecesForWrap.Length; i ++)
            {
                if (foodPiecesForWrap[i].name == playerItemState.itemInHand)
                {
                    foodPiecesForWrap[i].SetActive(true);
                    
                    CheckForSameItemInWrap(playerItemState.itemInHand);
                }
            }
            
            playerAnimator.SetBool("PickedUp", false);
            playerAnimator.SetBool("GrabIdle", false);

            itemInUse = false;
            playerItemState.ClearItem();
            tryingToPlaceInWrap = false;
        }
        else
        {
            playerAnimator.SetBool("PickedUp", false);
            playerAnimator.SetBool("GrabIdle", false);

            itemInUse = false;
            playerItemState.ClearItem();
        }
    }

    void HoldingDrugs(bool rightClick)
    {
        if(rightClick)
        {
            playerAnimator.SetBool("PickedUp", false);
            playerAnimator.SetBool("GrabIdle", false);

            itemInUse = false;
            playerItemState.ClearItem();
        }
    }

    public void PlaceDrugsInBag()
    {
        //Place the drugs in the bag
            playerAnimator.SetBool("PickedUp", false);
            playerAnimator.SetBool("GrabIdle", false);

            itemInUse = false;
            playerItemState.ClearItem();
    }

    void CheckForSameItemInWrap(String itemToAdd)
    {
        bool itemAlreadyInWrap = false;

        for (int i = 0; i < foodPiecesInsideWrap.Count; i ++)
        {
            if (itemToAdd == foodPiecesInsideWrap[i])
            {
                itemAlreadyInWrap = true;
            }
        }

        if (!itemAlreadyInWrap)
        {
            foodPiecesInsideWrap.Add(itemToAdd);
        }
    }

    public void EmptyWrap()
    {
        for (int i = 0; i < foodPiecesForWrap.Length; i ++)
        {
            foodPiecesForWrap[i].SetActive(false);
        }

        foodPiecesInsideWrap.Clear();
    }

    void SetLayerRecursively(GameObject obj, int newLayer) //Setting the layer of an object and its children
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child != null)
            {
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
}
