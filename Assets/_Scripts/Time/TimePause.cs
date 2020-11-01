using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using UnityEngine.SceneManagement;

public class TimePause : MonoBehaviour
{
    public GameObject timeUI;
    private TimeClock scriptTimeClock;

    private void Start()
    {
        AlInicio();
    }
    private void AlInicio()
    {
        scriptTimeClock = timeUI.GetComponent<TimeClock>();
    }

    private void continuarTiempo()
    {
        scriptTimeClock.Continuar();
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            scriptTimeClock.Pausar();

            Invoke("continuarTiempo", 3f);

            gameObject.SetActive(false);


            var activeScene = SceneManager.GetActiveScene();
            string sceneName = activeScene.name;
            FirebaseAnalytics.LogEvent("TimePause",
            new Parameter("Position", "(" + transform.position.x + "," + transform.position.y + "," + transform.position.z + ")"),
            new Parameter(FirebaseAnalytics.ParameterLevelName, sceneName)
            );
        }
    }
}
