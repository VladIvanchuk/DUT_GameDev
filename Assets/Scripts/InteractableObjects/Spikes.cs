using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int _damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if(player != null)
        {
            player.TakeDamage("Hp", _damage);
        }
    }
}
