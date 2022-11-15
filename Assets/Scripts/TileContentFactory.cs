
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class TileContentFactory : ScriptableObject
{
    [SerializeField]
    private TileContent _destinationPrefab;
    [SerializeField]
    private TileContent _emptyPrefab;
    [SerializeField]
    private TileContent _wallPrefab;
    [SerializeField]
    private TileContent _spawnPointPrefab;


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

    private TileContent Get(TileContent prefab)
    {
        TileContent instance = Instantiate(prefab);
        instance.OriginFactory = this;
        MovetoFactoryScene(instance.gameObject);
        return instance;
    }

    private Scene _contentScene;

    private void MovetoFactoryScene(GameObject o)
    {
        if (!_contentScene.isLoaded)
        {
            if (Application.isEditor)
            {
                _contentScene = SceneManager.GetSceneByName(name);
                if (!_contentScene.isLoaded)
                {
                    _contentScene = SceneManager.CreateScene(name);
                }
            }
            else
            {
                _contentScene = SceneManager.CreateScene(name);
            }
        }
        SceneManager.MoveGameObjectToScene(o, _contentScene);
    }
}
