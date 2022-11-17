using System;
using UnityEngine;

public class Tower : TileContent
{
    [SerializeField, Range(1.5f, 10.5f)]
    private float _targetingRange = 1.5f;

    [SerializeField]
    private Transform _turret;
    [SerializeField]
    private Transform _laserBeam;
    [SerializeField, Range(1f, 100f)]
    private float _dmgPerSecond = 10f;

    private Vector3 _laserBeamScale;

    private TargetPoint _target;
    private const int ENEMY_LAYER_MASK = 1 << 6;
    static Collider[] _targetsBuffer = new Collider[1];

    private void Awake()
    {
        _laserBeamScale = _laserBeam.localScale;
    }

    public override void GameUpdate()
    {
        if (IsTrackingTarget() || IsAcquireTarget())
        {
            Shoot();
        }
        else
        {
            _laserBeam.localScale = Vector3.zero;
        }
    }

    private void Shoot()
    {
        Vector3 point = _target.Position;
        _turret.LookAt(point);
        _laserBeam.localRotation = _turret.localRotation;
        float d = Vector3.Distance(_turret.position, point);
        _laserBeamScale.z = d;
        _laserBeam.localScale = _laserBeamScale;
        _laserBeam.localPosition = _turret.localPosition + 0.5f * d * _laserBeam.forward;
        _target.Enemy.TakeDamage(_dmgPerSecond * Time.deltaTime);
    }

    public bool IsTrackingTarget()
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

    private bool IsAcquireTarget()
    {
        Vector3 a = transform.localPosition;
        Vector3 b = a;
        b.y += 2f;
        int hits = Physics.OverlapCapsuleNonAlloc(a, b, _targetingRange, _targetsBuffer, ENEMY_LAYER_MASK);

        if (hits > 0)
        {
            _target = _targetsBuffer[0].GetComponent<TargetPoint>();
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
