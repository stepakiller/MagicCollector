using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] GameObject InteractKey;
    [SerializeField] LayerMask interactableLayer;
    Interactable currentInteractable;
    bool isInTrigger;

    void Update()
    {
        if (isInTrigger && Input.GetKeyDown(Settings.interactKey))
        {
            currentInteractable.Interact();
            InteractKey.SetActive(false);
            isInTrigger = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & interactableLayer.value) != 0)
        {
            currentInteractable = other.GetComponent<Interactable>();
            InteractKey.SetActive(true);
            isInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & interactableLayer.value) != 0)
        {
            InteractKey.SetActive(false);
            isInTrigger = false;
        }
    }
}
