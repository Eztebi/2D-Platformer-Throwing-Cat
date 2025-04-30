
using UnityEngine;
using System.Collections;

public class Enemy5 : MonoBehaviour
{
    public GameObject door;
    public int maxHealthEnemy = 6;
    public int currentEnemyHealth;
    public Animator animator;
    public GameObject boladefuego;
    public Transform pointA; // El punto A al que el enemigo se moverá
    public Transform pointB; // El punto B al que el enemigo se moverá
    public float speed = 2f; // Velocidad de movimiento del enemigo
    public float velocidadDeDisparo = 10f;
    public float intervaloDeDisparo = 1f;
    private Vector3 targetPosition; // Posición a la que el enemigo se está moviendo
    private float tiempoSiguienteDisparo;
    private bool move = true;
    public bool isAttacking = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentEnemyHealth = maxHealthEnemy;
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPosition = pointA.position;
        tiempoSiguienteDisparo = Time.time + intervaloDeDisparo; // Inicializar el tiempo de disparo
    }

    private void Update()
    {
        if (currentEnemyHealth <= 0)
        {
            Destroy(gameObject); // Si la vida es 0 o menos, destruir el objeto
            return;
        }

        // Solo mover si no estamos atacando
        if (move && !isAttacking)
        {
            MoveTowardsTarget();
        }

        // Solo atacar si no estamos en medio de un ataque y ha llegado el momento de disparar
        if (Time.time >= tiempoSiguienteDisparo && !isAttacking)
        {
            if (currentEnemyHealth <= 4 && currentEnemyHealth > 2)
            {
                StartCoroutine(PerformAttack2());
            }
            else if (currentEnemyHealth <= 6 && currentEnemyHealth>4)
            {
                StartCoroutine(PerformAttack1());
            }
            else if(currentEnemyHealth <= 2) {
                StartCoroutine(PerformAttack3());
            }
        }

        // Cambiar la posición objetivo cuando el enemigo llegue a la actual
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (targetPosition == pointA.position)
            {
                Flip();
                targetPosition = pointB.position;
            }
            else
            {
                Flip();
                targetPosition = pointA.position;
            }
        }
    }

    private void MoveTowardsTarget()
    {
        float step = speed * Time.deltaTime;  // La distancia que el enemigo se moverá en este frame
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

    private IEnumerator PerformAttack1()
    {
        move = false;  // Detiene el movimiento
        isAttacking = true;  // Marca que estamos atacando

        if (animator != null)
        {
            animator.SetBool("Shooting", true);  // Activa la animación de disparo
        }

        // Espera el tiempo que dura la animación de disparo
        yield return new WaitForSeconds(.7f);  // Ajusta este valor según la duración de tu animación

        if (animator != null)
        {
            animator.SetBool("Shooting", false);  // Desactiva la animación de disparo
        }

        // Disparar proyectil
        GameObject bolaDeFuego = Instantiate(boladefuego, transform.position, Quaternion.identity);

        // Calcular la dirección hacia abajo (o cualquier otra dirección deseada)
        Vector2 direccion = Vector2.down;

        Rigidbody2D rb = bolaDeFuego.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direccion * velocidadDeDisparo;
        }

        // Reanuda el movimiento
        move = true;
        isAttacking = false;  // Marca que ya no estamos atacando

        // Actualiza el tiempo de disparo para el próximo ataque
        tiempoSiguienteDisparo = Time.time + intervaloDeDisparo;
    }

    private IEnumerator PerformAttack2()
    {
        move = false;  // Detiene el movimiento
        isAttacking = true;  // Marca que estamos atacando

        if (animator != null)
        {
            animator.SetBool("Shooting", true);  // Activa la animación de disparo
        }

        // Espera el tiempo que dura la animación de disparo
        yield return new WaitForSeconds(.7f);  // Ajusta este valor según la duración de tu animación

        if (animator != null)
        {
            animator.SetBool("Shooting", false);  // Desactiva la animación de disparo
        }

        // Disparar 10 bolas de fuego una tras otra
        for (int i = 0; i < 10; i++)
        {
            GameObject bolaDeFuego = Instantiate(boladefuego, transform.position, Quaternion.identity);

            // Calcular la dirección hacia abajo (o cualquier otra dirección deseada)
            Vector2 direccion = Vector2.down;

            Rigidbody2D rb = bolaDeFuego.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direccion * velocidadDeDisparo;
            }

            yield return new WaitForSeconds(0.1f); // Tiempo entre disparos de bolas de fuego
        }

        // Permite que el enemigo se mueva nuevamente
        move = true;
        isAttacking = false;  // Marca que ya no estamos atacando

        // Actualiza el tiempo de disparo para el próximo ataque
        tiempoSiguienteDisparo = Time.time + intervaloDeDisparo;
    }
    private IEnumerator PerformAttack3()
    {
        move = false;  // Detiene el movimiento
        isAttacking = true;  // Marca que estamos atacando

        if (animator != null)
        {
            animator.SetBool("Shooting", true);  // Activa la animación de disparo
        }

        // Espera el tiempo que dura la animación de disparo
        yield return new WaitForSeconds(.7f);  // Ajusta este valor según la duración de tu animación

        if (animator != null)
        {
            animator.SetBool("Shooting", false);  // Desactiva la animación de disparo
        }

        // Disparar 3 bolas de fuego
        // 1. Bola recta
        DispararBolaDeFuego(Vector2.down);

        // 2. Bola diagonal hacia abajo a la izquierda
        DispararBolaDeFuego(new Vector2(-0.5f, -1f).normalized);

        // 3. Bola diagonal hacia abajo a la derecha
        DispararBolaDeFuego(new Vector2(0.5f, -1f).normalized);

        // Permite que el enemigo se mueva nuevamente
        move = true;
        isAttacking = false;  // Marca que ya no estamos atacando

        // Actualiza el tiempo de disparo para el próximo ataque
        tiempoSiguienteDisparo = Time.time + intervaloDeDisparo;
    }

    private void DispararBolaDeFuego(Vector2 direccion)
    {
        GameObject bolaDeFuego = Instantiate(boladefuego, transform.position, Quaternion.identity);
        Rigidbody2D rb = bolaDeFuego.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direccion * velocidadDeDisparo;
        }
    }
    private void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public void TakeHealth()
    {
        if (currentEnemyHealth > 1)
        {
            currentEnemyHealth--;
        }
        else
        {
            Destroy(gameObject);
            Destroy(door);
        }
    }
}
