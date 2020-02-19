using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillAnimation : MonoBehaviour
{
    /// <summary>
    /// 生成したオブジェクト（1..*）のTransを格納するためのリスト。
    /// </summary>
    public List<Transform> StoreList = new List<Transform>();

    /// <summary>
    /// 生成したオブジェクト（1..*）更新中の座標を格納するためのリスト。
    /// newTrよりも使い勝手がよさそう
    /// </summary>
    public List<Vector3[]> StoreListVector = new List<Vector3[]>();



    /// <summary>
    /// 移動用こるーちん
    /// </summary>
    /// <param name="store">更新した座標をListに格納</param>
    /// <param name="to">移動先座標</param>
    /// <returns></returns>
    public IEnumerator MoveToTargetPosition(List<Transform> store, List<Vector3> to)
    {
        //移動元生成した玉の今の座標　store からとってこれる？
        var fromList = new List<Vector3>();
        //移動先生成した玉の移動先　generatedTargetPos　をもらってきてこれに格納する
        var toList = new List<Vector3>();
        toList = to;


        // カウント用の時間変数
        var time = 0.0f;

        // 何秒でTargetPositionに到達させるか
        var limit = 0.4f;

        // leap用の移動速度保管用
        var progress = 0.0f;

        // TransformのListにlistCount分の情報を突っ込む
        for (int i = 0; store.Count > i; i++)
        {
            StoreList.Add(store[i]);
            fromList.Add(store[i].position);
        }

        while (progress < 1.0f)
        {
            time += Time.deltaTime;
            progress = time / limit;


            for (int i = 0; store.Count > i; i++)
            {
                //線形補完
                var newPos = Vector3.Lerp(fromList[i], toList[i], progress);
                store[i].position = newPos;
            }

            yield return null;
        }
    }
}
