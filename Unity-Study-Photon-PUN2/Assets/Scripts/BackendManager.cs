using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Database;

public class BackendManager : MonoBehaviour
{
    private static FirebaseApp app;
    private static FirebaseAuth auth;
    private static FirebaseDatabase database;

    private static DatabaseReference userDataRef;
    private static DatabaseReference currentUserDataRef;

    public static FirebaseAuth Auth => auth;
    public static FirebaseDatabase Database => database;

    public static DatabaseReference UserDataRef => userDataRef;
    public static DatabaseReference CurrentUserDataRef => currentUserDataRef;

    private void Awake()
    {
        CheckFirebaseDependencies();
    }

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                database = FirebaseDatabase.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                Debug.Log("FirebaseApp 종속성 검사 완료");

                userDataRef = database.RootReference.Child($"Users");
                auth.IdTokenChanged += Auth_IdTokenChanged;
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                // Firebase Unity SDK is not safe to use here.
                app = null;
                auth = null;
                database = null;

                userDataRef = null;
            }
        });
    }

    private void Auth_IdTokenChanged(object sender, System.EventArgs args)
    {
        if (auth.CurrentUser == null)
        {
            currentUserDataRef = null;
        }
        else
        {
            currentUserDataRef = database.RootReference.Child($"Users/{auth.CurrentUser.UserId}");
        }
    }
}
