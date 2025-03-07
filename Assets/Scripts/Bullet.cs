using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float movementSpeed = 4f;

    void Update()
    {
        gameObject.transform.Translate(0f, -movementSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            movementSpeed *= -1;
            gameObject.tag = "BulletAttack";
        }

        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().ApplyDamage();
            Destroy(gameObject);
        }

        if((collision.tag == "Enemy" && gameObject.tag == "BulletAttack") || collision.tag == "Wall")
            Destroy(gameObject);
    }
}
