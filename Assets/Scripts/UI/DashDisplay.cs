using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashDisplay : MonoBehaviour

{
    public Slider slider; // Referencia al Slider
    public float fillDuration = 1f; // Duración para rellenar el slider (en segundos)

    private void Start()
    {
        slider.value = 0f;
    }

    public void StartFillAndReset()
    {
        StartCoroutine(FillAndResetSliderCoroutine());
    }

    private IEnumerator FillAndResetSliderCoroutine()
    {
        // Llenar el slider
        float elapsedTime = 0f;
        float startValue = slider.value;
        float endValue = slider.maxValue;

        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            slider.value = Mathf.Lerp(startValue, endValue, elapsedTime / fillDuration);
            yield return null; // Espera hasta el próximo frame
        }

        slider.value = endValue; // Asegúrate de que el slider llegue al valor máximo

        // De inmediato, restablecer el slider a 0
        slider.value = 0f;
    }
}