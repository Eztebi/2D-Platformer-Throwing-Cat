using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    [SerializeField] private AudioClip explosionClip;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el objeto colisionado tiene el tag "Enemy2"
        if (collision.gameObject.CompareTag("Enemy2"))
        {
            // Destruye el enemigo y el proyectil
            Destroy(collision.gameObject); // Destruye Enemy2
            AudioFX.instance.PlaySoundFXClip(explosionClip, transform, 1f);
            Destroy(gameObject); // Destruye el proyectil (squarePrefab)
        }
        Destroy(gameObject); // Destruye el proyectil (squarePrefab)
    }
}