---
title: Colliders
description: Extensions pour les colliders Unity
---

## Méthodes

### IsIntersecting()

Vérifie si deux colliders se chevauchent.

```cs
bool IsIntersecting(this Collider collider, Collider other)
```

Exemple :

```cs
if (playerCollider.IsIntersecting(enemyCollider)) { /* Collision */ }
```

### ResizeProportionnally()

Redimensionne un collider proportionnellement.

```cs
Collider ResizeProportionnally(this Collider collider, Vector3 scale)
```

Exemple :

```cs
collider.ResizeProportionnally(new Vector3(2f, 2f, 2f));
```

## Code

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