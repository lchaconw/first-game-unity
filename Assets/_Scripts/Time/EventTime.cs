using UnityEngine;

public class EventTime : MonoBehaviour
{
    private void OnEnable()
    {
        TimeClock.AlLlegarAcero += CambiarARojo;
    }
    private void OnDisable()
    {
        TimeClock.AlLlegarAcero -= CambiarARojo;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    void CambiarARojo()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

}
