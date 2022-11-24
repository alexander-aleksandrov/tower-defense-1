using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _boardSize;
    [SerializeField]
    private GameBoard _board;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private TileContentFactory _contentFactory;
    [SerializeField]
    private WarFactory _warFactory;
    private TowerType _selectedTowerType;

    [SerializeField]
    private GameScenario _scenario;
    private GameScenario.State _activeScenario;

    [SerializeField, Range(0, 100)]
    private int _startingPlayerHealth = 10;
    private int _playerHealth;

    [SerializeField, Range(1f, 100f)]
    private float _playerSpeed = 1f;


    const float PAUSED_TIME_SCALE = 0f;

    GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();
    GameBehaviorCollection _enemies = new GameBehaviorCollection();
    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);


    public static Game instance;

    private void OnEnable()
    {
        instance = this;
    }

    private void Awake()
    {
        _playerHealth = _startingPlayerHealth;
        _board.Initialyze(_boardSize, _contentFactory);
        _activeScenario = _scenario.Begin();
    }

    private void Update()
    {
        //Game pause
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = Time.timeScale > PAUSED_TIME_SCALE ? PAUSED_TIME_SCALE : 1f;
        }
        else if (Time.timeScale > PAUSED_TIME_SCALE)
        {
            Time.timeScale = _playerSpeed;
        }

        //Place a wall/tower or spawn/destination
        if (Input.GetMouseButtonDown(0))
        {
            HandleAlternativeTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleTouch();
        }

        //Choose tower type 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selectedTowerType = TowerType.Laser;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _selectedTowerType = TowerType.Mortar;
        }

        //Begin a new game
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("New Game Started");
            BeginNewGame();
        }

        //Loosing the game 
        if (_playerHealth <= 0 && _startingPlayerHealth > 0)
        {
            Debug.Log("Defeated");
            BeginNewGame();
        }
        //Winning the game 
        if (!_activeScenario.Progress() && _enemies.IsEmpty)
        {
            Debug.Log("You win!");
            BeginNewGame();
            _activeScenario.Progress();
        }


        _activeScenario.Progress();

        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate();
        _nonEnemies.GameUpdate();
    }

    public static void EnemyReachedDestination()
    {
        instance._playerHealth--;
    }

    public void BeginNewGame()
    {
        _playerHealth = _startingPlayerHealth;
        _enemies.Clear();
        _nonEnemies.Clear();
        _board.Clear();
        _activeScenario = _scenario.Begin();
    }

    public static Shell SpawnShell()
    {
        Shell shell = instance._warFactory.Shell;
        instance._nonEnemies.Add(shell);
        return shell;
    }

    public static Explosion SpawnExplosion()
    {
        Explosion explosion = instance._warFactory.Explosion;
        instance._nonEnemies.Add(explosion);
        return explosion;
    }

    public static void SpawnEnemy(EnemyFactory factory, EnemyType type)
    {
        Tile spawnPoint = instance._board.GetSpawnPoint(Random.Range(0, instance._board.SpawnPointCount));
        Enemy enemy = factory.Get(type);
        enemy.SpawnOn(spawnPoint);
        instance._enemies.Add(enemy);
    }

    private void HandleTouch()
    {
        Tile tile = _board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleTower(tile, _selectedTowerType);
            }
            else
            {
                _board.ToggleWall(tile);
            }
        }
    }

    private void HandleAlternativeTouch()
    {
        Tile tile = _board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleDestination(tile);
            }
            else
            {
                _board.ToggleSpawnPoint(tile);
            }
        }
    }
}
