using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite; 
using System.Data; 
using System;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public TMP_InputField username;

    string conn = "Data Source=DB.s3db;Cache=Shared"; //Path to database.
    string sqlQuery;
    List<String> profiles = new List<String>();
    List<int> highscores = new List<int>();
    IDbConnection dbconn;
    IDbCommand dbcmd;
    public TMP_Dropdown dropdown1;

    private void CreateTable()
    {
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open();
            dbcmd = dbconn.CreateCommand();
            sqlQuery = "CREATE TABLE IF NOT EXISTS USER (" +
                "id INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT," +
                "name VARCHAR(255)  NOT NULL," +
                "highscore INTEGER DEFAULT '0' NOT NULL)";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();
            //dbconn.Close();
        }
    }

    void Start() 
    {
        CreateTable();
       
     dbconn = (IDbConnection) new SqliteConnection(conn);
     dbconn.Open(); //Open connection to the database.
     dbcmd = dbconn.CreateCommand();
     sqlQuery = "SELECT * FROM USER";
     dbcmd.CommandText = sqlQuery;
     //Debug.Log(sqlQuery);
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int id = Convert.ToInt32(reader[0]);
            string name = reader[1].ToString();
            int highscore = Convert.ToInt32(reader[2]);
            Debug.Log(highscore.ToString());
            profiles.Add(name);
            highscores.Add(highscore);
            //Debug.Log( "  name =" + name );
        }
        reader.Close();
        reader = null;
        //dbcmd.Dispose();
        //dbcmd = null;
        //dbconn.Close();
        //dbconn = null;
    }

    public void AddUser()
    {
        CreateTable();
        //string conn = "URI=file:" + Application.dataPath + "/DB.s3db"; //Path to database.
        //IDbConnection dbconn;
        dbconn = new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        sqlQuery = "INSERT INTO USER (name, highscore) VALUES ( '\''"+ username.text + "'\'', 0) ";
        //Debug.Log(sqlQuery);
        //dbconn.Open();
        dbcmd = dbconn.CreateCommand();
/*        string sqlQuery = "CREATE TABLE IF NOT EXISTS [USER] (" +
            "[id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT," +
            "[name] VARCHAR(255)  NOT NULL," +
            "[highscore] INTEGER DEFAULT '0' NOT NULL)";*/
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteScalar();
        //dbconn.Close();
        SceneManager.LoadScene("Menu Scene 1");
    }

    public void PlayGame()
    {
        int id = PlayerPrefs.GetInt("id", -1);
        if( id != -1)
        {
            SceneManager.LoadScene("Carina'sScene");
        }
            
    }

    public void GoToAddUser()
    {
        SceneManager.LoadScene("AddUser Scene");
    }

    public void GoToProfiles()
    {

        //SceneManager.LoadScene("Profiles Scene");

        SceneManager.LoadScene("Profiles Scene");
    }

    public void ShowProfiles()
    {
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        dbcmd = dbconn.CreateCommand();
        sqlQuery = "SELECT * FROM USER";
        dbcmd.CommandText = sqlQuery;
        //Debug.Log(sqlQuery);
        IDataReader reader = dbcmd.ExecuteReader();
        profiles = new List<String>();
        while (reader.Read())
        {
            int id = Convert.ToInt32(reader[0]);
            string name = reader[1].ToString();
            int highscore = Convert.ToInt32(reader[2]);
            //Debug.Log(highscore.ToString());
            profiles.Add(name);
            highscores.Add(highscore);
            //Debug.Log("  name =" + name);
        }
        reader.Close();
        reader = null;
        //var dropdown = GetComponent<Dropdown>();
        dropdown1.ClearOptions();
        dropdown1.AddOptions(profiles);
    }

    public void SelectProfile()
    {
        Debug.Log(highscores[dropdown1.value]);

        PlayerPrefs.SetInt("highscore", highscores[dropdown1.value]);
        PlayerPrefs.SetInt("id", dropdown1.value + 1);
        //Debug.Log(ScoreManager.instance.highscore);
        SceneManager.LoadScene("Menu Scene 1");
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("Menu Scene 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
