using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChekpointObs : MonoBehaviour
{
    public Transform respawnPositon;
    
    private bool isFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -3)
        {
            if (!isFalling)
            {
                isFalling = true;
                Invoke("checkPointRespawn", 0.5f);
            }

            transform.position = new Vector3(transform.position.x, -3, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RespawnObs")
        {
            respawnPositon = other.transform;
            //Debug.Log("Spawn");
        }
    }

    private void checkPointRespawn()
    {
        transform.position = respawnPositon.position;
        isFalling = false;
    }
}
