using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
    [System.Serializable]
    public class EnemyConfig
    {

        public Enemy _prefab;

        [FloatRangeSlider(0.5f, 2f)]
        public FloatRange _scale = new FloatRange(1f);

        [FloatRangeSlider(-0.4f, 0.4f)]
        public FloatRange _pathOffset = new FloatRange(0f);

        [FloatRangeSlider(0.2f, 5f)]
        public FloatRange _speed = new FloatRange(1f);

        [FloatRangeSlider(10f, 1000f)]
        public FloatRange _health = new FloatRange(100f);
    }
    [SerializeField]
    EnemyConfig _small, _medium, _large;


    private EnemyType _enemyType;

    EnemyConfig GetConfig(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Small:
                return _small;
            case EnemyType.Medium:
                return _medium;
            case EnemyType.Large:
                return _large;
        }
        Debug.Assert(false, "Unsupported enemy type.");
        return null;
    }

    public Enemy Get(EnemyType type = EnemyType.Medium)
    {
        EnemyConfig config = GetConfig(type);
        Enemy instance = CreateGameObjectInstance(config._prefab);
        instance.OrigignFactory = this;
        instance.Initialize(
            config._scale.RandomValueInRange,
            config._speed.RandomValueInRange,
            config._pathOffset.RandomValueInRange,
            config._health.RandomValueInRange);
        return instance;
    }

    public void Reclaim(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
