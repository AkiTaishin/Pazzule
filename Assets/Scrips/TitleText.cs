using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleText : MonoBehaviour
{
    // タイトルテキスト
    [SerializeField] private Text Title = null;

    // Start is called before the first frame update
    private void Start()
    {
        Title.text = ("タイトルでっす!!");
    }

    // Update is called once per frame
    /// <summary>
    /// 遷移
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SceneManager.LoadScene("Ready");
        }

    }
}
