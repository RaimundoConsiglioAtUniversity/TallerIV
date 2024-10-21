using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperTiled2Unity;

public class LevelManager : MonoBehaviour
{
    GameObject level;
    public List<string> objects;

    void Start()
    {
        level = GameObject.FindGameObjectWithTag("Level");
        foreach (Transform child in transform)
        {
            objects.Add(child.transform.name);
        }
        
        if (objects.Contains("Collectibles"))
        {
            foreach (Transform child in transform)
            {
                SuperCustomProperties collectible = child.gameObject.GetComponent<SuperCustomProperties>();
                if (collectible)
                {
                    collectible.TryGetCustomProperty("Type", out CustomProperty property);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
