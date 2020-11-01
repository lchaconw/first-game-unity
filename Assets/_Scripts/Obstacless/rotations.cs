using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotations : MonoBehaviour
{   public float speed;
    public Vector3 rotationLad;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.Rotate(rotationLad * speed * Time.deltaTime); 
        
   } 
}