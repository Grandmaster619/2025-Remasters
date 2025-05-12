using System.Collections.Generic;
using UnityEngine;

public static class BulletLibrary
{
    // Constants
    public const string RESOURCE_PATH = @"Bullets\";
    public static readonly int Count;

    // Fields
    private static Bullet[] bulletTypes;
    private static Dictionary<string, Bullet> bullets;

    // Static Constructor, called the first time the class is accessed
    static BulletLibrary()
    {
        // Initialize library with all bullet types
        Count = Resources.LoadAll<Bullet>(RESOURCE_PATH).Length;
        bulletTypes = new Bullet[Count];
        bullets = new Dictionary<string, Bullet>(Count);

        InitLibrary();
    }

    private static void InitLibrary()
    {
        // Store all bullet types in the dictionary
        // *Note: keys are stored in UPPER CASE
        int counter = 0;
        foreach (Bullet bullet in Resources.LoadAll<Bullet>(RESOURCE_PATH))
        {
            bulletTypes[counter++] = bullet;
            bullets.Add(bullet.name.ToUpper(), bullet);
        }
    }

    /// <summary>
    /// Tries to find the bullet type <paramref name="bulletName"/> in the Resources. If it does, it assigns it to <paramref name="bullet"/>.
    /// </summary>
    /// <param name="bulletName">Desired Bullet.</param>
    public static bool Contains(string bulletName, out Bullet bullet)
    {
        if (bullets.ContainsKey(bulletName.ToUpper()))
        {
            bullet = bullets[bulletName.ToUpper()];
            return true; 
        }
        else
        {
            Debug.LogError("Bullet " + bulletName + " not found in Resources!");
            bullet = null;
            return false;
        }
    }

    /// <summary>
    /// Returns the bullet at the specified <paramref name="index"/>.
    /// They should be stored in alphabetical order, since they are loaded from a Unity project folder.
    /// Best used for initialization (see ParticleLauncher.cs)
    /// </summary>
    /// <param name="index">Bullet number to access.</param>
    public static Bullet GetBullet(int index)
    {
        return index < Count ? bulletTypes[index] : null;
    }

    /// <summary>
    /// Prints the contents of the Library in [KEY] = VALUE format.
    /// </summary>
    public static void PrintLibrary()
    {
        Debug.Log("===BULLET LIBRARY CONTENTS===");
        foreach (string key in bullets.Keys)
        {
            Debug.Log("[" + key + "] = " + bullets[key]);
        }
        Debug.Log("========================");
    }
}