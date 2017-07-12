using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Text score;
    public Text timer;

    public PlayerController player;
    

    public float FinalScore;

    private int scoreNumber;
    private float startTime;
    private float currentTime;
    private int lifes = 5;

    private static GameManager gameManager;

    public static GameManager instance
    {
        get
        {
            if (!gameManager)
            {
                gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (!gameManager)
                {
                    Debug.Log("There isnt exists an AudioManager on a GameObject in your scene");
                }
            }
            return gameManager;
        }
    }


    private void Start()
    {
        startTime = Time.time;
        StartCoroutine(ShowScore());
        
    }

    private void Update()
    {
        currentTime = Time.time - startTime;
        string minutes = ((int)currentTime / 60).ToString();
        string seconds = (currentTime % 60).ToString("f2");

        
        timer.text = minutes + ":" + seconds; 
    }

    public static void CoinAdded(int score)
    {
        instance.scoreNumber += score;
        instance.score.text = instance.scoreNumber.ToString();
    }


    IEnumerator ShowScore()
    {
        while(true)
        {
            yield return new WaitForSeconds(3);
            instance.scoreNumber --;
            instance.score.text = instance.scoreNumber.ToString();
        }
    }

    public static void EnterTrap(int damage)
    {
        instance.scoreNumber = instance.scoreNumber * damage / 100;
        instance.score.text = instance.scoreNumber.ToString();
        instance.lifes--;
        instance.player.GetHit();

        if (instance.lifes <= 0)
        {
            instance.scoreNumber = 0;
            NewEpisode();
        }
    }

    public static void NewEpisode()
    {
        //instance.data.WriteText(instance.scoreNumber);
        //instance.player.position = new Vector2(0, 0);

        //PlayerPrefs.SetInt("Episode", 0);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        
    }
    
    
    
}
