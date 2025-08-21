public class HealthControl
{
    private float currentHp;
    private bool isDead;
    private float maxHp;
    
    public HealthControl(float maxHp)
    {
        this.maxHp = maxHp;
    }

    public void OnInit()
    {
        isDead = false;
        currentHp = 0;
    }


    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            OnDead();
        }
    }

    public void Heal(float heal)
    {
        currentHp += heal;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }
    private void OnDead()
    {
        isDead = true;
    }
    
}
