using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 25f;   // 속도 조절
    [SerializeField] private float lifetime = 3f;

    private int damage;
    private Vector3 moveDirection;

    public void Init(int dmg, Vector3 direction)
    {
        damage = dmg;
        moveDirection = direction.normalized;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        IDamaged damaged = other.GetComponent<IDamaged>();
        if (damaged != null)
        {
            damaged.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
