using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TMP_Text timer;
    
    void Start()
    {
        
    }

    void Update()
    {
        timer.text = $"Timer: {Time.realtimeSinceStartup: 0.000}";
    }
}
