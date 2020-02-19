using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// スコアの保存クラス
/// 遷移時に変数を保存しておく
/// </summary>
public class SaveManager
{
    /// <summary>
    /// スコアを保存するため変数
    /// </summary>
    static int SaveResult = 0;


    /// <summary>
    /// セット
    /// </summary>
    /// <param name="score"></param>
    public static void SetResult(int score)
    {
        SaveResult = score;
    }


    /// <summary>
    /// ゲット
    /// </summary>
    /// <returns></returns>
    public static int GetResult()
    {
        return SaveResult;
    }
}
