using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class TimerImage : MonoBehaviour
{
    public bool isTick { get; private set;}
    public float currentTime { get; private set; } = 1;
    [SerializeField] private float maxTime;
    [SerializeField] private Image img;

    public void StartTimer()
    {
        isTick = true;
    }

    public void StopTimer()
    {
        isTick = false;
    }
    
    public void ResetTimer()
    {
        currentTime = maxTime;
        img.fillAmount = 1f;
    }

    private void Start()
    {
        isTick = false;
        img = GetComponent<Image>(); //назначаем в переменную компонент с объектом 
    }

    private void Update()
    { 
        if (isTick == true && currentTime>0)
        {
            img.fillAmount = currentTime / maxTime;
            currentTime -= Time.deltaTime;
        }
    }
}
