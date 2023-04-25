 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menuManagerinstance;
    public bool gameState;
    public GameObject menuElement;
    public GameObject menuElement1;
    public GameObject GameOver;
    public GameObject Finish;
    void Start()
    {
        gameState = false;
        menuManagerinstance = this;
        menuElement1.GetComponent<Text>().text=PlayerPrefs.GetInt("score").ToString();
    }


    void Update()
    {
        
    }
    public void startTheGame()
    {
        gameState= true;
        menuElement.SetActive(false);
        GameObject.FindWithTag("particle").GetComponent<ParticleSystem>().Play();
    }
    public void reStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
