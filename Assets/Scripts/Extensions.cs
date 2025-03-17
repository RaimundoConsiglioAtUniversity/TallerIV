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

    public static T[] AddUniqueItems<T>(this T[] originalArray, T[] newItems)
    {
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

    public static T[] AddUniqueItems<T>(this T[] originalArray, T newItem)
    {
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

    public static GameObject[] SearchChildrenByName(this Transform parent, string searchString)
    {
        GameObject[] children = {};

        foreach (Transform child in parent)
        {
            if (child.name.Contains(searchString))
                children.AddUniqueItems(child.gameObject);

            // Recursively search in the child's children
            children.AddUniqueItems(child.SearchChildrenByName(searchString));
        }

        return children;
    }
}
