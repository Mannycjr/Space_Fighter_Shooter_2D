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
    private Sprite[] _livesSprites;
    [SerializeField]
    private Text _GameOverText;
    private float _textFlickerDelay = 0.25f;
    [SerializeField]
    private Text _RestartText;
    [SerializeField]
    private GameObject _shieldsStrength;
    [SerializeField]
    private GameObject[] shieldsStrengthIcons;

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

    // Update score display
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives >= 1)
        {
            _LivesImg.sprite = _livesSprites[currentLives];
        }
        else if (currentLives < 1)
        {
            GameOverSequence();
        }
    }

    public void UpdateShieldsStrength(int currentShieldStrength)
    {
        switch (currentShieldStrength)
        {
            case 3:
                _shieldsStrength.SetActive(true);
                for (int i = 0; i < shieldsStrengthIcons.Length; i++)
                {
                    shieldsStrengthIcons[i].SetActive(true);
                }
                break;
            case 2:
                shieldsStrengthIcons[2].SetActive(false);
                break;
            case 1:
                shieldsStrengthIcons[1].SetActive(false);
                break;
            default:
                _shieldsStrength.SetActive(false);
                break;
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
