using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    IInteractable interactableInRange = null;
    public GameObject interactionIcon;

    // Start is called before the first frame update
    void Start()
    {
        interactionIcon.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            interactableInRange?.Interacte();
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteracte())
        {
            interactableInRange = interactable;
            interactionIcon.SetActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
            interactionIcon.SetActive(false);
        }
    }



}
