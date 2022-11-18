using System;
using UnityEngine;

public abstract class Tower : TileContent
{
    [SerializeField, Range(1.5f, 10.5f)]
    protected float _targetingRange = 1.5f;

    private const int ENEMY_LAYER_MASK = 1 << 6;
    static Collider[] _targetsBuffer = new Collider[100];

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
        Vector3 a = transform.localPosition;
        Vector3 b = a;
        b.y += 2f;
        int hits = Physics.OverlapCapsuleNonAlloc(a, b, _targetingRange, _targetsBuffer, ENEMY_LAYER_MASK);

        if (hits > 0)
        {
            _target = _targetsBuffer[UnityEngine.Random.Range(0, hits)].GetComponent<TargetPoint>();
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