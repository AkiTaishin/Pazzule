using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMoveSumple : MonoBehaviour
{
    /// <summary>
    /// 移動させるオブジェクト
    /// </summary>
    [SerializeField]
    private Transform objTr = null;
    /// <summary>
    /// 移動先
    /// </summary>
    [SerializeField]
    private Vector3 targetPos = Vector3.zero;
    /// <summary>
    /// 移動アニメーションコルーチンの格納用変数
    /// </summary>
    private Coroutine sampleMoveCoroutine = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //複数移動アニメーションが呼ばれないようにすでに動作しているものは停止しnullを入れる
            if (sampleMoveCoroutine != null)
            {
                StopCoroutine(sampleMoveCoroutine);
                sampleMoveCoroutine = null;
            }

            //objTrの現在の座標からtargetPosまで移動する
            sampleMoveCoroutine =  StartCoroutine(SampleMove(new Transform[] { objTr },objTr.position,targetPos));
        }
    }

    /// <summary>
    /// 移動アニメーション
    /// </summary>
    /// <param name="objTrArr"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    private IEnumerator SampleMove(Transform[] objTrArr, Vector3 from,Vector3 to)
    {
        #region 変数をここで宣言
        var time = 0.0f;
        var limit = 3.0f;
        var progress = 0.0f;
        #endregion

        objTr.transform.position = from;
        while (progress < 1.0f)
        {
            time += Time.deltaTime;
            progress = time / limit;

            #region 移動処理


            objTr.transform.position = Vector3.Lerp(from, to, progress);


            #endregion

            yield return null;
        }
        objTr.transform.position = to;
    }


}
