using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class LevelLoggingBehaviour : MonoBehaviour
{
    private int _sceneIndex;
    private string _sceneName;

    // Start is called before the first frame update
    void Start()
    {
        var activeScene = SceneManager.GetActiveScene();
        _sceneIndex = activeScene.buildIndex;
        _sceneName = activeScene.name;

        Analytics.CustomEvent("Level Start",
            new Dictionary<string, object>
            {
                {"Level", _sceneName}
            }
        );

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart,
                new Parameter(FirebaseAnalytics.ParameterLevel, _sceneIndex),
                new Parameter(FirebaseAnalytics.ParameterLevelName, _sceneName));
    }

    // Update is called once per frame
    void OnDestroy()
    {
        Analytics.CustomEvent("Out Level",
            new Dictionary<string, object>
            {
                {"Level", _sceneName}
            }
        );

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd,
                new Parameter(FirebaseAnalytics.ParameterLevel, _sceneIndex),
                new Parameter(FirebaseAnalytics.ParameterLevelName, _sceneName));
    }
}
