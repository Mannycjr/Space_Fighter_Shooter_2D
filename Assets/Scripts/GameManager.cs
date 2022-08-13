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
    public int _waveID = 0; // Other scripts need to access wave level number
    private float _waveTime = 5.0f;
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
        Debug.Log("GameManager::StartSpawning() Called");
        _waveID++;
        _waveTime += 10;

        /*        */
        if (_waveID > 5)
        {
            Debug.Log("You Win!");
            return;
        }

        _uiManagerScript.WaveDisplayOn();
        _uiManagerScript.WaveIDUpdate(_waveID);
        StartCoroutine(WaveCountdown(_waveTime));
        _spawnManager.StartSpawning(_waveID);
    }

    private IEnumerator WaveCountdown(float _time)
    {
        while(_time > 0)
        {
            _time -= Time.deltaTime;
            _uiManagerScript.WaveTimeUpdate(_time);
            yield return new WaitForEndOfFrame();
        }
        _spawnManager.StopSpawning();

        yield return _holdtime;
        StartSpawning();

    }

}
