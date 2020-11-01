using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamara : MonoBehaviour
{
    public GameObject player;
    private Vector3 cameraPosition = new Vector3(0, 5,-8);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + cameraPosition;
    }
}
