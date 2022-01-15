using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreText;
    public Text highscoreText;
    public Text lifeText;
    public Text gameOverText;

    string conn = "Data Source=DB.s3db;Cache=Shared"; //Path to database.
    string sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    int score = 0;
    int highscore = 0;
    int life = 3;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", highscore);
        scoreText.text = score.ToString() + " POINTS";
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
        lifeText.text = "LIFE: " + life.ToString();
        gameOverText.text = "";
    }

    public void AddPoint()
    {
        score += 1;
        scoreText.text = score.ToString() + " POINTS";

        if(score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
        }
    }

    public void LoseLife()
    {
        life -= 1;
        lifeText.text = "LIFE: " + life.ToString();

        if(life == 0)
        {
            gameOverText.text = "GAME OVER";
            int id = PlayerPrefs.GetInt("id");
            int newscore = PlayerPrefs.GetInt("highscore", score);
            dbconn = new SqliteConnection(conn);
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            sqlQuery = "UPDATE USER " +  " SET highscore = " + newscore.ToString() + " WHERE id = " + id.ToString();
            //Debug.Log(sqlQuery);
            //dbconn.Open();
            dbcmd = dbconn.CreateCommand();
            /*        string sqlQuery = "CREATE TABLE IF NOT EXISTS [USER] (" +
                        "[id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT," +
                        "[name] VARCHAR(255)  NOT NULL," +
                        "[highscore] INTEGER DEFAULT '0' NOT NULL)";*/
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();
            StartCoroutine(waiter());
        }
    }

    IEnumerator waiter()
    {
        Time.timeScale = 0;
        //yield on a new YieldInstruction that waits for 10 seconds.
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu Scene 1");
    }
}
