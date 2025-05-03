using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpScript : MonoBehaviour
{
    [SerializeField] Material outlineMaterial;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float grabRange;
    private bool outlineActive;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject shawarmaKnife;
    [SerializeField] private GameObject shawarmaDisplayKnife;
    [SerializeField] private GameObject wrapQTEPrefab;
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private Animator wrapAnimator;
    private UseItemScript useItemScript;
    private Renderer outlinedRenderer;
    private FPPlayerController fPPlayerController;

    [HideInInspector]
    public bool wrapQTEActive;
    private GameObject newQTE;
    [SerializeField] private GameObject packagedWrap;
    [SerializeField] private Transform packagedWrapSpawn;
    [SerializeField] private ParticleSystem smokeParticleEffect;
    [SerializeField] private Transform packagedWrapHandLoc;

    [HideInInspector]
    private bool packagedWrapActive;
    public GameObject wrapInHand;
    [SerializeField] private GameObject closedTakeAwayBag;
    [SerializeField] private GameObject cupPrefab;

    [HideInInspector]
    public GameObject cup;
    [SerializeField] private Transform cupHandLoc;
    [SerializeField] private Transform takeawayHandLoc;
    private bool levelStarted;
    [SerializeField] private CustomerManagerScript customerManagerScript;
    [SerializeField] private RotisserieFillUpScript rotisserieFillUpScript;
    private PlayerItemState playerItemState;

    void Start()
    {
        outlineMaterial.SetFloat("_Thickness", 0);
        useItemScript = gameObject.GetComponent<UseItemScript>();
        fPPlayerController = gameObject.GetComponent<FPPlayerController>();
        playerItemState = GetComponent<PlayerItemState>();
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward), out hit, grabRange))
        {
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

            switch (hit.transform.tag)
            {
                /*case "ShawarmaKnife" :
                    ActivateObjectOutline(hit.transform);
                    if (CheckIfHoldingItem()) break;
                    IsKnifeInteractedWith(hit.transform.gameObject, shawarmaKnife);
                    break;*/

                case "ShaverCase" :
                    CanPutKnifeBack(hit.transform);
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

                case "Tomatoes" :
                    ActivateObjectOutline(hit.transform);
                    IsItemPickedUp(hit.transform);
                    break;
                    
                case "Weed" :
                    ActivateObjectOutline(hit.transform);
                    IsItemPickedUp(hit.transform);
                    break;

                case "Wrap" :
                    if (!wrapQTEActive && !packagedWrapActive)
                    {
                        ActivateObjectOutline(hit.transform);
                        CheckToDropFood();
                    }
                    else
                    {
                        DeactivateObjectOutline();
                    }
                    break;
                
                case "PackagedWrap" :
                    ActivateObjectOutline(hit.transform);
                    if (CheckIfHoldingItem()) break;
                    IsPackagedWrapInteractedWith(hit.transform.gameObject);
                    break;

                case "OpenTakeAway" :
                    ActivateObjectOutline(hit.transform);
                    OpenTakeAwayInteracted(hit.transform.gameObject);
                    break;

                case "ClosedTakeAway" :
                    ActivateObjectOutline(hit.transform);
                    if (CheckIfHoldingItem()) break;
                    ClosedTakeAwayInteracted();
                    break;

                case "Cup" :
                    ActivateObjectOutline(hit.transform);
                    if (CheckIfHoldingItem()) break;
                    IsCupPickedUp();
                    break;

                case "CupInstance" :
                    ActivateObjectOutline(hit.transform);
                    if (CheckIfHoldingItem()) break;
                    IsCupInstancePickedUp(hit.transform.gameObject);
                    break;

                case "OpenSign" :
                    if(levelStarted) break;
                    ActivateObjectOutline(hit.transform);
                    StartLevel();
                    break;

                default:
                    if (!hit.transform.TryGetComponent<IInteractable>(out _))
                    {
                        DeactivateObjectOutline(); //Temporary fix for the outline not disappearing when looking at a non-interactable object
                    }
                    break;
            }
        }
        else
        {
            DeactivateObjectOutline();
        }

        Debug.DrawRay(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward) * grabRange, Color.magenta);

        if (wrapQTEActive && newQTE != null && newQTE.GetComponent<WrapQTE>().finished) //Check if wrap QTE is finished
        {
            DeactivateWrapQTE();
        }
    }
    
    void StartLevel()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            levelStarted = true;
            DeactivateObjectOutline();
            customerManagerScript.StartLevel();
        }
    }

    private bool CheckIfHoldingItem()
    {
        if (playerItemState.IsHoldingItem())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void ActivateObjectOutline(Transform hitObject)
    {
        if (outlineActive == false)
        {
            outlinedRenderer = hitObject.GetComponent<Renderer>();

            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetFloat("_Thickness", 0.0068f);
            outlinedRenderer.SetPropertyBlock(propertyBlock);

            outlineActive = true;
        }
    }

    void DeactivateObjectOutline()
    {
        if (outlineActive == true && outlinedRenderer != null)
        {
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            outlinedRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("_Thickness", 0);
            outlinedRenderer.SetPropertyBlock(propertyBlock);

            outlinedRenderer = null;
            outlineActive = false;
        }
    }

    /*void IsKnifeInteractedWith(GameObject hitKnife, GameObject knifeHandVersion)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerAnimator.SetBool("CloseHandKnife", true);
            hitKnife.SetActive(false);
            knifeHandVersion.SetActive(true);
            shawarmaDisplayKnife = hitKnife;

            useItemScript.isItemInHand = true;
            useItemScript.itemInHand = "Knife";
        }
    }*/

    void CanPutKnifeBack(Transform ShawarmaShaver)
    {
        if (shawarmaKnife.activeSelf == true)
        {
            ActivateObjectOutline(ShawarmaShaver.transform);

            if (Input.GetKeyDown(KeyCode.Mouse0) && playerItemState.itemInHand == "Knife" && !useItemScript.itemInUse)
            {
                DeactivateObjectOutline();

                playerAnimator.SetBool("CloseHandKnife", false);
                shawarmaKnife.SetActive(false);
                shawarmaDisplayKnife.SetActive(true);

                playerItemState.ClearItem();
            }
        }
    }

    void IsPackagedWrapInteractedWith(GameObject hitWrap)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerAnimator.SetBool("HoldingWrap", true);
            hitWrap.transform.SetPositionAndRotation(packagedWrapHandLoc.position, packagedWrapHandLoc.rotation);
            hitWrap.transform.parent = packagedWrapHandLoc.parent;
            hitWrap.layer = LayerMask.NameToLayer("InFrontOfCamera");
            hitWrap.GetComponent<Rigidbody>().isKinematic = true;
            playerItemState.SetItem("PackagedWrap", false);

            wrapInHand = hitWrap;

            packagedWrapActive = false;
            DeactivateObjectOutline();
        }
    }

    void IsCupPickedUp()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerAnimator.SetBool("HoldCup", true);

            GameObject spawnedCup = Instantiate(cupPrefab, cupHandLoc.position, cupHandLoc.rotation);
            spawnedCup.layer = LayerMask.NameToLayer("InFrontOfCamera");
            spawnedCup.transform.parent = cupHandLoc.parent;
            //spawnedCup.transform.tag = "CupInstance";

            playerItemState.SetItem("Cup", false);
            cup = spawnedCup;
        }
    }

    void IsCupInstancePickedUp(GameObject hitCupInstance)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerAnimator.SetBool("HoldCup", true);

            cup = hitCupInstance;

            cup.transform.SetPositionAndRotation(cupHandLoc.position, cupHandLoc.rotation);
            cup.transform.parent = cupHandLoc.parent;
            cup.layer = LayerMask.NameToLayer("InFrontOfCamera");
            cup.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("InFrontOfCamera");
            cup.GetComponent<Rigidbody>().isKinematic = true;

            playerItemState.SetItem("Cup", false);
        }
    }

    void OpenTakeAwayInteracted(GameObject openTakeAwayBag)
    {
        TakeawayBagDataHolder openTakeawayBagDataHolder = openTakeAwayBag.GetComponent<TakeawayBagDataHolder>();
        TakeawayBagDataHolder closedTakeawayBagDataHolder = closedTakeAwayBag.GetComponent<TakeawayBagDataHolder>();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(playerItemState.itemInHand == "PackagedWrap")
            {
                playerAnimator.SetBool("HoldingWrap", false);

                openTakeawayBagDataHolder.ingredientsInsideWrap.AddRange(wrapInHand.GetComponent<PackagedWrapDataHolder>().ingredientsInsideThisWrap);
                Destroy(wrapInHand);

                playerItemState.ClearItem();
            }
            else if(playerItemState.itemInHand == "Cup" && cup.GetComponent<CupDataHolder>().pourLevel != -1)
            {
                playerAnimator.SetBool("HoldCup", false);

                openTakeawayBagDataHolder.drinkInsideBag = cup.GetComponent<CupDataHolder>().nameOfDrink;
                Destroy(cup);

                playerItemState.ClearItem();
            }
            else if(CheckForDrugInHand()) //Holding drugs
            {
                openTakeawayBagDataHolder.drugInsideBag = playerItemState.itemInHand;
                print("Drugs inside bag: " + openTakeawayBagDataHolder.drugInsideBag);
                useItemScript.PlaceDrugsInBag();

                playerItemState.ClearItem();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(playerItemState.itemInHand == null && openTakeawayBagDataHolder.ingredientsInsideWrap.Count > 0) //If player is not holding anything and there are ingredients in the bag
            {
                DeactivateObjectOutline();

                closedTakeAwayBag.SetActive(true);
                openTakeAwayBag.SetActive(false);

                closedTakeawayBagDataHolder.drinkInsideBag = openTakeawayBagDataHolder.drinkInsideBag;
                closedTakeawayBagDataHolder.ingredientsInsideWrap.AddRange(openTakeawayBagDataHolder.ingredientsInsideWrap);
                openTakeawayBagDataHolder.ingredientsInsideWrap.Clear();
                openTakeawayBagDataHolder.drinkInsideBag = null;
            }
        }
    }

    private bool CheckForDrugInHand()
    {
        var drugs = new HashSet<string> { "Weed", "Shrooms", "Coke" };
        return drugs.Contains(playerItemState.itemInHand);
    }

    void ClosedTakeAwayInteracted()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerItemState.itemInHand == null)
        {
            closedTakeAwayBag.transform.parent = takeawayHandLoc.parent;
            closedTakeAwayBag.transform.SetPositionAndRotation(takeawayHandLoc.position, takeawayHandLoc.rotation);
            closedTakeAwayBag.GetComponent<BoxCollider>().enabled = false;
            closedTakeAwayBag.GetComponent<Rigidbody>().isKinematic = true;
            closedTakeAwayBag.layer = LayerMask.NameToLayer("InFrontOfCamera");
            
            playerItemState.SetItem("ClosedTakeAway", false);
        }
    }
    

    void IsItemPickedUp(Transform hitItem)
    {
        if (!IsFoodItem(playerItemState.itemInHand)) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerAnimator.SetBool("GrabIdle", false);
            playerAnimator.SetBool("PickedUp", true);

            if(hitItem.tag == "ShawarmaStack")
            {
                rotisserieFillUpScript.EmptyRotisserie();
            }

            playerItemState.SetItem(hitItem.tag, true);

            StartCoroutine(ResetGrabIdleWithDelay());
        }
    }

    private IEnumerator ResetGrabIdleWithDelay()
    {
        // Wait for a short time before setting GrabIdle back to true
        yield return new WaitForSeconds(0.1f);
        playerAnimator.SetBool("GrabIdle", true);
    }

    void CheckToDropFood()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && playerItemState.IsHoldingItem() && IsFoodItem(playerItemState.itemInHand))
        {
            useItemScript.tryingToPlaceInWrap = true;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && useItemScript.foodPiecesInsideWrap.Count > 0)
        {
            ActivateWrapQTE();
            wrapQTEActive = true;
        }
    }

    bool IsFoodItem(string itemInHand)
    {
        return itemInHand != "Knife" && itemInHand != "PackagedWrap" && itemInHand != "Cup" && itemInHand != "ClosedTakeAway";
    }

    void ActivateWrapQTE()
    {
        newQTE = Instantiate(wrapQTEPrefab, canvasObject.transform);
        newQTE.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        newQTE.GetComponent<WrapQTE>().wrapAnimator = wrapAnimator;
        newQTE.GetComponent<WrapQTE>().playerPickUpScript = this;
        newQTE.GetComponent<WrapQTE>().playerController = fPPlayerController;

        fPPlayerController.canMove = false;
        fPPlayerController.wrapCamera = true;
    }

    void DeactivateWrapQTE()
    {
        fPPlayerController.canMove = true;
        fPPlayerController.wrapCamera = false;

        GameObject newPackagedWrap = Instantiate(packagedWrap, packagedWrapSpawn.position, packagedWrapSpawn.rotation);
        newPackagedWrap.GetComponent<PackagedWrapDataHolder>().ingredientsInsideThisWrap.AddRange(useItemScript.foodPiecesInsideWrap);
        packagedWrapActive = true;

        Instantiate(smokeParticleEffect, packagedWrapSpawn.position, smokeParticleEffect.transform.rotation);
        useItemScript.EmptyWrap();

        wrapQTEActive = false;
    }
}
