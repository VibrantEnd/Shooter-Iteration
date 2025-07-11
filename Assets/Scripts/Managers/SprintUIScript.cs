using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintUIScript : MonoBehaviour
{
    Material material;
    PlayerMovement playerMovement;
    private void Awake()
    {
        material = GetComponentInChildren<Renderer>().material;
    }
    void Update()
    {
        
    }
}
