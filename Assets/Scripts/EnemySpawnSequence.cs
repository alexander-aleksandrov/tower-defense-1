using UnityEngine;

[System.Serializable]
public class EnemySpawnSequence
{
    [SerializeField]
    private EnemyFactory _factory;
    [SerializeField]
    private EnemyType _type = EnemyType.Medium;
    [SerializeField, Range(0, 100)]
    private int _amount = 1;
    [SerializeField, Range(0.1f, 10f)]
    private float _coolDown = 1f;

    public State Begin() => new State(this);

    [System.Serializable]
    public struct State
    {
        private int _count;
        private float _cooldown;

        private EnemySpawnSequence _sequence;
        public State(EnemySpawnSequence sequence)
        {
            _sequence = sequence;
            _count = 0;
            _cooldown = sequence._coolDown;
        }
        public float Progress(float deltatTime)
        {
            _cooldown += deltatTime;
            while (_cooldown >= _sequence._coolDown)
            {
                _cooldown -= _sequence._coolDown;
                if (_count >= _sequence._amount)
                {
                    return _cooldown;
                }
                _count++;
                Game.SpawnEnemy(_sequence._factory, _sequence._type);
            }
            return -1f;
        }
    }
}
