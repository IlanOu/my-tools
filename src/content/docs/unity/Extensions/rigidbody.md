---
title: Rigidbody
description: Extensions pour les Rigidbody Unity
---

Extension utilitaire pour simplifier les opérations de physique sur les Rigidbody Unity.

## Utilisation

```cs
using Tools;
```

## Méthodes disponibles

### AddForceTowardsTarget()
Applique une force dirigée vers une position cible spécifiée.

**Syntaxe :**
```cs
void AddForceTowardsTarget(this Rigidbody rigidbody, float force, Vector3 target)
```

**Exemple :**
```cs
// Pousse l'objet vers le joueur avec une force de 500
enemyRigidbody.AddForceTowardsTarget(500f, player.transform.position);

// Attire un objet vers un point d'attraction
collectibleRb.AddForceTowardsTarget(200f, magnetPosition);
```

### LimitVelocity()
Limite la vitesse maximale du Rigidbody en conservant la direction du mouvement.

**Syntaxe :**
```cs
void LimitVelocity(this Rigidbody rigidbody, float maxSpeed)
```

**Exemple :**
```cs
// Limite la vitesse du joueur à 10 unités/seconde
playerRigidbody.LimitVelocity(10f);

// Empêche un projectile d'aller trop vite
bulletRb.LimitVelocity(50f);
```

**Notes :**
- La méthode préserve la direction du mouvement
- Utilise `linearVelocity` (Unity 2023.2+) au lieu de l'ancienne propriété `velocity`

## Code source

```cs
using UnityEngine;

namespace Tools
{
    public static class RigidbodyExtension
    {
        public static void AddForceTowardsTarget(this Rigidbody rigidbody, float force, Vector3 target)
        {
            Vector3 direction = target - rigidbody.transform.position;
            direction.Normalize();
            rigidbody.AddForce(direction * force);
        }
        
        public static void LimitVelocity(this Rigidbody rigidbody, float maxSpeed)
        {
            Vector3 velocity = rigidbody.linearVelocity;
            if (velocity.magnitude > maxSpeed)
            {
                velocity.Normalize();
                velocity *= maxSpeed;
                rigidbody.linearVelocity = velocity;
            }
        }
    }
}
```