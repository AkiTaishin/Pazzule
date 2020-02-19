using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    /// <summary>
    /// 補充するべき玉の配列要素番号を格納
    /// </summary>
    public List<int[]> GetArrayNumber = new List<int[]>();

    [SerializeField] private RefillAnimation refill = null;

    /// <summary>
    /// タッチ入力開始と終了の判定処理に用いる
    /// </summary>
    private Vector3 MoveStart, MoveEnd = Vector3.zero;

    private Vector3 SaveMousePosition = Vector3.zero;

    /// <summary>
    /// 消す予約をするときに使用
    /// 自分の移動先の配列要素番号を格納する
    /// </summary>
    private int ArrayNoX, ArrayNoY = -1;

    private int NewX, NewY = -1;

    /// <summary>
    /// 全てチェックし終わって消せる玉がない場合にfalseに戻る。
    /// 消せる玉が１つでもある場合はtrue。
    /// </summary>
    private bool bLoop = false;

    /// <summary>
    /// コルーチン稼働中はtrue。
    /// 二重処理防止用フラグ。
    /// </summary>
    private bool bMoveManagmentIsBesy = false;

    /// <summary>
    /// 触れた玉を一時的に複製する用の変数
    /// </summary>
    private GameObject Copy = null;

    private Transform copyTr = null;

    /// <summary>
    /// 触れた玉に当たった玉を一時的に複製する用の変数
    /// </summary>
    private GameObject ChangeBallCopy;

    private Transform ChangeBallCopyTr;

    /// <summary>
    /// 触れた玉と触れた玉に当たった玉を一時的に入れ替えるための変数
    /// </summary>
    private Vector3 WorkCopyPos = Vector3.zero;

    /// <summary>
    /// 触れた玉に当たった玉を一時的に複製する用の変数の生殺与奪を管理
    /// </summary>
    private bool bCopy = false;

    /// <summary>
    /// 触れられた実体を保管しておく
    /// </summary>
    private BallStatus SaveEntity = null;

    [SerializeField] private BallCreate Ballcreate = null;
    [SerializeField] private DeleteCheck Deletecheck = null;

    /// <summary>
    /// コルーチン実行中の多重実行防止。
    /// フラグがfalseの時だけ実行する。
    /// </summary>
    private void Update()
    {
        if (!bMoveManagmentIsBesy)
            BlockMove();
    }

    /// <summary>
    /// タッチ入力開始と終了処理。
    /// タッチされた場所をスクリーン座標からワールド座標に変換し、触られた玉を特定。
    /// 触られた玉の誤動作防止のため、遊びチェック用の変数を作成。
    /// コルーチン実行
    /// </summary>
    public void BlockMove()
    {
        float MovementX, MovementY;
        MovementX = MovementY = 0.0f;

        KeyDownMoment();
        PushKeyMoment();
        KeyUpMoment(MovementX, MovementY);
    }

    /// <summary>
    /// タップされた瞬間orクリックされた瞬間の処理
    /// </summary>
    public void KeyDownMoment()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Input.mousePosition.zは0.0ｆが入る
            MoveStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

            Ray ray = new Ray(Camera.main.ScreenPointToRay(MoveStart).origin, new Vector3(0, 0, 1));

            if (Physics.Raycast(ray, out RaycastHit hit, 1.56f))
            {
                // Debug.Log(hit.point);
                for (int j = 0; j < 6; j++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (Ballcreate.BlockList[i, j].transform.position == hit.collider.gameObject.transform.position)
                        {
                            // SaveMousePositionは押し込み判定の最中の比較に使用する
                            SaveMousePosition = Ballcreate.BlockList[i, j].transform.position;

                            // 複製を生成
                            Copy = Instantiate(Ballcreate.BlockList[i, j].gameObject);
                            Copy.name = "CopyBall";
                            copyTr = Copy.transform;

                            // 複製されている玉の色を少し暗くする
                            Copy.GetComponent<Renderer>().material.color = Ballcreate./*GetComponent<BallStatus>().*/GetComponent<Renderer>().material.color = new Color32(128, 128, 128, 255);

                            // 実体の保存と非表示
                            SaveEntity = Ballcreate.BlockList[i, j];
                            Ballcreate.BlockList[i, j].gameObject.SetActive(false);

                            ArrayNoX = i;
                            ArrayNoY = j;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// タップされている間orクリックされている間の処理
    /// </summary>
    public void PushKeyMoment()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Copy != null)
        {
            // マウスの座標をワールド座標に変換してMousePosに格納しておく
            var MousePos = Input.mousePosition;
            MousePos = Camera.main.ScreenToWorldPoint(MousePos);

            // 複製した玉の座標更新
            copyTr.position = MoveTurn(MousePos, SaveMousePosition, 1.0f);

            // 入れ替え先の玉の情報(配列番号やポジション)を取得するためのループ
            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    // 持っている玉がほかの玉の座標と重なったとき
                    if (Ballcreate.BlockList[i, j].transform.position == new Vector3(copyTr.position.x, copyTr.position.y, 0.0f))
                    {
                        // 入れ替え先の玉の複製は削除するまで1度きり
                        if (!bCopy)
                        {
                            bCopy = true;

                            // WorkCopyPosに複製移動先のポジションを入れる
                            WorkCopyPos = Ballcreate.BlockList[i, j].transform.position;

                            // 移動先の複製を生成する
                            ChangeBallCopy = Instantiate(Ballcreate.BlockList[i, j].gameObject);
                            ChangeBallCopy.name = "ggwp";

                            // 移動先の複製のトランスを保存・セット
                            ChangeBallCopyTr = ChangeBallCopy.transform;
                            //ChangeBallCopyTr.position.Set(SaveMousePosition.x, SaveMousePosition.y, -1.5f);

                            // SaveMousePositionは最初に触った玉の座標が格納されている。
                            // 入れ替えのために複製した玉の座標にSaveMousePositionを代入して、入れ替えを実現する
                            ChangeBallCopyTr.position = new Vector3(SaveMousePosition.x, SaveMousePosition.y, -1.0f);
                        }
                    }
                }
            }

            // プレイヤーが移動先を変えたり変なところで指を離した場合
            if (WorkCopyPos != new Vector3(copyTr.position.x, copyTr.position.y, 0.0f))
            {
                if (bCopy)
                {
                    // 入れ替えるために複製した玉を削除
                    Destroy(ChangeBallCopy);
                    bCopy = false;
                }
            }
        }
    }

    /// <summary>
    /// 4方向への移動制限
    /// </summary>
    /// <param name="mouse">MousePos</param>
    /// <param name="saveMouse">SaveMousePosition</param>
    /// <param name="value">1.0f(移動量)</param>
    /// <returns>複製した玉の更新座標</returns>
    public Vector3 MoveTurn(Vector3 mouse, Vector3 saveMouse, float value)
    {
        if (mouse.x >= saveMouse.x + value)
        {
            mouse.x = saveMouse.x + value;
            mouse.y = saveMouse.y;
        }
        else if (mouse.x <= saveMouse.x - value)
        {
            mouse.x = saveMouse.x - value;
            mouse.y = saveMouse.y;
        }
        else if (mouse.y >= saveMouse.y + value)
        {
            mouse.x = saveMouse.x;
            mouse.y = saveMouse.y + value;
        }
        else if (mouse.y <= saveMouse.y - value)
        {
            mouse.x = saveMouse.x;
            mouse.y = saveMouse.y - value;
        }

        if (mouse.x < -3.0f || mouse.y < 2.9f || mouse.x > 3.0f || mouse.y > 8.5f)
        {
            mouse.x = saveMouse.x;
            mouse.y = saveMouse.y;
        }

        return new Vector3(mouse.x, mouse.y, -value);
    }

    /// <summary>
    /// タップが離された瞬間orクリックが離された瞬間の処理
    /// Movemanagmentコルーチンの開始
    /// </summary>
    /// <param name="movementX">X方向への移動量</param>
    /// <param name="movementY">Y方向への移動量</param>
    public void KeyUpMoment(float movementX, float movementY)
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //複製削除
            if (Copy != null)
                Destroy(Copy);

            if (ChangeBallCopy != null)
            {
                // 何もせずに話された場合複製削除
                Destroy(ChangeBallCopy);
                bCopy = false;
            }

            // 実体の再表示
            if (SaveEntity != null)
                SaveEntity.gameObject.SetActive(true);

            MoveEnd = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

            movementX = MoveEnd.x - MoveStart.x;
            movementY = MoveEnd.y - MoveStart.y;

            StartCoroutine(MoveManagemant(movementX, movementY));
        }
    }

    /// <summary>
    /// 玉を動させるかどうかをつかさどるマネージャー
    /// 多重動作防止フラグを管理している。
    /// 玉を動かせる場合のみ削除判定処理実行を命令する。
    /// </summary>
    /// <param name="MovementX">X方向への移動認識変数</param>
    /// <param name="MovementY">Y方向への移動認識変数</param>
    /// <returns>コルーチン実行</returns>
    private IEnumerator MoveManagemant(float MovementX, float MovementY)
    {
        bMoveManagmentIsBesy = true;

        // 左右入力
        if (Mathf.Abs(MovementX) > Mathf.Abs(MovementY))
        {
            // どっち入力
            if (MovementX > 20)
            {
                // Rayにて衝突判定が発生していないときはArrayNoに-1が格納されたままになっている
                if (ArrayNoX != 5 && ArrayNoX != -1 && ArrayNoY != -1)
                {
                    NewX = ArrayNoX + 1;
                    NewY = ArrayNoY;

                    if (CanMovement())
                    {
                        Movement();
                        yield return StartCoroutine(PrimaryProcess());
                    }

                    ArrayNoX = ArrayNoY = NewX = NewY = -1;
                }
            }

            if (MovementX < -20)
            {
                // Rayにて衝突判定が発生していないときはArrayNoに-1が格納されたままになっている
                if (ArrayNoX != 0 && ArrayNoX != -1 && ArrayNoY != -1)
                {
                    NewX = ArrayNoX - 1;
                    NewY = ArrayNoY;

                    if (CanMovement())
                    {
                        Movement();
                        yield return StartCoroutine(PrimaryProcess());
                    }

                    ArrayNoX = ArrayNoY = NewX = NewY = -1;
                }
            }
        }
        else
        {
            // 上下
            if (MovementY > 20)
            {
                // Rayにて衝突判定が発生していないときはArrayNoに-1が格納されたままになっている
                if (ArrayNoY != 0 && ArrayNoX != -1 && ArrayNoY != -1)
                {
                    NewX = ArrayNoX;
                    NewY = ArrayNoY - 1;

                    if (CanMovement())
                    {
                        Movement();
                        yield return StartCoroutine(PrimaryProcess());
                    }

                    ArrayNoX = ArrayNoY = NewX = NewY = -1;
                }
            }

            if (MovementY < -20)
            {
                // Rayにて衝突判定が発生していないときはArrayNoに-1が格納されたままになっている
                if (ArrayNoY != 5 && ArrayNoX != -1 && ArrayNoY != -1)
                {
                    NewX = ArrayNoX;
                    NewY = ArrayNoY + 1;

                    if (CanMovement())
                    {
                        Movement();
                        yield return StartCoroutine(PrimaryProcess());
                    }

                    ArrayNoX = ArrayNoY = NewX = NewY = -1;
                }
            }
        }

        bMoveManagmentIsBesy = false;
    }

    /// <summary>
    /// 指を離した位置に動かす
    /// </summary>
    private void Movement()
    {
        var WorkArray = Ballcreate.BlockList[NewX, NewY];
        var Workpos = Ballcreate.BlockList[NewX, NewY].transform.position;

        Ballcreate.BlockList[NewX, NewY].transform.position = Ballcreate.BlockList[ArrayNoX, ArrayNoY].transform.position;
        Ballcreate.BlockList[ArrayNoX, ArrayNoY].transform.position = Workpos;

        Ballcreate.BlockList[NewX, NewY] = Ballcreate.BlockList[ArrayNoX, ArrayNoY];
        Ballcreate.BlockList[ArrayNoX, ArrayNoY] = WorkArray;
    }

    /// <summary>
    /// 動かした先で削除できる玉があるかチェックする
    /// </summary>
    /// <returns>trueの時はMovement()へ</returns>
    private bool CanMovement()
    {
        var WorkArray = Ballcreate.BlockList[NewX, NewY];

        Ballcreate.BlockList[NewX, NewY] = Ballcreate.BlockList[ArrayNoX, ArrayNoY];
        Ballcreate.BlockList[ArrayNoX, ArrayNoY] = WorkArray;
        var ret = Check();

        WorkArray = Ballcreate.BlockList[NewX, NewY];
        Ballcreate.BlockList[NewX, NewY] = Ballcreate.BlockList[ArrayNoX, ArrayNoY];
        Ballcreate.BlockList[ArrayNoX, ArrayNoY] = WorkArray;

        return ret;
    }

    /// <summary>
    /// 消える玉チェック
    /// </summary>
    private bool Check()
    {
        // 再探査
        for (int j = 0; j < 6; j++)
        {
            for (int i = 0; i < 6; i++)
            {
                bLoop = Deletecheck.DeleteBlockCheck(Ballcreate, i, j);
            }
        }

        return bLoop;
    }

    /// <summary>
    /// 大きなシステムの動き。
    /// </summary>
    /// <returns>遅延フレーム数待機</returns>
    private IEnumerator PrimaryProcess()
    {
        // 動かす玉と動かされた玉のチェック
        bLoop = Deletecheck.DeleteBlockCheck(Ballcreate, NewX, NewY);
        bLoop = Deletecheck.DeleteBlockCheck(Ballcreate, ArrayNoX, ArrayNoY);

        while (bLoop == true)
        {
            Delete();

            yield return new WaitForSeconds(0.3f);
            // 補充処理
            var c = Refill();
            yield return c;

            Check();

            yield return new WaitForSeconds(0.3f);
        }

        // ここにまだ動かせるかチェック
        // @todo ランダム生成からの４方向の色チェックからのNumber変更で重い処理が改善できる。
        while (!MoveCheck())
        {
            Debug.Log("逆にすごい");
            yield return new WaitForSeconds(2.0f);
            Deletecheck.AllDelete(Ballcreate);

            yield return new WaitForSeconds(1.0f);
            // bool値の結果に応じて全部の玉を再補充
            Ballcreate.StartGame();
        }
    }

    /// <summary>
    /// 消える玉チェック
    /// </summary>
    public bool MoveCheck()
    {
        bool ret = false;

        // 再探査
        for (int j = 2; j < Ballcreate.BlockListWork.GetLength(0) - 2; j++)
        {
            for (int i = 2; i < Ballcreate.BlockListWork.GetLength(1) - 2; i++)
            {
                if (TempMoveCheck(i, j, i, j - 1) ||
                    TempMoveCheck(i, j, i, j + 1) ||
                    TempMoveCheck(i, j, i + 1, j) ||
                    TempMoveCheck(i, j, i - 1, j))
                {
                    ret = true;
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// BlockListWork配列の仮入れ替え
    /// </summary>
    /// <param name="fromI">i</param>
    /// <param name="fromJ">j</param>
    /// <param name="toI">iの移動先</param>
    /// <param name="toJ">jの移動先</param>
    /// <returns>移動できますかフラグ</returns>
    public bool TempMoveCheck(int fromI, int fromJ, int toI, int toJ)
    {
        bool ret = false;
        Ballcreate.CreateBlockListWork();

        if (Ballcreate.BlockListWork[toI, toJ] != null)
        {
            var WorkArray = Ballcreate.BlockListWork[toI, toJ];

            Ballcreate.BlockListWork[toI, toJ] = Ballcreate.BlockListWork[fromI, fromJ];
            Ballcreate.BlockListWork[fromI, fromJ] = WorkArray;

            ret = Deletecheck.AfterMoveCheck(Ballcreate.BlockListWork, toI, toJ);
        }

        return ret;
    }

    /// <summary>
    /// 削除
    /// </summary>
    private void Delete()
    {
        Deletecheck.PlzDelete(Ballcreate);
    }

    /// <summary>
    /// 補充
    /// 配列のどこに補充したかを格納
    /// </summary>
    public Coroutine Refill()
    {
        // 補充処理
        GetArrayNumber = Ballcreate.RefillBlock(out var generatedTargetPos);
        // 配列の補充要素取得用
        var work = GetArrayNumber;

        // リスト保管用
        var generatedBallTr = new List<Transform>();

        for (int i = 0; i < work.Count; i++)
        {
            // 補充要素情報取得
            var ballIdx = GetArrayNumber[i];
            generatedBallTr.Add(Ballcreate.BlockList[ballIdx[0], ballIdx[1]].transform);
        }

        // 補充アニメーションコルーチン実行
        var ret = StartCoroutine(refill.MoveToTargetPosition(generatedBallTr, generatedTargetPos));
        return ret;
    }
}