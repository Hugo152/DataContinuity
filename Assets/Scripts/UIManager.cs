using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class UIManager : MonoBehaviour
{
    private bool isInMainScene;

    public string playerName;

    [SerializeField] private TMP_InputField nameField;

    public static UIManager UIManagerScript;

    // Start is called before the first frame update
    void Awake()
    {
        if(UIManagerScript == null)
        {
            UIManagerScript = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        isInMainScene = false;
    }

    // Update is called once per frame
    void Update()
    {
        NameSelector();
    }

    //The name of the player is selected
    void NameSelector()
    {
        if(isInMainScene == false)
        {
            playerName = nameField.text;

        }
    }

    //The game starts
    public void StartGame()
    {
        isInMainScene = true;
        SceneManager.LoadScene(1);
    }

    //The game is quit
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
