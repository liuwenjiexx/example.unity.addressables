using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AddressablesUtility
{


    public static async Task InitializeAddressables()
    {
        var op = Addressables.InitializeAsync(false);
        try
        {

            await op.Task;
            if (op.Status != AsyncOperationStatus.Succeeded)
            {
                throw new Exception("Addressables failed to initialize");
            }
        }
        finally
        {
            Addressables.Release(op);
        }
        Debug.Log("Addressables initialized");
    }

    public static async Task CheckForUpdates(string remoteCatalogUrl)
    {

        var op = Addressables.LoadContentCatalogAsync(remoteCatalogUrl, false);
        try
        {
            await op.Task;
            if (op.Status != AsyncOperationStatus.Succeeded)
            {
                if (op.OperationException != null)
                {
                    Debug.LogException(op.OperationException);
                }
                throw new Exception("Failed to load remote catalog");
            }

        }
        finally
        {
            Addressables.Release(op);
        }
        Debug.Log("Remote catalog loaded successfully");
        var catalogHandle = Addressables.CheckForCatalogUpdates(false);
        List<string> catalogs;
        try
        {
            await catalogHandle.Task;
            if (catalogHandle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new Exception("Failed to CheckForCatalogUpdates");
            }
            catalogs = catalogHandle.Result;
        }
        finally
        {
            Addressables.Release(catalogHandle);
        }

        Debug.Log($"Update catalogs {catalogs.Count}, [{string.Join(", ", catalogs)}]");
        if (catalogs.Count == 0)
            return;

        var locatorsHandle = Addressables.UpdateCatalogs(catalogs, false);
        List<IResourceLocator> locators;
        List<object> downloadKeys = new List<object>();
        try
        {
            await locatorsHandle.Task;
            if (locatorsHandle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new Exception("Failed to UpdateCatalogs");
            }
            locators = locatorsHandle.Result;

            foreach (var locator in locators)
            {
                Debug.Log($"locatorId: {locator.LocatorId}, {locator.Keys.Count()}");
                downloadKeys.AddRange(locator.Keys);
            }
        }
        finally
        {
            Addressables.Release(locatorsHandle);
        }


        var downloadSizeHandle = Addressables.GetDownloadSizeAsync(downloadKeys);
        await downloadSizeHandle.Task;
        if (downloadSizeHandle.Status != AsyncOperationStatus.Succeeded)
        {
            throw new Exception("Failed to GetDownloadSizeAsync");
        }
        Debug.Log($"GetDownloadSizeAsync successfully, download size: {downloadSizeHandle.Result}");
        long size = downloadSizeHandle.Result;
        if (size == 0)
            return;
        var downloadHandle = Addressables.DownloadDependenciesAsync(downloadKeys, Addressables.MergeMode.Union, false);
        try
        {
            await downloadHandle.Task;
            if (downloadHandle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new Exception("Failed to DownloadDependenciesAsync");
            }
        }
        finally
        {
            Addressables.Release(downloadHandle);
        }

    }


}
