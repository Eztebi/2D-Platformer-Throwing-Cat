using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    //Audio
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip throwCatClip;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip ammoClip;
    [SerializeField] private AudioClip healthClip;
    [SerializeField] private AudioClip explosionClip;

    //dash
    public DashDisplay dashDisplay;
    //Vida
    public int maxHealth = 5;
    public int healthCurrent;
    public HealthBar healthBar;
    Vector2 checkpointPos;
    //Movimjiento personaje
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float wallJumpForwardForce = 10f;
    public float wallSlideSpeed = 2f;
    private float wallCheckDistance = 0.1f;
    public Transform carryPointDetection;
    public Transform carryPointDetection2;
    public Transform carryCatPoint;
    public LayerMask catLayer;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform wallCheckHand;
    public Transform wallCheckFeet;
    public LayerMask wallLayer;
    public Transform shootPoint;
    public GameObject squarePrefab;
    public float shootingForce = 10f;
    private float destroyDelay = 5f;
    public Animator animation;
    public Transform respawnPoint;
    //Dash
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public bool canDash = true;
    public TrailRenderer trailRenderer;
    //Layer de bola de fuego
    public LayerMask enemyLayer;
    private bool isDashing = false;
    private float dashTime;

    //Cat
    public Transform catTransform;
    public CatController CatController;
    public GameObject catPrefab;
    private bool isCarryingCat = false;
    public Rigidbody2D catRb;
    public float catThrow = 8f;
    public Transform CatObj;
    public Animator catAnimator;

    private Rigidbody2D rb;
    private BoxCollider2D coll; // El collider del jugador
    private Vector2 originalColliderSize; // Tamaño original del collider
    private Vector2 originalColliderOffset;
    private bool isGrounded;
    private bool facingRight = true;
    private bool isThrowing = false;
    private bool isTouchingWallHand;
    private bool isTouchingWallFeet;
    private bool canWallJump;
    private bool isWallJumping = false;
    private bool isTouchingCarryPoint;
    private bool isTouchingCarryPoint2;
  


    private int maxAmmo = 3; // Máximo de balas
    public int currentAmmo; // Balas disponibles

    void Start()
    {

       // Physics.IgnoreLayerCollision(8, 7, false);// inicia con la deteccion de los enemigos
        healthCurrent =maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>(); // Obtiene el collider del jugador
        originalColliderSize = coll.size;
        originalColliderOffset = coll.offset;// Almacena el tamaño original del collider
        currentAmmo = maxAmmo; // Inicializa las balas disponibles
        checkpointPos=transform.position;
      

    }

    void Update()
    {
        isTouchingWallHand = Physics2D.OverlapCircle(wallCheckHand.position, wallCheckDistance, wallLayer);
        isTouchingWallFeet = Physics2D.OverlapCircle(wallCheckFeet.position, wallCheckDistance, wallLayer);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        isTouchingCarryPoint = Physics2D.OverlapCircle(carryPointDetection.position, 0.5f, catLayer);
        isTouchingCarryPoint2 = Physics2D.OverlapCircle(carryPointDetection2.position, 0.6f, catLayer);
        {

            if (transform.position.y < -6 || catPrefab.transform.position.y<-6)
            {

                Respawn();
            }

            if (!isThrowing && !isWallJumping && !isDashing)
            {
                if (isCarryingCat)
                {
                  
                    // Permite el movimiento mientras se carga el gato
                    Move();
                    //Aventar el gato
                    ThrowCat();
                }
                else
                {
                   

                    if (isTouchingWallHand || isTouchingWallFeet)
                    {
                        if (!isGrounded)
                        {
                            WallSlide();
                            if (Input.GetButtonDown("Jump"))
                            {
                                WallJump();
                            }
                        }
                        else
                        {
                            Move();
                            Jump();
                        }
                    }
                    else
                    {
                        Move();
                        Jump();
                    }

                    if (Input.GetMouseButtonDown(0) && !isThrowing && !isDashing)
                    {
                        ThrowSquare();
                    }

                    if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
                    {
                        Dash();
                    }
                }
            }

            if (isDashing)
            {
                DashMove();
            }

            CarryCat();
        }
    }
        void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");

        Vector2 moveVelocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        rb.velocity = moveVelocity;

        if ((facingRight && moveInput < 0) || (!facingRight && moveInput > 0))
        {
            Flip();
        }

        if (animation != null)
        {
            animation.SetBool("IsMoving", Mathf.Abs(moveInput) > 0.1f);
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            canWallJump = false;
            if (animation != null)
            {
                animation.SetBool("IsJumping", false);
            }
        }

        else if (isTouchingWallHand || isTouchingWallFeet)
        {
            canWallJump = true;
        }
        else
        {
            canWallJump = false;
        }

        if (!isTouchingWallHand && !isTouchingWallFeet && isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            if (animation != null)
            {
                animation.SetBool("IsJumping", true);
            }
            AudioFX.instance.PlaySoundFXClip(jumpClip, transform, 1f);
        }
    }

    void WallJump()
    {
        isWallJumping = true;
        Flip();
        Invoke("ApplyWallJumpImpulse", 0.1f);
        Invoke("ReEnableMovement", 0.5f);
    }

    void ApplyWallJumpImpulse()
    {
        Vector2 wallJumpDirection = facingRight ? Vector2.left : Vector2.right;
        rb.velocity = new Vector2(wallJumpDirection.x * -wallJumpForwardForce, jumpForce);

        if (animation != null)
        {
            animation.SetBool("IsJumping", true);
        }
    }

    void ReEnableMovement()
    {
        isWallJumping = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void WallSlide()
    {
        if (!isGrounded && (isTouchingWallHand || isTouchingWallFeet))
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void ThrowSquare()
    {
        if (currentAmmo > 0)
        {
            isThrowing = true;

            if (animation != null)
            {
                animation.SetBool("Throw", true);
            }

            // Instancia el objeto squarePrefab
            GameObject square = Instantiate(squarePrefab, shootPoint.position, Quaternion.identity);
            Rigidbody2D squareRb = square.GetComponent<Rigidbody2D>();

            
            if (!facingRight)
            {
                Vector3 scale = square.transform.localScale;
                scale.x *= -1; // Invierte la escala en el eje X
                square.transform.localScale = scale;
            }

            // Configura la velocidad del proyectil
            Vector2 throwDirection = facingRight ? Vector2.right : Vector2.left;
            squareRb.velocity = throwDirection * shootingForce;
            AudioFX.instance.PlaySoundFXClip(shootClip, transform, 1f);
            // Destruye el proyectil después de un cierto tiempo
            Destroy(square, destroyDelay);

            currentAmmo--; // Disminuye el número de balas disponibles

            Invoke("EnableMovement", 0.1f);
        }
    }

    void EnableMovement()
    {
        isThrowing = false;
        if (animation != null)
        {
            animation.SetBool("Throw", false);
        }
    }

    void Dash()
    {
        isDashing = true;
        canDash = false;
        dashTime = Time.time + dashDuration;

        // Reducir el tamaño del collider
        coll.size = new Vector2(0.2f,0.11f);
        coll.offset= new Vector2(0f, -0.11f);
        rb.velocity = new Vector2(facingRight ? dashForce : -dashForce, 0);

        rb.gravityScale = 0;

        if (animation != null)
        {
            animation.SetBool("Dash", true);
        }
        AudioFX.instance.PlaySoundFXClip(dashClip, transform, 1f);
        Invoke("EndDash", dashDuration);
    }

    void DashMove()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, 0.5f, enemyLayer);
        foreach (Collider2D enemy in enemiesHit)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Destroy(enemy.gameObject);
            }
        }

        if (Time.time >= dashTime)
        {
            EndDash();
        }
    }

    void EndDash()
    {
        isDashing = false;
        rb.gravityScale = 1;

        // Restaurar el tamaño del collider
        coll.size = originalColliderSize;
        coll.offset=originalColliderOffset;
        dashDisplay.StartFillAndReset();
        Invoke("ResetDash", dashCooldown);
        if (animation != null)
        {
            animation.SetBool("Dash", false);
        }
    }

    void ResetDash()
    {
        canDash = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isDashing)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                TakeDamage();
               
                PararDanio();
            }
        }
        else if (collision.gameObject.CompareTag("Enemy2"))
        {
            PararDanio();
            TakeDamage();
        }
        else if (collision.gameObject.CompareTag("AmmoPickup")) // Verifica si el objeto es un recolector de balas
        {
            PickupAmmo(collision.gameObject); // Pasa el objeto de recolector de balas para destruirlo
        }
        else if (collision.gameObject.CompareTag("HealthPickUp"))
        {
            PickUpHealth(collision.gameObject);
        }
    }
    //para no tomar danio constante
    void PararDanio()
    {
       
        Physics2D.IgnoreLayerCollision(9, 7, true);
        Physics2D.IgnoreLayerCollision(9, 8, true);
        
        rb.velocity = new Vector2(rb.velocity.x, 5f);
        AudioFX.instance.PlaySoundFXClip(damageClip, transform, 1f);
        Invoke(nameof(ReanudarDanio), 2f); // Reanudar el daño después de 1 segundo
        
    }
    void ReanudarDanio()
    {
      
            Physics2D.IgnoreLayerCollision(9, 8, false);
            Physics2D.IgnoreLayerCollision(9, 7, false);
        
    }
    bool NearCat()
    {

        if (isTouchingCarryPoint || isTouchingCarryPoint2)
        {
            return true;
        }
       
        else{
            return false;
        }
    }
    void CarryCat()
    {
        if (catPrefab == null) return;
        if (NearCat() && Input.GetMouseButtonDown(1))
        {
            isCarryingCat = !isCarryingCat;
            catRb.isKinematic = isCarryingCat;

            if (isCarryingCat)
            {

                catPrefab.transform.position = carryCatPoint.position;
                catPrefab.transform.SetParent(carryCatPoint);
              
            }
            else
            {

                catPrefab.transform.SetParent(CatObj);
            }
        }
    }
    void ThrowCat()
    {
        //if ((isCarryingCat))
        //{
            if (Input.GetMouseButton(0))
            {
            if (catAnimator != null)
            {
                catAnimator.SetBool("JumpCat", true);
            }
            catRb.isKinematic = false;
            isCarryingCat = !isCarryingCat;
                catPrefab.transform.SetParent(CatObj);
            AudioFX.instance.PlaySoundFXClip(throwCatClip, transform, 1f);
            catRb.velocity = new Vector2(catRb.velocity.x, catThrow);
            }
        //}
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        Gizmos.DrawWireSphere(wallCheckHand.position, wallCheckDistance);
        Gizmos.DrawWireSphere(wallCheckFeet.position, wallCheckDistance);
        Gizmos.DrawWireSphere(carryPointDetection.position, .5f);
        Gizmos.DrawWireSphere(carryPointDetection2.position, .6f);
        
    }
    public void UpdatefCheckpoint(Vector2 pos)
    {
        checkpointPos = pos;
    }
    void Respawn()
    {
       TakeDamage();
        transform.position = checkpointPos;
        rb.velocity = Vector2.zero;
        catTransform.position =checkpointPos;
        rb.velocity = Vector2.zero;
    }

    void PickupAmmo(GameObject ammoPickup)
    {
        // Rellena las balas hasta el máximo, sin exceder el límite de 3
        currentAmmo = Mathf.Min(currentAmmo + 3, maxAmmo);
        AudioFX.instance.PlaySoundFXClip(ammoClip, transform, 1f);

        // Destruye el objeto de recolector de balas
        Destroy(ammoPickup);
    }
    void PickUpHealth(GameObject healthPickup)
    {
        TakeHealth();
        AudioFX.instance.PlaySoundFXClip(healthClip, transform, 1f);

        Destroy(healthPickup);
    }
    void TakeDamage()
    {
        if (healthCurrent == 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        healthCurrent--;
        healthBar.SetHealth(healthCurrent);
    }
    void TakeHealth()
    {
        if (healthCurrent == maxHealth)
        {

        }
        else
        {
            healthCurrent++;
            healthBar.SetHealth(healthCurrent);
        }
    }

}