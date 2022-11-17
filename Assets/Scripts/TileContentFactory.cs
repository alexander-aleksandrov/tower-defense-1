
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
    private TileContent _towerPrefab;


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
            case TileContentType.Tower:
                return Get(_towerPrefab);
        }
        return null;
    }

    private TileContent Get(TileContent prefab)
    {
        TileContent instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }
}
