using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
      
        Destroy(gameObject);
    }
}