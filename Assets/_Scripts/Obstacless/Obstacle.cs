using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{   public Vector3 posStart;
    public Vector3 posEnd;
    public float duration ;
    
    private float lerp = 0;
    // Start is called before the first frame update
    void Start()
    {

        transform.localPosition = posStart;
        
    }

    // Update is called once per frame
    void Update()
    {
        lerp = Mathf.PingPong(Time.time, duration)/duration;
        
        transform.localPosition = Vector3.Lerp(posStart,posEnd,lerp);
    }
}
