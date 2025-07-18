using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TestStart : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return InitializeAssets();
        StartCoroutine(LoadAsset());
    }

    IEnumerator InitializeAssets()
    {
        
        var op = Addressables.InitializeAsync();
        yield return op;
        if (op.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Addressables initialized");
        }
        else
        {
            Debug.Log("Addressables failed to initialize");
            yield break;
        }


        Debug.Log($"RuntimePath={Addressables.RuntimePath}\n" +
            $"LibraryPath={Addressables.LibraryPath}\n" +
            $"BuildPath={Addressables.BuildPath}\n" +
            $"PlayerBuildDataPath={Addressables.PlayerBuildDataPath}\n" +
            $"StreamingAssetsSubFolder={Addressables.StreamingAssetsSubFolder}\n" +
            $"Addressables Version={Addressables.Version}\n");

        string remoteCatalogUrl = "https://192.168.32.77/settings.json";
        op = Addressables.LoadContentCatalogAsync(remoteCatalogUrl);
        yield return op;
        if (op.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Remote catalog loaded successfully");
        }
        else
        {
            Debug.Log("Failed to load remote catalog");
            yield break;
        }
        var catalogHandle = Addressables.CheckForCatalogUpdates(false);
        yield return catalogHandle;
        if (catalogHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Failed to CheckForCatalogUpdates");
            yield break;
        }
        Addressables.Release(catalogHandle);
        var catalogs = catalogHandle.Result;
        var locatorsHandle = Addressables.UpdateCatalogs(catalogs, false);
        yield return locatorsHandle;
        if (locatorsHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Failed to UpdateCatalogs");
            yield break;
        }
        var locators = locatorsHandle.Result;
        yield return DownloadAssets(locators);

    }

    IEnumerator DownloadAssets(IEnumerable downloadKeys)
    {
        var downloadSizeHandle = Addressables.GetDownloadSizeAsync(downloadKeys);
        yield return downloadSizeHandle;
        if (downloadSizeHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Addressables.Release(downloadSizeHandle);
            Debug.Log("Failed to GetDownloadSizeAsync");
            yield break;
        }
        long size = downloadSizeHandle.Result;
        Addressables.Release(downloadSizeHandle);
        if (size == 0)
            yield break;
        var downloadHandle = Addressables.DownloadDependenciesAsync(downloadKeys, Addressables.MergeMode.Union, false);
        yield return downloadHandle;
        if (downloadHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Addressables.Release(downloadHandle);
            Debug.Log("Download failed");
        }
        else
        {
            Debug.Log("Download success");
        }
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
