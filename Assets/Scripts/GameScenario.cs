using UnityEngine;
[CreateAssetMenu]
public class GameScenario : ScriptableObject
{
    [SerializeField]
    private EnemyWave[] waves = { };

    public State Begin() => new State(this);

    [System.Serializable]
    public struct State
    {
        private GameScenario _scenario;
        private int _index;
        EnemyWave.State _wave;

        public State(GameScenario scenario)
        {
            _scenario = scenario;
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
                    return false;
                }
                _wave = _scenario.waves[_index].Begin();
                deltaTime = _wave.Progress(deltaTime);
            }
            return true;
        }
    }
}
