---
title: GameObject
description: Extensions pour les GameObjects Unity
---

## Méthodes

### GetOrAdd()

Récupère ou ajoute un composant.

```cs
T GetOrAdd<T>(this GameObject gameObject) where T : Component
```

Exemple :

```cs
Rigidbody rb = gameObject.GetOrAdd<Rigidbody>();
```

### OrNull()

Évite les "fake null" d'Unity.

```cs
T OrNull<T>(this T obj) where T : Object
```

Exemple :

```cs
GameObject obj = FindObjectOfType<GameObject>().OrNull();
if (obj != null) { /* L'objet existe */ }
```

### DestroyChildren()

Détruit les enfants directs.

```cs
void DestroyChildren(this GameObject gameObject)
```

Exemple :

```cs
containerObject.DestroyChildren();
```

### Clone()

Crée une copie à une position et rotation spécifiées.

```cs
GameObject Clone(this GameObject original, Vector3 position, Quaternion rotation, Transform parent = null)
```

Exemple :

```cs
GameObject copy = prefab.Clone(new Vector3(0, 5, 0), Quaternion.identity);
```

## Code

```cs
using UnityEngine;

namespace Tools
{
    public static class GameObjectExtension
    {
        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (!component) component = gameObject.AddComponent<T>();
            return component;
        }
        
        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;

        public static void DestroyChildren(this GameObject gameObject)
        {
            foreach (Transform child in gameObject.transform)
            {
                Object.Destroy(child.gameObject);
            }
        }
        
        public static GameObject Clone(this GameObject original, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            GameObject clone = Object.Instantiate(original, position, rotation, parent);
            return clone;
        }
    }
}
```