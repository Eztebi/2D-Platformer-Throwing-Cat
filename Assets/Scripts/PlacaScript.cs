using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacaScript : MonoBehaviour
{
    public GameObject puerta;
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cat"))
        {
            Destroy(puerta);
            Destroy(gameObject);
        }
    }
}
