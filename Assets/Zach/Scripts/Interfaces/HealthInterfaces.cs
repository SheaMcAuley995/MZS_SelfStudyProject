public interface IDamageable<T>
{
    void TakeDamage(T amount);

    void Die();
}
