---
title: GameObject
description: Extensions pour les GameObjects Unity
---

Extension utilitaire pour simplifier les opérations courantes sur les GameObjects Unity.

## Utilisation

```cs
using Tools;
```

## Méthodes disponibles

### GetOrAdd()
Récupère un composant sur le GameObject, ou l'ajoute s'il n'existe pas.

**Syntaxe :**
```cs
T GetOrAdd<T>(this GameObject gameObject) where T : Component
```

**Exemple :**
```cs
// S'assure qu'un Rigidbody existe sur l'objet
Rigidbody rb = gameObject.GetOrAdd<Rigidbody>();
```

### OrNull()
Convertit un objet Unity en null si il a été détruit (évite les "fake null" d'Unity).

**Syntaxe :**
```cs
T OrNull<T>(this T obj) where T : Object
```

**Exemple :**
```cs
GameObject obj = FindObjectOfType<GameObject>().OrNull();
if (obj != null)
{
    // L'objet existe vraiment
}
```

ou

```cs
GameObject obj = FindObjectOfType<GameObject>().OrNull()?.// L'objet existe vraiment;
```

### DestroyChildren()
Détruit tous les enfants directs du GameObject.

**Syntaxe :**
```cs
void DestroyChildren(this GameObject gameObject)
```

**Exemple :**
```cs
// Vide complètement un conteneur
containerObject.DestroyChildren();
```

### Clone()
Crée une copie du GameObject à une position et rotation spécifiées.

**Syntaxe :**
```cs
GameObject Clone(this GameObject original, Vector3 position, Quaternion rotation, Transform parent = null)
```

**Exemple :**
```cs
// Clone un objet à une nouvelle position
GameObject copy = prefab.Clone(new Vector3(0, 5, 0), Quaternion.identity);

// Clone avec un parent spécifique
GameObject child = prefab.Clone(Vector3.zero, Quaternion.identity, parentTransform);
```

## Code source

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