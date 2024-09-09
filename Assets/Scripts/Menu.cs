using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Menu : MonoBehaviour
{
    public string playerName;

    public static Menu menu;

    [SerializeField] public static TMP_InputField nameField;

    // Start is called before the first frame update
    void Start()
    {
        menu = this;
    }

    // Update is called once per frame
    void Update()
    {
        NameSelector();
    }

    //The name of the player is selected
    void NameSelector()
    {
        playerName = nameField.GetComponent<TMP_InputField>().text;
    }

    //The game starts
    public void StartGame()
    {
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
