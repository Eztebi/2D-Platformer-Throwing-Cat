using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AmmoDisplay : MonoBehaviour
{
    public Text ammoText; // Arrastra aquí el componente Text del contador de balas
    //public PlayerController playerController; // Arrastra aquí el script del jugador
    public PlayerController playerController;
    void Update()
    {
        // Actualiza el texto del contador de balas en la UI
       // ammoText.text = "Ammo: " + playerController.currentAmmo.ToString();
        ammoText.text = "Ammo: " + playerController.currentAmmo.ToString();
    }
}