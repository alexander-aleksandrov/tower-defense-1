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
    [SerializeField]
    private GameScenario _scenario;
    private GameScenario.State _activeScenario;

    private TowerType _selectedTowerType;

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
        _board.Initialyze(_boardSize, _contentFactory);
        _activeScenario = _scenario.Begin();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleAlternativeTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleTouch();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selectedTowerType = TowerType.Laser;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _selectedTowerType = TowerType.Mortar;
        }

        _activeScenario.Progress();

        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate();
        _nonEnemies.GameUpdate();
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
