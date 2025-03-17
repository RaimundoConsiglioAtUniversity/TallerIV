using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Extensions
{
    public static void ClampVelocity(this Rigidbody2D rb, float clampX, float clampY)
    {
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -clampX, clampX), Mathf.Clamp(rb.velocity.y, -clampY, clampY));
    }
    public static void AccelerateTo(this Rigidbody2D rb, Vector2 velocity, float accelerationTime, float lerpVal, out float newLerpVal)
    {
        rb.velocity = Vector2.Lerp(rb.velocity, velocity, lerpVal);
        lerpVal += Time.deltaTime / accelerationTime;
        Mathf.Clamp01(lerpVal);
        
        newLerpVal = lerpVal;
    }

    public static Vector3 ArrayProduct(this Vector3 a, Vector3 b)
    {
        float width = a.x * b.x;
        float height = a.y * b.y;
        float depth = a.z * b.z;

        Vector3 product = new(width, height, depth);

        return product;
    }

    public static Vector3 ArrayProduct(this Vector3 a, Vector2 b)
    {
        float width = a.x * b.x;
        float height = a.y * b.y;
        float depth = a.z;

        Vector3 product = new(width, height, depth);

        return product;
    }

    public static Vector2 ArrayProduct(this Vector2 a, Vector2 b)
    {
        float width = a.x * b.x;
        float height = a.y * b.y;

        Vector2 product = new(width, height);

        return product;
    }

    public static void ResetOnChange(this float f, float a, float b, float def = 0f)
    {
        if(a != b)
        {
            f = def;
            a = b;
        }
    }

    public static IEnumerable<T> AddUniqueItems<T>(this IEnumerable<T> originalArray, IEnumerable<T> newItems)
    {
        if (originalArray == null)
            originalArray = new List<T>();  // Initialize if null

        if (newItems == null)
            newItems = new List<T>();  // Initialize if null

        // Convert array to list for flexibility
        List<T> list = new List<T>(originalArray);

        // Loop through new items and add them if they're not already in the list
        foreach (T item in newItems)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

        // Convert the list back to an array and return
        return list.ToArray();
    }

    public static IEnumerable<T> AddUniqueItems<T>(this IEnumerable<T> originalArray, T newItem)
    {
        if (originalArray == null)
            originalArray = new T[0];  // Initialize if null

        // Convert array to list for flexibility
        List<T> list = new List<T>(originalArray);

        if (!list.Contains(newItem))
            list.Add(newItem);

        // Convert the list back to an array and return
        return list.ToArray();
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        
        if (component == null)
            component = gameObject.AddComponent<T>();

        return component;
    }

    public static T EnsureComponent<T>(this GameObject gameObject, ref T component) where T : Component
    {
        if (component == null)
            component = gameObject.AddComponent<T>();
        
        return component;
    }

  // marker
public static List<GameObject> SearchChildrenByName(this Transform parent, string searchString)
    {
    List<GameObject> children = new List<GameObject>();

        foreach (Transform child in parent)
        {
            if (child.gameObject.name.Contains(searchString))
            {
                children.AddUniqueItems(child.gameObject);
                Debug.Log($"Found {child.gameObject.name} as a child of {parent.gameObject.name}");
            }

            // Recursively search in the child's children
        children.AddRange(child.SearchChildrenByName(searchString));
        }

    if (children.Count > 0)
        {
            Debug.Log($"{parent.gameObject.name}'s children:");

            foreach (var child in children)
                Debug.Log(child.gameObject.name);
        }

        return children;
    }
}
