using UnityEngine;
[CreateAssetMenu]
public class EnemyWave : ScriptableObject
{
    [SerializeField]
    public EnemySpawnSequence[] spawnSequences = { new EnemySpawnSequence() };

    public State Begin() => new State(this);

    [System.Serializable]
    public struct State
    {
        private EnemyWave _wave;
        private int _index;
        EnemySpawnSequence.State _sequence;
        public State(EnemyWave wave)
        {
            _wave = wave;
            _index = 0;
            _sequence = _wave.spawnSequences[0].Begin();
        }
        public float Progress(float deltaTime)
        {
            deltaTime = _sequence.Progress(deltaTime);
            while (deltaTime >= 0f)
            {
                if (++_index >= _wave.spawnSequences.Length)
                {
                    return deltaTime;
                }
                _sequence = _wave.spawnSequences[_index].Begin();
                deltaTime = _sequence.Progress(deltaTime);
            }
            return -1f;
        }
    }
}


