using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour
{
    public int Health;
    public bool IsDead = false;

    public event UnityAction<Entity> OnEntityDeaded;

    public void ApplyDamage(int health)
    {
        Health -= health;

        if (Health <= 0)
        {
            OnEntityDeaded?.Invoke(this);
            PlayerOnDead();
            IsDead = true;
        }
    }

    protected abstract void PlayerOnDead();
}
