using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    private UIManager _uiManagerScript;

    private SpawnManager _spawnManager;
    private int _waveID = 0;
    private float _waveTime = 20.0f;
    private float _holdtime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManagerScript = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("GameManager::Start() Called. The Spawn Manager is NULL.");
        }

        if (_uiManagerScript == null)
        {
            Debug.LogError("GameManager::Start() Called. The UI Manager is NULL.");
        }

    }

    private void Update()
    {
        //if the r key ws pressed
        //restart and load the scene
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1); //Current Game Scene
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        Debug.Log("GameManager::GameOver() Called");
        _isGameOver = true;
    }

    public void StartSpawning()
    {
        _waveID++;
        _waveTime += 10;
        
        if (_waveID > 10)
        {
            Debug.Log("You Win!");
            return;
        }

        _uiManagerScript.WaveIDUpdate(_waveID);
        _spawnManager.StartSpawning(_waveID);
        _uiManagerScript.WaveTimeUpdate(_waveTime);
        StartCoroutine(WaveCountdown(_waveTime));
    }

    private IEnumerator WaveCountdown(float _time)
    {
        while(_time > 0)
        {
            _time -= Time.deltaTime;
            _uiManagerScript.WaveTimeUpdate(_time);
            yield return new WaitForEndOfFrame();
        }
        _spawnManager.stopSpawning();

        //yield return _holdtime;
        StartSpawning();

    }

}
