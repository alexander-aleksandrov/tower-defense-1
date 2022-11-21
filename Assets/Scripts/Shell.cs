using UnityEngine;
public class Shell : WarEntity
{
    private Vector3 _launchPoint, _targetPoint, _launchVelocity;
    private float _age;
    public void Initialize(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity)
    {
        _launchPoint = launchPoint;
        _targetPoint = targetPoint;
        _launchVelocity = launchVelocity;
    }

    public override bool GameUpdate()
    {
        _age += Time.deltaTime;
        Vector3 p = _launchPoint + _launchVelocity * _age;
        p.y -= 0.5f * 9.81f * _age * _age;
        if (p.y < 0)
        {
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
