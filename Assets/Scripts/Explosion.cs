using UnityEngine;

public class Explosion : WarEntity
{
    [SerializeField, Range(0f, 1f)]
    private float _duration = 0.5f;

    private float _age;

    public void Initialize(Vector3 position, float blastRadius, float damage)
    {
        TargetPoint.FillBuffer(position, blastRadius);
        for (int i = 0; i < TargetPoint.BufferedCount; i++)
        {
            TargetPoint.GetBuffered(i).Enemy.TakeDamage(damage);
        }
        transform.localPosition = position;
        transform.localScale = Vector3.one * (2f * blastRadius);
    }

    public override bool GameUpdate()
    {
        _age += Time.deltaTime;
        if (_age >= _duration)
        {
            OriginFactory.Reclaim(this);
            return false;
        }
        return true;
    }
}
