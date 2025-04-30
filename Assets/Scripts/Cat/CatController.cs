using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CatController : MonoBehaviour
{
    [SerializeField] Enemy5 enemy5;

    //audio
    public AudioClip catAttack;
    //

    public Transform playerTransform;
    public LayerMask playerLayer;
    public Rigidbody2D playerRb;
    public float catSpeed = 2f;
    public float stopDistance = 0.1f;
    public float jumpForce = 5f;
    public Animator catAnimator;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public float detectionRange;

    [SerializeField]private Rigidbody2D rbCat;
    private SpriteRenderer spriteRenderer;
    public bool IsLoaded = false;
    private bool isFollowing=true;
    private bool isGrounded = true;


    /*  void Start()
      {
          rb = GetComponent<Rigidbody2D>();
          spriteRenderer = GetComponent<SpriteRenderer>();
      }

      void Update()
      {
          if (!IsLoaded)
          {
              FollowPlayer();
          }

          isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

          if (isGrounded && catAnimator != null)
          {
              catAnimator.ResetTrigger("JumpCat");
          }
      }

      public void FollowPlayer()
      {
          float distanceToPlayer = Vector2.Distance(transform.position, player.position);

          if (distanceToPlayer > stopDistance)
          {
              Vector2 direction = (player.position - transform.position).normalized;
              rb.velocity = new Vector2(direction.x * catSpeed, rb.velocity.y);

              if (catAnimator != null)
              {
                  catAnimator.SetBool("RunCat", Mathf.Abs(rb.velocity.x) > 0.1f);
              }

              FlipSprite(direction.x);
          }
          else
          {
              rb.velocity = new Vector2(0, rb.velocity.y);

              if (catAnimator != null)
              {
                  catAnimator.SetBool("RunCat", false);
              }
          }
      }

      void FlipSprite(float moveDirectionX)
      {
          if (moveDirectionX < 0 && facingRight)
          {
              Flip();
          }
          else if (moveDirectionX > 0 && !facingRight)
          {
              Flip();
          }
      }

      void Flip()
      {
          facingRight = !facingRight;
          Vector3 localScale = transform.localScale;
          localScale.x *= -1;
          transform.localScale = localScale;
      }


      public void OnCatThrown()
      {
          IsLoaded = false;
          rb.gravityScale = 1;
          rb.velocity = Vector2.zero;
          transform.SetParent(null); // Asegura que el gato no esté bajo el Player
      }*/
    private void Awake()
    {
        rbCat = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isFollowing)
        { 
            if (catAnimator != null)
            {
                catAnimator.SetBool("RunCat", false);
            }
        
            FollowPlayer();
            
        }
        else
        {
            StopCat();
            if (catAnimator != null)
            {
                catAnimator.SetBool("RunCat", true);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    private void StopCat()
    {
       
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);

        if (playerCollider != null)
        {
            playerTransform = playerCollider.transform;
            isFollowing = false;
           
        }
        else
        {
            isFollowing = true;  // Cambiar a seguir si el jugador no está detectado
            
        }
    }

    private void FollowPlayer()
    {
        if (catAnimator != null)
        {
            catAnimator.SetBool("RunCat", false);
        }

        if (playerTransform == null) return;

        MoveTowards(playerTransform);
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);

        if (playerCollider == null)
        {
            if (catAnimator != null)
            {
                catAnimator.SetBool("RunCat", true);
            }
            isFollowing = false;  // Dejar de seguir si el jugador sale del rango
            
        }
    }

    private void MoveTowards(Transform playerPosition)
    {
        // Calcula la dirección hacia el objetivo
        Vector2 direction = (playerPosition.position - transform.position).normalized;
        // Conserva la velocidad vertical actual y ajusta solo la velocidad horizontal
        rbCat.velocity = new Vector2(direction.x * catSpeed, rbCat.velocity.y);

        // Voltea el enemigo según la dirección del movimiento
        if (direction.x > 0 && spriteRenderer.flipX || direction.x < 0 && !spriteRenderer.flipX)
        {
            Flip();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el objeto que colisiona tiene el tag especificado
        if (collision.gameObject.CompareTag("Enemy3"))
        {
           
                if (catAnimator != null)
                {
                    catAnimator.SetBool("AttackCat", true);
                    AudioFX.instance.PlaySoundFXClip(catAttack, transform, 1f);
                }
                Destroy(collision.gameObject);
                Invoke(nameof(ReturnToAnimation), .3f);
            
        }
        if (collision.gameObject.CompareTag("Enemy5"))
        {
            
                PararDanio();
                if (catAnimator != null)
                {
                    catAnimator.SetBool("AttackCat", true);
                    AudioFX.instance.PlaySoundFXClip(catAttack, transform, 1f);
                }
                enemy5.TakeHealth();
                Invoke(nameof(ReturnToAnimation), .3f);
            
        }
    }
    private void ReturnToAnimation()
    {
        if (catAnimator != null)
        {
            catAnimator.SetBool("AttackCat", false);
        }
    }

    private void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    void PararDanio()
    {

        Physics2D.IgnoreLayerCollision(9, 7, true);
        Physics2D.IgnoreLayerCollision(9, 8, true);
        Invoke(nameof(ReanudarDanio), .8f); // Reanudar el daño después de 1 segundo

    }
    void ReanudarDanio()
    {

        Physics2D.IgnoreLayerCollision(9, 8, false);
        Physics2D.IgnoreLayerCollision(9, 7, false);

    }
}