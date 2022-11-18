
using UnityEngine;

[CreateAssetMenu]
public class TileContentFactory : GameObjectFactory
{
    [SerializeField]
    private TileContent _destinationPrefab;
    [SerializeField]
    private TileContent _emptyPrefab;
    [SerializeField]
    private TileContent _wallPrefab;
    [SerializeField]
    private TileContent _spawnPointPrefab;
    [SerializeField]
    private Tower[] _towerPrefabs;


    public void Reclaim(TileContent content)
    {
        Destroy(content.gameObject);
    }

    public TileContent Get(TileContentType type)
    {
        switch (type)
        {
            case TileContentType.Empty:
                return Get(_emptyPrefab);
            case TileContentType.Destination:
                return Get(_destinationPrefab);
            case TileContentType.Wall:
                return Get(_wallPrefab);
            case TileContentType.SpawnPoint:
                return Get(_spawnPointPrefab);

        }
        return null;
    }

    public Tower Get(TowerType towerType)
    {
        Tower prefab = _towerPrefabs[(int)towerType];
        return Get(prefab);
    }

    private T Get<T>(T prefab) where T : TileContent
    {
        T instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }
}
