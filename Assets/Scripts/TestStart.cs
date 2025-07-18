using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TestStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadAsset());
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
