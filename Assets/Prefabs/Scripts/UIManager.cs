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
    [SerializeField]
    public  Slider thrusterBar;
    [SerializeField] GameObject _shiled;
    [SerializeField] private Image[] _shiledImages;
    [SerializeField] private Player _player;
    [SerializeField] private Text _ammoTxt;


    // Start is called before the first frame update
    void Start()
    {
        
        _gameOver.SetActive(false);
        _restartText.SetActive(false);
        _shiled.SetActive(false);
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

    public void ThrusterBar(float amount)
    {
        thrusterBar.value = amount;
        
    }

    public void ShiledOn()
    {
        _shiled.SetActive(true);
        _shiledImages[0].enabled = true;
        _shiledImages[1].enabled = true;
        _shiledImages[2].enabled = true;

    }
    public void ShiledOff()
    {
        _shiled.SetActive(false);
      

    }
    public void ChangeShieldImage(int num)
    {
        


        if ( num >0 )
        {
            _shiledImages[num].enabled = false;
        }
        else if(num == 0)
        {
            _shiledImages[num].enabled = false;
            _shiled.SetActive(false);
            _player.ShiledDameged();
        }
        

    }

    public void RefreshAmmo(int amount)
    {
        if (amount <= 0)
        {
            _ammoTxt.text = "Out Of Ammo";
            
        }
        else 
        {
            _ammoTxt.text = amount.ToString();
        }

       
    }
}
