using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TestReference : MonoBehaviour
{

    public AssetReference asset1;
    public AssetReference asset2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        if (GUILayout.Button("Asset 1"))
        {
            StartCoroutine(InstantiateAsync(asset1));
        }
        if (GUILayout.Button("Asset 2"))
        {
            StartCoroutine(InstantiateAsync(asset2));
        }
        if (GUILayout.Button("LoadUI"))
        {
            StartCoroutine(LoadUI());
        }
    }

    IEnumerator InstantiateAsync(AssetReference reference)
    {
        var op = reference.InstantiateAsync();
        yield return op;
        GameObject go = op.Result;
        

    }

    IEnumerator LoadUI()
    {
        var op = Addressables.LoadAssetAsync<GameObject>("Assets/PublishAssets/Prefabs/UI/Panel.prefab");
        yield return op;
        var canvas = GameObject.FindFirstObjectByType<Canvas>();
        var go = Instantiate(op.Result, canvas.transform);

    }
}
