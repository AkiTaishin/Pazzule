using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCreate : MonoBehaviour
{
    /// <summary>
    /// 初期化用定数
    /// </summary>
    private const int DefaultNumber = -1;
    public const int StageMAX = 6;
    [SerializeField] private Move move = null;
    [SerializeField] private DeleteCheck Deletecheck = null;

    /// <summary>
    /// 複製する玉の実態
    /// </summary>
    public GameObject BallPre = null;
  
    // ボールのNumberの数
    private int BallColor = 10;

    /// <summary>
    /// ゲームでの玉の位置を管理する配列
    /// </summary>
    public BallStatus[,] BlockList = new BallStatus[StageMAX, StageMAX] {

        { null, null, null, null, null, null},
        { null, null, null, null, null, null},
        { null, null, null, null, null, null},
        { null, null, null, null, null, null},
        { null, null, null, null, null, null},
        { null, null, null, null, null, null},

    };

    public BallStatus[,] BlockListWork = new BallStatus[StageMAX + 4, StageMAX + 4] {

        { null, null, null, null, null, null, null, null, null, null},
        { null, null, null, null, null, null, null, null, null, null},
        { null, null, null, null, null, null, null, null, null, null},
        { null, null, null, null, null, null, null, null, null, null},
        { null, null, null, null, null, null, null, null, null, null},
        { null, null, null, null, null, null, null, null, null, null},
        { null, null, null, null, null, null, null, null, null, null},
        { null, null, null, null, null, null, null, null, null, null},
        { null, null, null, null, null, null, null, null, null, null},
        { null, null, null, null, null, null, null, null, null, null},

    };

    private void Start()
    {
        StartGame();

        while (!move.MoveCheck())
        {
            Deletecheck.AllDelete(this);
            StartGame();
        }       
    }

    /// <summary>
    /// 玉の初期配置決定とそれを配列に格納する
    /// </summary>
    public void StartGame()
    {

       bool bOnce = false;

        {
            float x, y;
            int u, v;
            u = v = 0;

            x = -2.5f;
            y = 8.0f;

            for (int i = 0; i < StageMAX * StageMAX; i++, u++)
            {
                // ぼーる生成
                GameObject Ball = Instantiate(BallPre) as GameObject;
                
                Ball.tag = "number";
                var ballStatus = Ball.GetComponent<BallStatus>();



                // タブるかもしれないけどとりあえず格納
                int WorkNumber = Random.Range(0, BallColor);

                // 最初だけ通る処理
                if (bOnce)
                {
                    switch (v)
                    {
                        case 0:
                            {
                                // 隣と同じNumberの時のみ再度生成
                                int BeforeNumber = BlockList[u - 1, v].Number;

                                while (WorkNumber == BeforeNumber)
                                {
                                    WorkNumber = Random.Range(0, BallColor);

                                }

                                break;
                            }

                        default:
                            {
                                // 上の行がある。
                                // やってることはほぼ同じ
                                int UpNumber = BlockList[u, v - 1].Number;

                                int BeforeNumber = (u != 0) ? BlockList[u - 1, v].Number : DefaultNumber;


                                // ここで番号かぶりがあった場合に生成しなおし
                                while (WorkNumber == UpNumber || WorkNumber == BeforeNumber)
                                {
                                    WorkNumber = Random.Range(0, BallColor);

                                }

                                break;
                            }

                    }

                }
                bOnce = true;

                ballStatus.Init(WorkNumber);

                BlockList[u, v] = ballStatus;

                Ball.transform.position = new Vector2(x, y);
                

                x += 1.0f;

                if (x > 3.0f)
                {
                    x = -2.5f;
                    y -= 1.0f;

                    u = -1;
                    v += 1;
                }
            }
        }

        #region debug
        string st = "";

        for(int j = 0; j < StageMAX; j++)
        {
            st += "[";
            for(int i = 0; i < StageMAX; i++)
            {
                st +=  BlockList[i, j].Number + ",";

            }
            st += "]\n";
        }

        Debug.Log(st);
        #endregion
    }


    /// <summary>
    /// 玉の補充を管理する。
    /// nullが入った配列要素をReturnする。
    /// </summary>
    /// <param name="generatedTargetPos">ローカルに持ってけるやつ。最終的な移動先</param>
    /// <returns>生成するボールの配列要素番号の組み合わせList</returns>

    public List<int[]> RefillBlock(out List<Vector3> generatedTargetPos)
    {
        List<int[]> ReturnArrayNumber = new List<int[]>();
        generatedTargetPos = new List<Vector3>();


        for (int j = 0; j < 6; j++)
        {
            for (int i = 0; i < 6; i++)
            {
                if (BlockList[i, j] == null)
                {
                    GameObject Ball = Instantiate(BallPre) as GameObject;

                    //var grv = Ball.GetComponent<Rigidbody>();
                    //grv.useGravity = true;

                    var ballStatus = Ball.GetComponent<BallStatus>();
                    int WorkNumber = Random.Range(0, BallColor);

                    ballStatus.Init(WorkNumber);
                    BlockList[i, j] = ballStatus;
                   
                    
                    ReturnArrayNumber.Add(new int[] { i, j });

                    //if (Ball.transform.position == new Vector3(-2.5f + i, 8.0f - j, 0))
                    //{
                    //    grv.useGravity = false;
                    //}
                    generatedTargetPos.Add(new Vector3(-2.5f + i, 8.0f - j, 0.0f));
                    Ball.transform.position = new Vector2(-2.5f + i, 8.0f - j + 8.0f);
                }
            }
        }

        // ここかこの関数を引数にしてRefillAnimationを呼び出す
        return ReturnArrayNumber;


    }

    /// <summary>
    /// BloskListのコピーを作成する。
    /// @todo 配列のコピーの仕方リファレンス見ていい感じの処理に書きかえてみて下さい。
    /// </summary>
    public void CreateBlockListWork()
    {
        BlockListWork = new BallStatus[StageMAX + 4, StageMAX + 4] {
            { null, null, null, null, null, null, null, null, null, null},
            { null, null, null, null, null, null, null, null, null, null},
            { null, null, null, null, null, null, null, null, null, null},
            { null, null, null, null, null, null, null, null, null, null},
            { null, null, null, null, null, null, null, null, null, null},
            { null, null, null, null, null, null, null, null, null, null},
            { null, null, null, null, null, null, null, null, null, null},
            { null, null, null, null, null, null, null, null, null, null},
            { null, null, null, null, null, null, null, null, null, null},
            { null, null, null, null, null, null, null, null, null, null},
        };

        for (int j = 0; j < 6 ; j++)
        {
            for (int i = 0; i < 6; i++)
            {
                BlockListWork[i+2, j+2] = BlockList[i, j];
            }
        }
    }
}
