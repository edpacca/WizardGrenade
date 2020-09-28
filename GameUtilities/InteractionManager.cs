using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade.GameObjects
{
    class InteractionManager
    {
        public static void FireballExplosion(Wizard wizard, Explosion explosion)
        {
            Vector2 relativePosition = wizard.position - explosion.Position;
            if (Mechanics.VectorMagnitude(relativePosition) < explosion.explosionRadius * 2)
            {
                wizard.velocity += relativePosition * (250 / Mechanics.VectorMagnitude(relativePosition));
                Damage(wizard, 1500 / (int)Mechanics.VectorMagnitude(relativePosition));
            }
        }

        public static void Damage(Wizard wizard, int damage)
        {
            wizard.UpdateHealth(damage);
        }
    }
}
