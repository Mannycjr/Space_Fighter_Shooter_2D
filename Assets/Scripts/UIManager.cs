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
    private Text _ammoText;
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
    [SerializeField]
    private Slider _thrustersSlider;
    [SerializeField]
    private Image _thrustersSliderFill;

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
        _scoreText.text = "SCORE: " + playerScore.ToString();
    }

    // Update ammo display
    // Feature: Ammo Count
    public void UpdateAmmo(int playerAmmo)
    {
        _ammoText.text = "AMMO: " + playerAmmo.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _livesSprites[currentLives];
        if (currentLives < 1)
        {
            GameOverSequence();
        }
    }

    // Feature: Shield Strength
    // ● Visualize the strength of the shield.This can be done through UI on screen or color changing of the shield.
    // ● Allow for 3 hits on the shield to accommodate visualization
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

    public void UpdateThrustersSlider(float thrustValue)
    {
        if (thrustValue >= 0 && thrustValue <= 10)
        {
            _thrustersSlider.value = thrustValue;

        }
    }

    public void ThurstersSliderUsableColor(bool usableThrusters)
    {
        if (usableThrusters)
        {
            _thrustersSliderFill.Color = Color.green;
        }
        else if (!usableThrusters)
        {
            _thrustersSliderFill.Color = Color.red;
        }
    }
}
