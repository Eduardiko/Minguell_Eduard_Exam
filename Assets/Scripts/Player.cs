using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{

    [SerializeField] float movementSpeed = 5f;
    [SerializeField] GameObject attackCollider;
    [SerializeField] private TextMeshProUGUI healthText;

    private SpriteRenderer spriteRenderer;
    private bool isDamaged = false;
    private int health = 100;
    private Animator animator;

    void Start()
    {
        attackCollider.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float verticalMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal");

        animator.SetInteger("directionX", (int)horizontalMovement);
        animator.SetInteger("directionY", (int)verticalMovement);

        gameObject.transform.Translate(movementSpeed * horizontalMovement * Time.deltaTime, movementSpeed * verticalMovement * Time.deltaTime, 0f);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Attack();

        healthText.text = "Health: " + health;
    }
    public void AttackPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
            Attack();
    }

    void Attack()
    {
        StartCoroutine(PerformAttack());
    }

    IEnumerator PerformAttack()
    {
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        attackCollider.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !isDamaged)
            ApplyDamage();
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

        if(health <= 0)
            Destroy(gameObject);

        yield return new WaitForSeconds(1.5f);
        
        isDamaged = false;
        spriteRenderer.color = Color.white;
    }
}
