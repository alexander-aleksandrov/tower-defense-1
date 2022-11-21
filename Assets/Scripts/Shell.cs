using UnityEngine;
public class Shell : WarEntity
{
    private Vector3 _launchPoint, _targetPoint, _launchVelocity;
    private float _age, _blastRadius, _damage;
    public void Initialize(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity, float blastRadius, float damage)
    {
        _launchPoint = launchPoint;
        _targetPoint = targetPoint;
        _launchVelocity = launchVelocity;
        _blastRadius = blastRadius;
        _damage = damage;
    }

    public override bool GameUpdate()
    {
        _age += Time.deltaTime;
        Vector3 p = _launchPoint + _launchVelocity * _age;
        p.y -= 0.5f * 9.81f * _age * _age;
        if (p.y <= 0)
        {
            TargetPoint.FillBuffer(_targetPoint, _blastRadius);
            for (int i = 0; i < TargetPoint.BufferedCount; i++)
            {
                TargetPoint.GetBuffered(i).Enemy.TakeDamage(_damage);
            }
            OriginFactory.Reclaim(this);
            return false;
        }
        transform.localPosition = p;
        Vector3 d = _launchVelocity;
        d.y = -9.81f * _age;
        transform.localRotation = Quaternion.LookRotation(d);
        return true;
    }

}
