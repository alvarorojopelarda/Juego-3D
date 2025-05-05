using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicaPies : MonoBehaviour
{
    public PlayerMovement logicaPersonaje1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        logicaPersonaje1.puedoSaltar=true;
    }
    private void OnTriggerExit(Collider other)
    {
        logicaPersonaje1.puedoSaltar = false;
    }
}
