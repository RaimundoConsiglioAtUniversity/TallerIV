using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        if (a == b)
            return;
        
        f = def;
        a = b;
    }

    // Can use both assignment form `list = list.AddIfUnique(item)` and in-place modification form `list.AddIfUnique(item)`
    public static List<T> AddIfUnique<T>(this List<T> list, T item)
    {
        if (list.Contains(item))
            return list;
        
        list.Add(item);

        return list;
    }

    public static List<T> AddIfUnique<T>(this List<T> list, List<T> items)
    {
        foreach (var item in items)
            list = list.AddIfUnique(item);

        return list;
    }

    public static List<T> AddIfUnique<T>(this List<T> list, T[] items)
    {
        foreach (var item in items)
            list = list.AddIfUnique(item);

        return list;
    }

    // Must use assignment form `array = array.AddIfUnique(item)`
    public static T[] AddIfUnique<T>(this T[] array, T item)
    {
        List<T> list = array.ToList();

        list = list.AddIfUnique(item);

        return list.ToArray();
    }

    public static T[] AddIfUnique<T>(this T[] array, List<T> items)
    {
        foreach (var item in items)
            array = array.AddIfUnique(item);

        return array;
    }

    public static T[] AddIfUnique<T>(this T[] array, T[] items)
    {
        foreach (var item in items)
            array = array.AddIfUnique(item);

        return array;
    }

    public static List<GameObject> SearchChildrenByName(this Transform parent, string searchString)
    {
        List<GameObject> children = new();

        foreach (Transform child in parent)
        {
            if (child.gameObject.name.Contains(searchString))
                children = children.AddIfUnique(child.gameObject); // Capture the result of the extension method

            // Recursively search in the child's children
            children.AddRange(child.SearchChildrenByName(searchString));
        }

        return children;
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (!gameObject.TryGetComponent<T>(out var component))
            component = gameObject.AddComponent<T>();

        return component;
    }

    public static T EnsureComponent<T>(this GameObject gameObject, ref T component) where T : Component
    {
        if (component == null)
            component = gameObject.AddComponent<T>();
        
        return component;
    }
}
