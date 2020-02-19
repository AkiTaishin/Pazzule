using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountDown : MonoBehaviour
{
    /// <summary>
    /// 制限時間
    /// </summary>
    private int LimitTime = 20;

    /// <summary>
    /// 今何秒経ったのか
    /// </summary>
    private float Count = 0;


    [SerializeField] private DeleteCheck deleteCheck = null;
    
    [SerializeField] private Text Timer = null;
    [SerializeField] private Text Score = null;

    // Start is called before the first frame update
    private void Start() 
    {
        Count = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        Count += Time.deltaTime;

        // 引数0で小数点以下の表示が消える
        Timer.text = ("Time: " + (LimitTime - Count).ToString("0"));

        Score.text = ("Score: " + deleteCheck.GetResult().ToString("0"));

        if(Count > LimitTime)
        {
            //@todoここあとで直してねぼく
            SaveManager.SetResult(deleteCheck.GetResult());
            SceneManager.LoadScene("Result");

        }

    }
}
