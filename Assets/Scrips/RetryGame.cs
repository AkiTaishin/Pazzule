using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RetryGame : MonoBehaviour
{

    /// <summary>
    /// リトライ
    /// </summary>
    public void Tap()
    {

#if UNITY_EDITOR

        SceneManager.LoadScene("Title");
        Debug.Log("hai");

#elif UNITY_ANDROID

        SceneManager.LoadScene("Title");

#endif
    }
}
    