using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
/*{
    public Transform pointA; // Primer punto de patrullaje
    public Transform pointB; // Segundo punto de patrullaje
    public float moveSpeed = 2f;
    public float detectionRange = 5f; // Rango de detección 
    public LayerMask playerLayer; // Capa del jugador

    private Transform currentTarget;
    private bool isPatrolling = true;
    private Rigidbody2D rb;
    private Transform playerTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Inicialmente, el enemigo patrullará hacia el primer punto
        currentTarget = pointA;
    }

    private void Update()
    {
        if (isPatrolling)
        {
            Patrol();
        }
        else
        {
            FollowPlayer();
        }
    }

    private void Patrol()
    {
        // Mueve al enemigo hacia el objetivo actual usando física
        MoveTowards(currentTarget);

        // Si el enemigo ha llegado al objetivo, cambia el objetivo
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f && currentTarget==pointA)
        {
            // Cambia el objetivo de patrullaje
            currentTarget =pointB;
        }
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f && currentTarget == pointB)
        {
            // Cambia el objetivo de patrullaje
            currentTarget = pointA;
        }

        // Verifica si el jugador está dentro del rango de detección usando OverlapCircle
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
        if (playerCollider != null)
        {
            playerTransform = playerCollider.transform; // Guarda la referencia al jugador
            isPatrolling = false; // Deja de patrullar y sigue al jugador
        }
    }

    private void FollowPlayer()
    {
        if (playerTransform != null)
        {
            // Mueve al enemigo hacia el jugador usando física
            MoveTowards(playerTransform);

            // Verifica si el jugador ha salido del rango de detección usando OverlapCircle
            Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
            if (playerCollider == null)
            {
                isPatrolling = true; // Vuelve a patrullar
                // Reestablece el objetivo de patrullaje
                currentTarget = (Vector2.Distance(transform.position, pointA.position) < Vector2.Distance(transform.position, pointB.position)) ? pointA : pointB;
                playerTransform = null; // Limpiar la referencia al jugador
            }
        }
        else
        {
            // Si no se tiene una referencia al jugador, vuelve a patrullar
            isPatrolling = true;
            currentTarget = (Vector2.Distance(transform.position, pointA.position) < Vector2.Distance(transform.position, pointB.position)) ? pointA : pointB;
        }
    }

    private void MoveTowards(Transform target)
    {
        // Calcula la dirección hacia el objetivo
        Vector2 direction = (target.position - transform.position).normalized;
        // Conserva la velocidad vertical actual y ajusta solo la velocidad horizontal
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        // Voltea el enemigo según la dirección del movimiento
        if (direction.x > 0 && !IsFacingRight() || direction.x < 0 && IsFacingRight())
        {
            Flip();
        }
    }

        private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}*/
{
    public Transform pointA; // Primer punto de patrullaje
    public Transform pointB; // Segundo punto de patrullaje
    public float moveSpeed = 2f;
    public float detectionRange = 5f; // Rango de detección 
    public LayerMask playerLayer; // Capa del jugador

    private Transform currentTarget;
    private bool isPatrolling = true;
    private Rigidbody2D rb;
    private Transform playerTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Inicialmente, el enemigo patrullará hacia el primer punto
        currentTarget = pointA;
    }

    private void Update()
    {
        if (isPatrolling)
        {
            Patrol();
        }
        else
        {
            FollowPlayer();
        }
    }

    private void Patrol()
    {
        // Mueve al enemigo hacia el objetivo actual usando física
        MoveTowards(currentTarget);

        // Si el enemigo ha llegado al objetivo, cambia el objetivo
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            if (currentTarget == pointA)
            {
                // Cambia el objetivo de patrullaje a pointB
                currentTarget = pointB;
            }
            else if (currentTarget == pointB)
            {
                // Cambia el objetivo de patrullaje a pointA
                currentTarget = pointA;
            }
        }

        // Verifica si el jugador está dentro del rango de detección usando OverlapCircle
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
        if (playerCollider != null)
        {
            playerTransform = playerCollider.transform; // Guarda la referencia al jugador
            isPatrolling = false; // Deja de patrullar y sigue al jugador
        }
    }

    private void FollowPlayer()
    {
        if (playerTransform != null)
        {
            // Mueve al enemigo hacia el jugador usando física
            MoveTowards(playerTransform);

            // Verifica si el jugador ha salido del rango de detección usando OverlapCircle
            Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
            if (playerCollider == null)
            {
                isPatrolling = true; // Vuelve a patrullar
                // Reestablece el objetivo de patrullaje
                if (Vector2.Distance(transform.position, pointA.position) < Vector2.Distance(transform.position, pointB.position))
                {
                    currentTarget = pointA;
                }
                else
                {
                    currentTarget = pointB;
                }
                playerTransform = null; // Limpiar la referencia al jugador
            }
        }
        else
        {
            // Si no se tiene una referencia al jugador, vuelve a patrullar
            isPatrolling = true;
            // Reestablece el objetivo de patrullaje
            if (Vector2.Distance(transform.position, pointA.position) < Vector2.Distance(transform.position, pointB.position))
            {
                currentTarget = pointA;
            }
            else
            {
                currentTarget = pointB;
            }
        }
    }

    private void MoveTowards(Transform target)
    {
        // Calcula la dirección hacia el objetivo
        Vector2 direction = (target.position - transform.position).normalized;
        // Conserva la velocidad vertical actual y ajusta solo la velocidad horizontal
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        // Voltea el enemigo según la dirección del movimiento
        if (direction.x > 0 && !IsFacingRight() || direction.x < 0 && IsFacingRight())
        {
            Flip();
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void Flip()
    {
        // Voltea la dirección del enemigo
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}