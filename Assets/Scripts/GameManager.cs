using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ArcherEnemyController _archerReference;
    [SerializeField] private MeleeEnemyController _meleeReference;

    private PlayerController _playerController;
    private WavesScriptable _waveConfig;
    private int _activeEnemies = 0;

    private List<Transform> _spawnPoints = new List<Transform>();
    private List<Transform> _usedSpanwsPoints = new List<Transform>();

    private List<ArcherEnemyController> _archers = new List<ArcherEnemyController>();
    private List<MeleeEnemyController> _melees = new List<MeleeEnemyController>();

    private int _currentMeleeInstance = 0;
    private int _currentArcherInstance = 0;

    private const int MAX_MELEES_INSTACES = 5;
    private const int MAX_ARCHER_INSTACES = 10;
    private const int NON_REPETED_SPAWNS = 3;

    public void StartGame()
    {
        Cursor.visible = false;
        _playerController.Enable(true);
    }
    private void Awake()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("Respawn").Select(go => go.transform).ToList();
        _spawnPoints.ForEach(s => s.GetComponent<MeshRenderer>().enabled = false);
        _waveConfig = Resources.Load<WavesScriptable>("WavesConfig");
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(SpawnEnemies(3, 0));
        }
    }

    private void OnEnemyDie()
    {
        _activeEnemies--;
    }

    private void SpawMelee(Transform spawnPoint)
    {
        if (_melees.Count < MAX_MELEES_INSTACES)
        {
            MeleeEnemyController instance = Instantiate(_meleeReference);
            instance.transform.position = spawnPoint.transform.position;
            instance.transform.rotation = spawnPoint.transform.rotation;
            instance.Initialize(OnEnemyDie);
            _melees.Add(instance);
        }
        else
        {
            _melees[_currentMeleeInstance].transform.position = spawnPoint.transform.position;
            _melees[_currentMeleeInstance].transform.rotation = spawnPoint.transform.rotation;
            _melees[_currentMeleeInstance].Initialize(OnEnemyDie);

            _currentMeleeInstance++;
            if (_currentMeleeInstance > _melees.Count - 1)
                _currentMeleeInstance = 0;
        }
    }


    private void SpawArcher(Transform spawnPoint)
    {
        if (_archers.Count < MAX_ARCHER_INSTACES)
        {
            ArcherEnemyController instance = Instantiate(_archerReference);
            instance.transform.position = spawnPoint.transform.position;
            instance.transform.rotation = spawnPoint.transform.rotation;
            instance.Initialize(OnEnemyDie);
            _archers.Add(instance);
        }
        else
        {
            _archers[_currentArcherInstance].transform.position = spawnPoint.transform.position;
            _archers[_currentArcherInstance].transform.rotation = spawnPoint.transform.rotation;
            _archers[_currentArcherInstance].Initialize(OnEnemyDie);

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
        if (_usedSpanwsPoints.Count > NON_REPETED_SPAWNS)
        {
            Transform usedToReturn = _usedSpanwsPoints[0];
            _usedSpanwsPoints.Remove(usedToReturn);
            _spawnPoints.Add(usedToReturn);
        }
        return spawnPoint;
    }

    private IEnumerator SpawnEnemies(int archers, int melees)
    {
        for (int i = 0; i < archers; i++)
        {
            SpawArcher(GetSpawnPoint());
            yield return new WaitForSeconds(1);
        }

        for (int i = 0; i < melees; i++)
        {
            SpawMelee(GetSpawnPoint());
            yield return new WaitForSeconds(1);
        }
    }




}
