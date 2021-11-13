using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // handle to Text in UI
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _GameOverText;
    private float _textFlickerDelay = 0.25f;
    [SerializeField]
    private Text _RestartText;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore(0);
        _GameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    // Update is called once per frame
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _GameOverText.gameObject.SetActive(true);
        _RestartText.gameObject.SetActive(true);
        StartCoroutine(GameOverTextFlickerRoutine());
    }

    IEnumerator GameOverTextFlickerRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(_textFlickerDelay);
            _GameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(_textFlickerDelay);
            _GameOverText.gameObject.SetActive(true);
        }

    }

}
