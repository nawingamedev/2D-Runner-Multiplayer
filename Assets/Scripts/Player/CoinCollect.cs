using UnityEngine;

public class CoinCollect : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject other = collision.gameObject;
        }
    }
}
