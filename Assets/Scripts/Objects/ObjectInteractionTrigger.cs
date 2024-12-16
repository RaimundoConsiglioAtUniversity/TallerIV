using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class ObjectInteractionTrigger : MonoBehaviour
{
    public int objectID;
    [SerializeField] protected InteractiveObject[] linkedObjects;

    void Start()
    {
        print($"Initiated {gameObject.name}'s ObjectInteractionTrigger's Setup Method");

        linkedObjects = SetTiledProperties.Instance.interactives
            .Where(Interactive => Interactive.ID == objectID)
            .ToArray();
    }

    public abstract void Interact();
}
