using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Enemy3 : MonoBehaviour
{
    public Animator animator;
    public GameObject boladefuego;
    public Transform pointA; // El punto A al que el enemigo se moverá
    public Transform pointB; // El punto B al que el enemigo se moverá    public float speed = 2f; // Velocidad de movimiento del enemigo
    public float velocidadDeDisparo = 10f;
    public float intervaloDeDisparo = 1f;
    public float speed = 2f;
    private Vector3 targetPosition; // Posición a la que el enemigo se está moviendo
    private float tiempoSiguienteDisparo;
    private bool move = true;
    public bool isAttacking = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPosition = pointA.position;
        tiempoSiguienteDisparo = Time.time + intervaloDeDisparo; // Inicializar el tiempo de disparo
    }

    private void Update()
    {
        // Solo mover si no estamos atacando
        if (move && !isAttacking)
        {
            MoveTowardsTarget();
        }

        // Solo atacar si no estamos en medio de un ataque y ha llegado el momento de disparar
        if (Time.time >= tiempoSiguienteDisparo && !isAttacking)
        {
            StartCoroutine(PerformAttack());
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

    private IEnumerator PerformAttack()
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
    private void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}