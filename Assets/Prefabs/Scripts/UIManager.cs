using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreTxt,_gameOverTxt;
    [SerializeField] private Sprite[] liveSprites;
    [SerializeField] private Image _liveImage;
    [SerializeField] private GameObject _gameOver;
    [SerializeField] private GameObject _restartText;
    // Start is called before the first frame update
    void Start()
    {
        _gameOver.SetActive(false);
        _restartText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshScore(int sAmount)
    {
        _scoreTxt.text = "Score : " + sAmount;
        
    }

    public void LivesChange(int health)
    {
        _liveImage.sprite = liveSprites[health];
        if (health <= 0)
        {
            GameOver();
        }
        
    }

    void GameOver()
    {
        _gameOver.SetActive(true);
        GameManager.Instance.GameOver();
        StartCoroutine(FlickerGameOver());
        _restartText.SetActive(true);
    }

    IEnumerator FlickerGameOver()
    {
        while (true)
        {
            _gameOverTxt.text = "Game Over!";
            yield return new WaitForSeconds(.5f);
            _gameOverTxt.text = "";
            yield return new WaitForSeconds(.5f);
        }
        

    }
}
