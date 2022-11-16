using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Transform _arrow;
    private Tile _east, _west, _north, _south, _nextOnPath;
    private int _distance;

    private Quaternion _northRotation = Quaternion.Euler(90f, 0f, 0f);
    private Quaternion _eastRotation = Quaternion.Euler(90f, 90f, 0f);
    private Quaternion _southRotation = Quaternion.Euler(90f, 180f, 0f);
    private Quaternion _westRotation = Quaternion.Euler(90f, 270f, 0f);

    private TileContent _content;

    public Tile NextTileOnPath => _nextOnPath;
    public Vector3 ExitPoint { get; private set; }

    public Direction PathDirection { get; private set; }

    public TileContent Content
    {
        get => _content;
        set
        {
            if (_content != null)
            {
                _content.Recycle();
            }
            _content = value;
            _content.transform.localPosition = transform.localPosition;
        }
    }
    public bool HasPath => _distance != int.MaxValue;
    public bool IsAlternative { get; set; }

    public static void MakeEastWestNeighbors(Tile east, Tile west)
    {
        east._west = west;
        west._east = east;
    }
    public static void MakeNorthSouthNeighbors(Tile north, Tile south)
    {
        north._south = south;
        south._north = north;
    }
    public void ClearPath()
    {
        _distance = int.MaxValue;
        _nextOnPath = null;
    }
    public void BecomeDestination()
    {
        _distance = 0;
        _nextOnPath = null;
        ExitPoint = transform.localPosition;
    }
    private Tile GrowPathTo(Tile neighbor, Direction direction)
    {
        if (!HasPath || neighbor == null || neighbor.HasPath)
        {
            return null;
        }
        neighbor._distance = _distance + 1;
        neighbor._nextOnPath = this;
        neighbor.ExitPoint = neighbor.transform.localPosition + direction.GetHalfVector();
        neighbor.PathDirection = direction;
        return neighbor.Content.Type != TileContentType.Wall ? neighbor : null;
    }
    public Tile GrowPathEast() => GrowPathTo(_east, Direction.West);
    public Tile GrowPathWest() => GrowPathTo(_west, Direction.East);
    public Tile GrowPathNorth() => GrowPathTo(_north, Direction.South);
    public Tile GrowPathSouth() => GrowPathTo(_south, Direction.North);

    public void ShowPath()
    {
        if (_distance == 0)
        {
            _arrow.gameObject.SetActive(false);
            return;
        }
        _arrow.gameObject.SetActive(true);
        _arrow.localRotation =
            _nextOnPath == _north ? _northRotation :
            _nextOnPath == _east ? _eastRotation :
            _nextOnPath == _south ? _southRotation :
            _westRotation;
    }
}

