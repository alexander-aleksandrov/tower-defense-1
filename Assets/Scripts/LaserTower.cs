using UnityEngine;

public class LaserTower : Tower
{

    [SerializeField]
    private Transform _turret;

    [SerializeField]
    private Transform _laserBeam;
    private Vector3 _laserBeamScale;


    [SerializeField, Range(1f, 100f)]
    private float _dmgPerSecond = 10f;

    private TargetPoint _target;
    private const int ENEMY_LAYER_MASK = 1 << 6;
    static Collider[] _targetsBuffer = new Collider[100];

    public override TowerType TowerType => TowerType.Laser;

    private void Awake()
    {
        _laserBeamScale = _laserBeam.localScale;
    }

    public override void GameUpdate()
    {
        if (IsTrackingTarget(ref _target) || IsAcquireTarget(out _target))
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

}
