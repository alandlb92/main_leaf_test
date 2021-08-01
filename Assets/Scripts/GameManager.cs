using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ArcherEnemyController _archerReference;
    [SerializeField] private MeleeEnemyController _meleeReference;
    [Range(5, 20)]
    [SerializeField] private int _maxMeleesInstaces = 5;
    [Range(5, 20)]
    [SerializeField] private int _maxArcherInstaces = 10;
    [Range(1, 10)]
    [SerializeField] private int _nonRepetedSpawns = 5;
    [SerializeField] private float _gameTimeMinutes = 3;

    private AmbientAudioController _ambientAudioController;
    private PlayerController _playerController;
    private WavesScriptable _waveConfig;

    private StartScreenUI _startScreenUI;
    private HudUI _hudUI;
    private WarningScreenUI _warningScreenUI;
    private GamePauseUI _gamePauseUI;
    private GameOverUI _gameOverUI;
    private LoadingUI _loadingUI;

    private Transform _playerStartPoint;
    private List<Transform> _spawnPoints = new List<Transform>();
    private List<Transform> _usedSpanwsPoints = new List<Transform>();

    private List<ArcherEnemyController> _archers = new List<ArcherEnemyController>();
    private List<MeleeEnemyController> _melees = new List<MeleeEnemyController>();

    private int _currentWave = 0;
    private int _currentMeleeInstance = 0;
    private int _currentArcherInstance = 0;
    private bool _buildingWave;
    private bool _gameplayStarted;
    private bool _waitingWaveWarningInterface;
    private bool _countTime;
    private float _time;

    private int _archerKilled;
    private int _meleeKilled;

    public void PlayAgain()
    {
        _loadingUI.Open();
        _currentWave = 0;
        _currentMeleeInstance = 0;
        _currentArcherInstance = 0;
        _archerKilled = 0;
        _meleeKilled = 0;
        _playerController.RecoverLife();
        _playerController.RecoverArrows();
        _archers.ForEach(a => a.HideBody());
        _melees.ForEach(m => m.HideBody());
        _playerController.Reset(_playerStartPoint);
        CorotineUtils.WaiSecondsAndExecute(this, 2, () =>
        {
            _loadingUI.Close();
            StartGame();
        });

    }

    public void StartGame()
    {
        _time = (_gameTimeMinutes * 60);
        _countTime = true;
        _hudUI.SetTime(_time);
        _gameplayStarted = true;
        Cursor.visible = false;
        CorotineUtils.WaitEndOfFrameAndExecute(this, () =>
        {
            _playerController.Enable(true);
        });
        _hudUI.Open();
        NewWave();
    }

    public void PauseGame()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            _gamePauseUI.Close();
            Cursor.visible = false;
            CorotineUtils.WaitEndOfFrameAndExecute(this, () =>
            {
                _playerController.Enable(true);
            });
        }
        else
        {
            Time.timeScale = 0;
            _gamePauseUI.Open();
            Cursor.visible = true;
            _playerController.Enable(false);
        }
    }

    public void PlayerDie()
    {
        GameOver();
    }

    private void GameOver()
    {
        _countTime = false;
        _gameOverUI.ShowGameOver(_archerKilled, _meleeKilled);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        _loadingUI = FindObjectOfType<LoadingUI>();
        _loadingUI.Open();
        _spawnPoints = GameObject.FindGameObjectsWithTag("Respawn").Select(go => go.transform).ToList();
        _spawnPoints.ForEach(s => s.GetComponent<MeshRenderer>().enabled = false);
        _waveConfig = Resources.Load<WavesScriptable>("WavesConfig");
        _playerController = FindObjectOfType<PlayerController>();
        _hudUI = FindObjectOfType<HudUI>();
        _warningScreenUI = FindObjectOfType<WarningScreenUI>();
        _ambientAudioController = FindObjectOfType<AmbientAudioController>();
        _gamePauseUI = FindObjectOfType<GamePauseUI>();
        _gameOverUI = FindObjectOfType<GameOverUI>();
        _startScreenUI = FindObjectOfType<StartScreenUI>();
        _playerStartPoint = GameObject.FindGameObjectWithTag("PlayerStart").transform;
        _playerStartPoint.GetComponent<MeshRenderer>().enabled = false;

        CorotineUtils.WaiSecondsAndExecute(this, 2, () =>
        {
            _loadingUI.Close();
            _startScreenUI.Open();
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _gameplayStarted && !_waitingWaveWarningInterface && !_playerController.IsDead)
        {
            PauseGame();
        }

        if (_countTime && !_waitingWaveWarningInterface && !_playerController.IsDead)
        {
            _time -= Time.deltaTime;
            if (_time <= 0)
            {
                _playerController.TakeDamage(100);
                _time = 0;
            }
            _hudUI.SetTime(_time);
        }
    }

    private void OnEnemyDie(BaseEnemyController.Type enemyType)
    {
        if (_buildingWave)
            return;

        if (enemyType == BaseEnemyController.Type.ARCHER)
            _archerKilled++;
        else
            _meleeKilled++;

        int aliveEnemies = _melees.FindAll(c => !c.IsDead).Count() + _archers.FindAll(c => !c.IsDead).Count();
        if (aliveEnemies == 0)
        {
            NewWave();
        }
    }

    private void NewWave()
    {
        _waitingWaveWarningInterface = true;
        _buildingWave = true;
        CorotineUtils.WaiSecondsAndExecute(this, 3, () =>
        {
            _warningScreenUI.ShowWarnning(_waveConfig.WaveConfigs[_currentWave].Name,
                () =>
                {
                    _waitingWaveWarningInterface = false;
                    StartCoroutine(StartWave(_waveConfig.WaveConfigs[_currentWave]));
                    _currentWave++;
                    if (_currentWave > _waveConfig.WaveConfigs.Length - 1)
                        _currentWave = _waveConfig.WaveConfigs.Length - 1;
                }, OnChooseLifeOrArrows, _currentWave != 0);
        });
    }

    private void SpawMelee(Transform spawnPoint, int count)
    {
        if (_melees.Count < _maxMeleesInstaces)
        {
            MeleeEnemyController instance = Instantiate(_meleeReference);
            instance.transform.position = spawnPoint.transform.position;
            instance.transform.rotation = spawnPoint.transform.rotation;
            instance.Initialize(OnEnemyDie, count);
            _melees.Add(instance);
        }
        else
        {
            _melees[_currentMeleeInstance].transform.position = spawnPoint.transform.position;
            _melees[_currentMeleeInstance].transform.rotation = spawnPoint.transform.rotation;
            _melees[_currentMeleeInstance].Initialize(OnEnemyDie, count);

            _currentMeleeInstance++;
            if (_currentMeleeInstance > _melees.Count - 1)
                _currentMeleeInstance = 0;
        }
    }


    private void SpawArcher(Transform spawnPoint, int count)
    {
        if (_archers.Count < _maxArcherInstaces)
        {
            ArcherEnemyController instance = Instantiate(_archerReference);
            instance.transform.position = spawnPoint.transform.position;
            instance.transform.rotation = spawnPoint.transform.rotation;
            instance.Initialize(OnEnemyDie, count);
            _archers.Add(instance);
        }
        else
        {
            _archers[_currentArcherInstance].transform.position = spawnPoint.transform.position;
            _archers[_currentArcherInstance].transform.rotation = spawnPoint.transform.rotation;
            _archers[_currentArcherInstance].Initialize(OnEnemyDie, count);

            _currentArcherInstance++;
            if (_currentArcherInstance > _archers.Count - 1)
                _currentArcherInstance = 0;
        }
    }

    private Transform GetSpawnPoint()
    {
        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
        _spawnPoints.Remove(spawnPoint);
        _usedSpanwsPoints.Add(spawnPoint);
        if (_usedSpanwsPoints.Count > _nonRepetedSpawns)
        {
            Transform usedToReturn = _usedSpanwsPoints[0];
            _usedSpanwsPoints.Remove(usedToReturn);
            _spawnPoints.Add(usedToReturn);
        }
        return spawnPoint;
    }

    private IEnumerator StartWave(WaveConfig waveConfig)
    {
        _ambientAudioController.StartWave();
        for (int i = 0; i < waveConfig.Archer; i++)
        {
            SpawArcher(GetSpawnPoint(), i);
            yield return new WaitForSeconds(1);
        }

        for (int i = 0; i < waveConfig.Melee; i++)
        {
            SpawMelee(GetSpawnPoint(), i);
            yield return new WaitForSeconds(1);
        }
        _buildingWave = false;
    }

    private void OnChooseLifeOrArrows(WarningScreenUI.Choose choose)
    {
        if (choose == WarningScreenUI.Choose.LIFE)
        {
            _playerController.RecoverLife();
        }
        else
        {
            _playerController.RecoverArrows();
        }
    }
}
