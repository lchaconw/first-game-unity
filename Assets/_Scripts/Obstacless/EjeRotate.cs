using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjeRotate : MonoBehaviour
{
    public Quaternion rotStart;
    public Quaternion rotEnd;
    public float duration;

    private float lerp = 0;
    // Start is called before the first frame update
    void Start()
    {

        transform.localRotation = rotStart;

    }

    // Update is called once per frame
    void Update()
    {
        lerp = Mathf.PingPong(Time.time, duration) / duration;

        transform.localRotation = Quaternion.Lerp(rotStart, rotEnd, lerp);
    }
}
