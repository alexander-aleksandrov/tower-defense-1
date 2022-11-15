using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Tile _tileFrom, _tileTo;
    private Vector3 _positionFrom, _positionTo;
    private float _progress;
    private Direction _direction;
    private DirectionChange _directionChange;
    private float _directionAngleFrom, _directionAngleTo;


    public EnemyFactory OrigignFactory { get; set; }

    public void SpawnOn(Tile tile)
    {
        _tileFrom = tile;
        _tileTo = tile.NextOnPath;
        _progress = 0f;
        PrepareIntro();
    }

    private void PrepareIntro()
    {
        _positionFrom = _tileFrom.transform.localPosition;
        _positionTo = _tileFrom.ExitPoint;
        _direction = _tileFrom.PathDirection;
        _directionChange = DirectionChange.None;
        _directionAngleFrom = _directionAngleTo = _direction.GetAngle();
        transform.localRotation = _direction.GetRotation();
    }

    public bool GameUpdate()
    {
        _progress += Time.deltaTime;
        while (_progress >= 1f)
        {
            _tileFrom = _tileTo;
            _tileTo = _tileTo.NextOnPath;
            if (_tileTo == null)
            {
                OrigignFactory.Reclaim(this);
                return false;
            }
            _progress -= 1f;
            PrepareNextState();

        }
        transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);
        if (_directionChange != DirectionChange.None)
        {
            float angle = Mathf.LerpUnclamped(_directionAngleFrom, _directionAngleTo, _progress);
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }
        return true;
    }

    private void PrepareNextState()
    {
        _positionFrom = _positionTo;
        _positionTo = _tileFrom.ExitPoint;
        _directionChange = _direction.GetDirectionChangeTo(_tileFrom.PathDirection);
        _direction = _tileFrom.PathDirection;
        _directionAngleFrom = _directionAngleTo;

        switch (_directionChange)
        {
            case DirectionChange.None:
                PrepareForward();
                break;
            case DirectionChange.TurnRight:
                PrepareTurnRight();
                break;
            case DirectionChange.TurnLeft:
                PrepareTurnLeft();
                break;
            default:
                PrepareTurnAround();
                break;
        }
    }
    private void PrepareForward()
    {
        transform.localRotation = _direction.GetRotation();
        _directionAngleTo = _direction.GetAngle();
    }
    private void PrepareTurnRight()
    {
        _directionAngleTo = _directionAngleFrom + 90f;
    }
    private void PrepareTurnLeft()
    {
        _directionAngleTo = _directionAngleFrom - 90f;
    }
    private void PrepareTurnAround()
    {
        _directionAngleTo = _directionAngleFrom + 180f;
    }

}
