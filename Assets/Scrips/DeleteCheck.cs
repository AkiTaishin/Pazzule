using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeleteCheck : MonoBehaviour
{
    /// <summary>
    /// 削除する玉の削除予約フラグ
    /// trueの場合削除する。
    /// </summary>
    public bool bIsDelete = false;


    //消した玉の総数
    private int Result = 0;

   /// <summary>
   /// 受けとった配列番号の持つNumberと同じNumberを持つ配列番号の生存フラグをfalseにする。
   /// </summary>
   /// <param name="List">全体の配列</param>
   /// <param name="SearchX">X要素番号</param>
   /// <param name="SearchY">Y要素番号</param>
   /// <returns>削除フラグ</returns>
    public bool DeleteBlockCheck(BallCreate List, int SearchX, int SearchY)
    {
        // 横並び削除
        {
            if (SearchX > 0 && SearchX < 5)
            {

                if (List.BlockList[SearchX - 1, SearchY].Number == List.BlockList[SearchX, SearchY].Number &&
                    List.BlockList[SearchX + 1, SearchY].Number == List.BlockList[SearchX, SearchY].Number)

                {
                    bIsDelete = true;
                    List.BlockList[SearchX - 1, SearchY].bAlive = false;
                    List.BlockList[SearchX, SearchY].bAlive = false;
                    List.BlockList[SearchX + 1, SearchY].bAlive = false;
                }
            }

            if (SearchX > 0 && (List.BlockList[SearchX - 1, SearchY].Number == List.BlockList[SearchX, SearchY].Number))
            {
                bIsDelete = true;
                List.BlockList[SearchX - 1, SearchY].bAlive = false;
                List.BlockList[SearchX, SearchY].bAlive = false;
            }

            if (SearchX < 5 && (List.BlockList[SearchX + 1, SearchY].Number == List.BlockList[SearchX, SearchY].Number))
            {
                bIsDelete = true;
                List.BlockList[SearchX + 1, SearchY].bAlive = false;
                List.BlockList[SearchX, SearchY].bAlive = false;
            }
        }

        // 縦並び削除
        {
            if (SearchY > 0 && SearchY < 5)
            {
                if (List.BlockList[SearchX, SearchY + 1].Number == List.BlockList[SearchX, SearchY].Number &&
                List.BlockList[SearchX, SearchY - 1].Number == List.BlockList[SearchX, SearchY].Number)
                {
                    bIsDelete = true;
                    List.BlockList[SearchX, SearchY + 1].bAlive = false;
                    List.BlockList[SearchX, SearchY - 1].bAlive = false;
                    List.BlockList[SearchX, SearchY].bAlive = false;
                }
            }

            if (SearchY < 5 && (List.BlockList[SearchX, SearchY + 1].Number == List.BlockList[SearchX, SearchY].Number))
            {
                bIsDelete = true;
                List.BlockList[SearchX, SearchY + 1].bAlive = false;
                List.BlockList[SearchX, SearchY].bAlive = false;
            }

            if (SearchY > 0 && (List.BlockList[SearchX, SearchY - 1].Number == List.BlockList[SearchX, SearchY].Number))
            {
                bIsDelete = true;
                List.BlockList[SearchX, SearchY - 1].bAlive = false;
                List.BlockList[SearchX, SearchY].bAlive = false;
            }
        }

        return bIsDelete;
    }

    public bool AfterMoveCheck(BallStatus[,] ballStatuses, int SearchX, int SearchY)
    {
        bool ReturnCantMove = false;
       // Debug.Log(SearchX + "_" + SearchY);


        // 横並び削除
        {
            // x要素の1番目から要素数-1の時の移動処理
            //if (SearchX > 0 && SearchX < ballStatuses.GetLength(1) - 1)
            //{
            //    // 両隣を見に行っている
            //    if (ballStatuses[SearchX - 1, SearchY].Number == ballStatuses[SearchX, SearchY].Number &&
            //        ballStatuses[SearchX + 1, SearchY].Number == ballStatuses[SearchX, SearchY].Number)
            //    {
            //        ReturnCantMove = true;
            //    }
            //}

            // 左側を見に行っている
            if (SearchX > 0)
                if (ballStatuses[SearchX - 1, SearchY] != null)
                {
                    if ((ballStatuses[SearchX - 1, SearchY].Number == ballStatuses[SearchX, SearchY].Number))
                    {
                        ReturnCantMove = true;
                    }
                }

            // 右側を見に行っている
            if (SearchX < ballStatuses.GetLength(1) - 1)
                if (ballStatuses[SearchX + 1, SearchY] != null)
                {
                    if ((ballStatuses[SearchX + 1, SearchY].Number == ballStatuses[SearchX, SearchY].Number))
                    {

                        ReturnCantMove = true;
                    }
                }

            //if(SearchX > 1 || SearchX <ballStatuses.GetLength(1) - 2)
            //{
            //    if (ballStatuses[SearchX + 2, SearchY] != null)
            //    {
            //        if (ballStatuses[SearchX + 2, SearchY].Number == ballStatuses[SearchX, SearchY].Number &&
            //             ballStatuses[SearchX + 2, SearchY].Number == ballStatuses[SearchX, SearchY].Number)
            //        {
            //            ReturnCantMove = true;
            //        }
            //    }
            //}
        }

        // 縦並び削除
        {
            //if (SearchY > 0 && SearchY < ballStatuses.GetLength(0) - 1)
            //{
            //    if (ballStatuses[SearchX, SearchY + 1].Number == ballStatuses[SearchX, SearchY].Number &&
            //    ballStatuses[SearchX, SearchY - 1].Number == ballStatuses[SearchX, SearchY].Number)
            //    {

            //        ReturnCantMove = true;
            //    }
            //}

            if (SearchY < ballStatuses.GetLength(0) - 1)
                if (ballStatuses[SearchX, SearchY + 1] != null)
                {
                    if ((ballStatuses[SearchX, SearchY + 1].Number == ballStatuses[SearchX, SearchY].Number))
                    {

                        ReturnCantMove = true;
                    }
                }

            if (SearchY > 0)
                if (ballStatuses[SearchX, SearchY - 1] != null)
                {
                    if ((ballStatuses[SearchX, SearchY - 1].Number == ballStatuses[SearchX, SearchY].Number))
                    {

                        ReturnCantMove = true;
                    }
                }
        }


        return ReturnCantMove;
    }

    /// <summary>
    /// すべて消去
    /// </summary>
    public void AllDelete(BallCreate Ballcreate)
    {
        // 配列ごと削除
        for (int j = 0; j < 6; j++)
        {
            for (int i = 0; i < 6; i++)
            {
                Destroy(Ballcreate.BlockList[i, j].gameObject);
                Ballcreate.BlockList[i, j] = null;
            }
        }

    }

    /// <summary>
    /// 生存フラグがfalseの配列を一斉削除する。
    /// 削除したときに上にあった玉を下にずらしていく。
    /// 全て下にズラした後に、削除した配列の中身にnullを格納する。
    /// </summary>
    /// <param name="List">全体の配列</param>
    public void PlzDelete(BallCreate List)
    { 
        if (bIsDelete)
        {
            bIsDelete = false;

            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (!List.BlockList[i, j].bAlive)
                    {
                        Debug.Log("削除:" + i + "_" + j);

                        //玉の移動先配列
                        List<Vector3> toList = new List<Vector3>();
                        for (int k = 0; k <= j; k++)
                        {
                            toList.Add(List.BlockList[i, k].transform.position);
                        }

                        for (int k = j; k > 0; k--)
                        {
                            
                            var work = List.BlockList[i, k];
                            //1個上の座標を一つ下にずらす
                            List.BlockList[i, k - 1].transform.position = toList[k];

                            List.BlockList[i, k] = List.BlockList[i, k - 1];
                            List.BlockList[i, k - 1] = work;                 
                        }
                    }
                }
            }

            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (!List.BlockList[i, j].bAlive)
                    {
                        Destroy(List.BlockList[i, j].gameObject);
                        List.BlockList[i, j] = null;
                        Result += 1;
                    }
                }
            }   
        }
    }


    public int GetResult()
    {
        return Result;
    }

}
