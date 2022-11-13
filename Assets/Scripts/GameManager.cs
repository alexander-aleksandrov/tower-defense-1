using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _boardSize;
    [SerializeField]
    private GameBoard _board;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private TileContentFactory _contentFactory;

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

    private void Start()
    {
        _board.Initialyze(_boardSize);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
    }

    private void HandleTouch()
    {
        Tile tile = _board.GetTile(TouchRay);
        if (tile != null)
        {
            tile.Content = _contentFactory.Get(TileContentType.Destination);
        }
    }
}
