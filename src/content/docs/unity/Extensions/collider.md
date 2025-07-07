---
title: Colliders
description: Extensions pour les colliders Unity
---

Extension utilitaire pour simplifier les opérations courantes sur les colliders Unity.

## Utilisation

```cs
using Tools;
```

## Méthodes disponibles

### IsIntersecting()
Vérifie si deux colliders se chevauchent en comparant leurs bounds.

**Syntaxe :**
```cs
bool IsIntersecting(this Collider collider, Collider other)
```

**Exemple :**
```cs
if (playerCollider.IsIntersecting(enemyCollider))
{
    // Collision détectée
}
```

### ResizeProportionnally()
Redimensionne un collider en appliquant un facteur d'échelle proportionnel.

**Syntaxe :**
```cs
Collider ResizeProportionnally(this Collider collider, Vector3 scale)
```

**Exemple :**
```cs
// Double la taille du collider
collider.ResizeProportionnally(new Vector3(2f, 2f, 2f));
```

## Code source

```cs
namespace Tools
{
    public static class ColliderExtension
    {
        public static bool IsIntersecting(this Collider collider, Collider other)
        {
            return collider.bounds.Intersects(other.bounds);
        }

        public static Collider ResizeProportionnally(this Collider collider, Vector3 scale)
        {
            collider.transform.localScale = scale;
            return collider;
        }
    }
}
```