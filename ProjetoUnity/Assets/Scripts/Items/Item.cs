using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    public bool isTool;

    public event Action OnUsed;
    public event Action OnReleased;

    public void Use()
    {
        OnUsed?.Invoke();
    }

    public void StopUsing()
    {
        OnReleased?.Invoke();
    }
}