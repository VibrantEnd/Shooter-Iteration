using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("start timer");
        StartCoroutine(nameof(CountdownTimer));
    }

    private IEnumerator CountdownTimer()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("timer completed");
    } 
}
