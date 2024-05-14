using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Transform door;
    public Transform openPos;
    public Transform closePos;
    private float openSpeed = 2f;

    private bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isOpen = false;
        }
    }

    void Update()
    {
        if(isOpen)
        {
            door.position = Vector3.Lerp(door.position, openPos.position, Time.deltaTime * openSpeed);
        }
        if(!isOpen)
        {
            door.position = Vector3.Lerp(door.position, closePos.position, Time.deltaTime * openSpeed);
        }
    }
}
