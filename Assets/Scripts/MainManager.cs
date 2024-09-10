using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    public static int bestScore;
    
    private bool m_GameOver = false;
    public static string bestScorePlayerName;
    private string playerName;
    [SerializeField] private Button resetBestScoreButton;

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        bestScoreText.gameObject.SetActive(false);
        if(UIManager.UIManagerScript != null)
        {
            playerName = UIManager.UIManagerScript.playerName;
        }
        resetBestScoreButton.gameObject.SetActive(false);
        LoadBestScore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        UpdateBestScore();
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    //The best score is updated when the previous one is braked
    void UpdateBestScore()
    {
        if (m_Points > bestScore)
        {
            bestScore = m_Points;
            bestScorePlayerName = playerName;
            SaveBestScore();
        }
        if (UIManager.UIManagerScript != null)
        {
            LoadBestScore();
            SaveBestScore();
            bestScoreText.text = "Best score: " + bestScore + " Name: " + bestScorePlayerName;
        }
        
    }

    public void GameOver()
    {
        m_GameOver = true;
        bestScoreText.gameObject.SetActive(true);
        GameOverText.SetActive(true);
        resetBestScoreButton.gameObject.SetActive(true);
    }

    public void ResetBestScore()
    {
        bestScore = 0;
        SaveBestScore();
        LoadBestScore();
    }

    class SaveData
    {
        public string bestScorePlayerName;
        public int bestScore;
    }
    public void SaveBestScore()
    {
        SaveData data = new SaveData();
        data.bestScorePlayerName = bestScorePlayerName;
        data.bestScore = bestScore;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void LoadBestScore()
    {
        string path = Application.persistentDataPath +  "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScorePlayerName = data.bestScorePlayerName;
            bestScore = data.bestScore;
        }
    }
}
