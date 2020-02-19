using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class QuitGame : MonoBehaviour
{
    /// <summary>
    /// 終了
    /// </summary>
    public void Tap()
    {

#if UNITY_EDITOR
       
        UnityEditor.EditorApplication.isPlaying = false;
       
#elif UNITY_ANDROID

       UnityEngine.Application.Quit();

#endif 
    }
}
