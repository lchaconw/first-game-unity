using UnityEngine;

public class MirrorObstacle : MonoBehaviour
{
    public Vector3 returnPlace;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = returnPlace;
            //Destroy(this.gameObject);
        }
    }
}
