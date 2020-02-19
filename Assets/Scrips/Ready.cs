using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Ready : MonoBehaviour
{
    /// <summary>
    /// 固定テキスト
    /// </summary>
    [SerializeField] private Text ReadyCheck = null;

    /// <summary>
    /// カウントダウンテキスト
    /// </summary>
    [SerializeField] private Text Readycount = null;

    /// <summary>
    /// 時間
    /// </summary>
    private int Count = 0;
    private bool bOnce = false;

    /// <summary>
    /// 時間計測用
    /// </summary>
    private float ReadyCount = 5;

    void Start()
    {
        ReadyCheck.text = ("Get Ready ?");
    }

//@todo 遷移じゃなくコルーチンで呼び出せるように
    private void Update()
    {        
        //　5秒でスタート
        ReadyCount -= Time.deltaTime;

        // 引数0で小数点以下の表示が消える
        Readycount.text = (ReadyCount.ToString("0"));

        if (ReadyCount <= 1)
        {
            if (!bOnce)
            {
                bOnce = true;
            }

            Readycount.text = ("GO!!");
        }

        // 削除
        if (Count > ReadyCount)
        {
            ReadyCheck.enabled = false;
            Readycount.enabled = false;
            Destroy(this);
            SceneManager.LoadScene("Game");
        }

    }
}
