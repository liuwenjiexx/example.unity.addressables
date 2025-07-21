using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TestStart : MonoBehaviour
{
    string remoteCatalogUrl = "http://192.168.32.77/AssetBundles/Android/catalog_0.1.json";

    // Start is called before the first frame update
    async void Start()
    {
        await AddressablesUtility.InitializeAssets();

        Debug.Log($"RuntimePath={Addressables.RuntimePath}\n" +
            $"LibraryPath={Addressables.LibraryPath}\n" +
            $"BuildPath={Addressables.BuildPath}\n" +
            $"PlayerBuildDataPath={Addressables.PlayerBuildDataPath}\n" +
            $"StreamingAssetsSubFolder={Addressables.StreamingAssetsSubFolder}\n" +
            $"Addressables Version={Addressables.Version}\n");
        Debug.Log($"persistentDataPath={Application.persistentDataPath}");

        await AddressablesUtility.CheckForUpdates(remoteCatalogUrl);

        StartCoroutine(LoadAsset());

    }



    IEnumerator LoadHotUpdateAssemblies()
    {
        string assemblyLabel = "Assembly";
        var textHandle = Addressables.LoadAssetsAsync<TextAsset>(assemblyLabel);
        yield return textHandle;
        foreach (var item in textHandle.Result)
        {
            Debug.Log(item.text);
        }
    }


    IEnumerator LoadAsset()
    {
        var op = Addressables.LoadAssetAsync<GameObject>("Assets/PublishAssets/Prefabs/TestReference.prefab");
        yield return op;
        var go = Instantiate(op.Result);

    }


    // Update is called once per frame
    void Update()
    {

    }
}
