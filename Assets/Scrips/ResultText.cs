using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ResultText : MonoBehaviour
{

    [SerializeField] private Text text = null;
    private int ResultScore = 0;

    // Start is called before the first frame update
    private void Start()
    {
        ResultScore = SaveManager.GetResult();
        text.text = ("You're Score: " + (ResultScore).ToString("0"));      
    }

}
