using Firebase;
using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class FirebaseInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            //FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            var app = FirebaseApp.DefaultInstance;
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}