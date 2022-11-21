using System;
using UnityEngine;

public abstract class Tower : TileContent
{
    [SerializeField, Range(1.5f, 10.5f)]
    protected float _targetingRange = 1.5f;

    public abstract TowerType TowerType { get; }
    protected bool IsTrackingTarget(ref TargetPoint _target)
    {
        if (_target == null)
        {
            return false;
        }
        Vector3 a = transform.localPosition;
        Vector3 b = _target.Position;
        float x = a.x - b.x;
        float z = a.z - b.z;
        float r = _targetingRange + _target.ColliderSize * _target.Enemy.Scale;

        if (x * x + z * z > r * r)
        {
            _target = null;
            return false;
        }
        return true;
    }

    protected bool IsAcquireTarget(out TargetPoint _target)
    {
        if (TargetPoint.FillBuffer(transform.localPosition, _targetingRange))
        {
            _target = TargetPoint.GetRandomTarget;
            return true;
        }
        _target = null;
        return false;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, _targetingRange);
    }
}

public enum TowerType
{
    Laser, Mortar
}