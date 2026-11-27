using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;

    public Item item;

    public void Interact()
    {
        OnInteract.Invoke();
        Debug.Log("Interacting");
    }
}
