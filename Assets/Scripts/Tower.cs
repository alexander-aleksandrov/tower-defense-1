using System;
using UnityEngine;

public class Tower : TileContent
{
    [SerializeField, Range(1.5f, 10.5f)]
    private float _targetingRange = 1.5f;

    private TargetPoint _target;
    private const int ENEMY_LAYER_MASK = 1 << 6;

    public override void GameUpdate()
    {
        if (TrackTarget() || IsAcquireTarget())
        {
            Debug.Log("Lock on target");
        }
    }

    public bool TrackTarget()
    {
        if (_target == null)
        {
            return false;
        }
        Vector3 a = transform.localPosition;
        Vector3 b = _target.Position;
        if (Vector3.Distance(a, b) > _targetingRange + 0.125f * _target.Enemy.Scale)
        {
            _target = null;
            return false;
        }
        return true;
    }

    private bool IsAcquireTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.localPosition, _targetingRange, ENEMY_LAYER_MASK);
        if (targets.Length > 0)
        {
            _target = targets[0].GetComponent<TargetPoint>();
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
        if (_target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, _target.Position);
        }
    }
}
