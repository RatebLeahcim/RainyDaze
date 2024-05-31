using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    public GameEvent OnInteractEvent { get; }
    public abstract void OnInteract();
}
