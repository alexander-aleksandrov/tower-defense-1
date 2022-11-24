using UnityEngine;
[CreateAssetMenu]
public class GameScenario : ScriptableObject
{
    [SerializeField]
    private EnemyWave[] waves = { };

    [SerializeField, Range(1, 10)]
    private int _cycles = 1;

    public State Begin() => new State(this);

    [System.Serializable]
    public struct State
    {
        private GameScenario _scenario;
        private int _index, _cycle;
        EnemyWave.State _wave;

        public State(GameScenario scenario)
        {
            _scenario = scenario;
            _cycle = 0;
            _index = 0;
            _wave = _scenario.waves[0].Begin();
        }

        public bool Progress()
        {
            float deltaTime = _wave.Progress(Time.deltaTime);

            while (deltaTime >= 0f)
            {
                if (++_index >= _scenario.waves.Length)
                {
                    if (++_cycle >= _scenario._cycles && _scenario._cycles > 0)
                    {

                        return false;
                    }
                    _index = 0;
                }
                _wave = _scenario.waves[_index].Begin();
                deltaTime = _wave.Progress(deltaTime);
            }
            return true;
        }
    }
}
