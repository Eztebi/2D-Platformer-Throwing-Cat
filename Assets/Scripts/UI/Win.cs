using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Win : MonoBehaviour
{
    public string playerTag = "Player";
    public string catTag = "Cat";
    // El tag del objeto que debe colisionar para mostrar el texto
    public Text endText; // Referencia al componente Text de la UI
    public float displayDuration = 3f; // Duración para mostrar el texto

    private void Start()
    {
        // Asegúrate de que el texto esté oculto al inicio
        if (endText != null)
        {
            endText.gameObject.SetActive(false);
        }
    }
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el objeto que colisiona tiene el tag especificado
        if (collision.gameObject.CompareTag(playerTag) )
        {
            ShowEndText();
        }
    }

    void ShowEndText()
    {
        if (endText != null)
        {
            endText.gameObject.SetActive(true); // Muestra el texto
            Invoke(nameof(HideEndText), displayDuration); // Oculta el texto después de un tiempo
        }
    }

    void HideEndText()
    {
        if (endText != null)
        {
            endText.gameObject.SetActive(false);
            if(SceneManager.GetActiveScene().buildIndex==2) {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            
        }
    }
}