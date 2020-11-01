using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public Vector3 respawnPositon;
    public AudioClip deadSound;
    private AudioSource _audioSource;
    private bool isFalling = false;
    public GameObject gameManager;
    private AdMobsADS _scriptAdmobs;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        respawnPositon = transform.position;
        _scriptAdmobs = gameManager.GetComponent<AdMobsADS>();
        
        if(_scriptAdmobs != null)
        {
            _scriptAdmobs.MostrarInterstitial();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -6)
        {
            if (!isFalling)
            {
                isFalling = true;
                _audioSource.Stop();
                _audioSource.PlayOneShot(deadSound, 1);
                _scriptAdmobs.MostrarInterstitial();
                Invoke("checkPointRespawn", 1.5f);
            }

            transform.position = new Vector3(transform.position.x, -6, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Respawn")
        {
            respawnPositon = other.transform.position;
            //Debug.Log("Spawn");
        }
    }

    private void checkPointRespawn()
    {
        var activeScene = SceneManager.GetActiveScene();
        string sceneName = activeScene.name;

        transform.position = respawnPositon;
        isFalling = false;
        FirebaseAnalytics.LogEvent("Fall",
           new Parameter("Position", "(" + transform.position.x + "," + transform.position.y + "," + transform.position.z + ")"),
           new Parameter(FirebaseAnalytics.ParameterLevelName, sceneName)
        );
    }
}
