using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
    [SerializeField]
    private Enemy _prefab;

    [SerializeField, FloatRangeSlider(0.5f, 2f)]
    private FloatRange _scale = new FloatRange(1f);

    [SerializeField, FloatRangeSlider(-0.4f, 0.4f)]
    private FloatRange _pathOffset = new FloatRange(1f);

    [SerializeField, FloatRangeSlider(0.2f, 5f)]
    private FloatRange _speed = new FloatRange(1f);

    public Enemy Get()
    {
        Enemy instance = CreateGameObjectInstance(_prefab);
        instance.OrigignFactory = this;
        instance.Initialize(
            _scale.RandomValueInRange,
            _speed.RandomValueInRange,
            _pathOffset.RandomValueInRange);
        return instance;
    }

    public void Reclaim(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
