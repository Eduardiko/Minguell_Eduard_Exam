using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{

    [SerializeField] float movementSpeed = 10f;
    [SerializeField] GameObject bullet;
    private SpriteRenderer spriteRenderer;

    private Vector2 destinationPoint;

    private float stillTime = 2f;
    private float stillTimer = 0f;

    private float shootTime = 1f;
    private float shootTimer = 0f;

    private float repeatShooting = 3f;
    private float currentlyShooted = 0f;

    private int health = 100;

    private bool isDamaged = false;

    // Start is called before the first frame update
    void Start()
    {
        destinationPoint = new Vector2(Random.Range(-8f, 8f), Random.Range(4.2f, 3f));
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.grey;

    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector2)transform.position != destinationPoint)
        {
            Vector2 translation = destinationPoint - (Vector2)transform.position;
            gameObject.transform.Translate(movementSpeed * Time.deltaTime * translation);

            shootTimer = shootTime;
            stillTimer = stillTime;
            currentlyShooted = 0;
        }
        else if (repeatShooting != currentlyShooted)
        {
            if (shootTimer > 0f)
            {
                shootTimer -= Time.deltaTime;
                return;
            }

            currentlyShooted++;
            shootTimer = shootTime;

            GameObject.Instantiate(bullet, destinationPoint, Quaternion.identity);
        }
        else
        {
            if (stillTimer > 0f)
            {
                stillTimer -= Time.deltaTime;
                return;
            }

            destinationPoint = new Vector2(Random.Range(-8f, 8f), Random.Range(4.2f, 3f));
        }
    }

    public void ApplyDamage()
    {
        StartCoroutine(DamageBlink());
    }

    IEnumerator DamageBlink()
    {
        isDamaged = true;
        spriteRenderer.color = Color.red;
        health -= 20;

        yield return new WaitForSeconds(1.5f);

        isDamaged = false;
        spriteRenderer.color = Color.grey;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack" || collision.tag == "BulletAttack")
            ApplyDamage();
    }
}
