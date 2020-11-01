using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class AuthUser : MonoBehaviour
{
    public TMP_InputField inputEmail;

    public TMP_InputField inputPassword;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textError;

    private FirebaseAuth auth;

    private Firebase.Auth.FirebaseUser user;

    public string displayName;
    private bool loginIndicator = false;
    private bool registerIndicator = false;
    private bool errorMessageIndicator = false;
    private string errorMessage;

    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    private void Update()
    {
        if (loginIndicator || registerIndicator)
        {
            GetSessionProfile();
            AuthStateChanged(this, null);
            if (loginIndicator)
                SceneManager.LoadScene("Menu");
            else if (registerIndicator)
                SceneManager.LoadScene("Login");
        }
        else if (errorMessageIndicator)
        {
            if (textError != null)
            {
                textError.text = errorMessage;
            }

            errorMessageIndicator = false;
        }
    }

    public void RegisterNewUserByEmail()
    {
        string email = inputEmail.text;
        string password = inputPassword.text;

        if (auth == null)
            InitializeFirebase();

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Entro 1");
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                string exceptionMessage;
                exceptionMessage = task.Exception.InnerExceptions[0].InnerException.Message.ToString();
                errorMessage = exceptionMessage;
                errorMessageIndicator = true;

                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + exceptionMessage);

                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            registerIndicator = true;
        });
    }

    public void LoginValidation()
    {
        string email = inputEmail.text;
        string password = inputPassword.text;

        if (auth == null)
            InitializeFirebase();

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                string exceptionMessage;
                exceptionMessage = task.Exception.InnerExceptions[0].InnerException.Message.ToString();
                errorMessage = exceptionMessage;
                errorMessageIndicator = true;

                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + exceptionMessage);

                return;
            }


            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            loginIndicator = true;
        });
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId + " usuario: " + user.DisplayName + " correo: " + user.Email);
                //displayName = user.DisplayName ?? "";
                displayName = user.Email ?? "";
                if (textName != null)
                {
                    textName.text = "Bienvenido, " + displayName;
                }
                //_email = user.Email ?? "";
                //photoUrl = user.PhotoUrl ?? "";
            }
        }
    }

    private void GetSessionProfile()
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            string name = user.DisplayName;
            string email = user.Email;
            System.Uri photo_url = user.PhotoUrl;
            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            string uid = user.UserId;
        }
    }

    void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= AuthStateChanged;
            auth = null;
        }
    }
    public void Logout()
    {
        FirebaseAuth.DefaultInstance.StateChanged -= AuthStateChanged;
        auth = null;
        SceneManager.LoadScene("Authentication");
    }
}
