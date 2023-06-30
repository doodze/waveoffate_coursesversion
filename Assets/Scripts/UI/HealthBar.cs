using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public float SetMaxHealth
    {
        get => _slider.maxValue;
        set => _slider.maxValue = value;
    }

    public float SetHealth
    {
        get => _slider.value;
        set => _slider.value = value;
    }

    private void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
