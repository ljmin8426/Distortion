using UnityEngine;

public class Player : SingletonDestroy<Player>, IDamaged
{
    public StateMachine<PLAYER_STATE, PlayerController> StateMachine { get; private set; }

    public void TakeDamage(int amount)
    {
        var shield = GetComponent<Shield>();
        if (shield != null && shield.IsShieldActive())
        {
            int remaining = shield.AbsorbDamage(amount);
            if (remaining <= 0)
            {
                return;
            }

            amount = remaining;
        }

        PlayerStatManager.instance.TakeDamage(amount);

        StateMachine.ChangeState(PLAYER_STATE.Hit);
    }
}
