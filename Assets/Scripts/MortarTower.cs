using System;
using UnityEngine;

public class MortarTower : Tower
{
    [SerializeField, Range(0.5f, 2f)]
    private float _shotsPerSecond = 1f;

    [SerializeField]
    private Transform _mortar;

    [SerializeField, Range(0.5f, 3f)]
    private float _shellBlastRadius = 1f;

    [SerializeField, Range(1f, 10f)]
    private float _shellDamage = 10f;

    private float _launchSpeed;
    private float _launchProgress;

    public override TowerType TowerType => TowerType.Mortar;
    private void Awake()
    {
        OnValidate();
    }
    private void OnValidate()
    {
        float x = _targetingRange + 0.25001f;
        float y = -_mortar.position.y;
        _launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
    }
    public override void GameUpdate()
    {
        _launchProgress += Time.deltaTime * _shotsPerSecond;
        while (_launchProgress >= 1)
        {
            if (IsAcquireTarget(out TargetPoint target))
            {

                Launch(target);
                _launchProgress -= 1;
            }
            else
            {
                _launchProgress = 0.999f;
            }
        }
    }

    private void Launch(TargetPoint target)
    {
        Vector3 launchPoint = _mortar.position;
        Vector3 targetPoint = target.Position;
        targetPoint.y = 0;
        Vector2 dir;
        dir.x = targetPoint.x - launchPoint.x;
        dir.y = targetPoint.y - launchPoint.y;

        float x = dir.magnitude;
        float y = -launchPoint.y;
        dir /= x;

        float g = 9.81f;
        float s = _launchSpeed;
        float s2 = s * s;

        float r = s2 * s2 - g * (g * x * x + 2f * y * s2);
        float tanTheta = (s2 + MathF.Sqrt(r)) / (g * x);
        float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        float sinTheta = cosTheta * tanTheta;

        _mortar.localRotation = Quaternion.LookRotation(new Vector3(dir.x, tanTheta, dir.y));
        Game.SpawnShell().Initialize(
            launchPoint,
            targetPoint,
            new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.y),
            _shellBlastRadius,
            _shellDamage);
    }
}
