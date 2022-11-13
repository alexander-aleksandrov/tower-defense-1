using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _boardSize;
    [SerializeField]
    private GameBoard _board;

    private void Start()
    {
        _board.Initialyze(_boardSize);
    }
}
