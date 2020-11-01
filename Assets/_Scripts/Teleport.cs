using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform initialPosition;
    // Start is called before the first frame update
   
   void OnTriggerEnter(Collider other) {
       Debug.Log("Colisiono");
       other.transform.position = initialPosition.position;
       Debug.Log(transform.position);
   }
}
