---
title: Rigidbody
description: Extensions pour les Rigidbody Unity
---

## Méthodes

### AddForceTowardsTarget()

Applique une force vers une cible.

```cs
void AddForceTowardsTarget(this Rigidbody rigidbody, float force, Vector3 target)
```

Exemple :

```cs
enemyRigidbody.AddForceTowardsTarget(500f, player.transform.position);
```

### LimitVelocity()

Limite la vitesse maximale.

```cs
void LimitVelocity(this Rigidbody rigidbody, float maxSpeed)
```

Exemple :

```cs
playerRigidbody.LimitVelocity(10f);
```

## Code

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