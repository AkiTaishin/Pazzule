using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStatus : MonoBehaviour
{
    /// <summary>
    /// 3Dテキストへのアクセス
    /// </summary>
    [SerializeField] private TextMesh BallNumber = null;

    /// <summary>
    /// Numberの値は全ての玉が持つ。
    /// この数値を参照して玉の判定をとっている。
    /// </summary>
    public int Number;

    /// <summary>
    /// 削除判定を取る際に使用。
    /// falseの時は削除予約されていることを示す。
    /// </summary>
    public bool bAlive = true;



    /// <summary>
    /// BallCreate.csで使用。
    /// numに応じてマテリアルの色を変更する。
    /// </summary>
    /// <param name="num">乱数</param>
    public void Init(int num)
    {
        Number = num;
        BallNumber.text = (Number).ToString("0");

        // 乱数Numの内容に応じてテキストの色を変更
        switch (num)
        {
            case 0:
                BallNumber.color = new Color32(255, 0, 0, 255);
                break;

            case 1:
                BallNumber.color = new Color32(0, 255, 0, 255);
                break;

            case 2:
                BallNumber.color = new Color32(0, 0, 255, 255);
                break;

            case 3:
                BallNumber.color = new Color32(255, 255, 0, 255);
                break;

            case 4:
                BallNumber.color = new Color32(0, 255, 255, 255);
                break;

            case 5:
                BallNumber.color = new Color32(255, 0, 255, 255);
                break;

            case 6:
                BallNumber.color = new Color32(128, 0, 0, 255);
                break;

            case 7:
                BallNumber.color = new Color32(0, 128, 0, 255);
                break;

            case 8:
                BallNumber.color = new Color32(0, 0, 128, 255);
                break;

            case 9:
                BallNumber.color = new Color32(0, 0, 0, 255);
                break;

            default:
                break;
        }
    }
}
