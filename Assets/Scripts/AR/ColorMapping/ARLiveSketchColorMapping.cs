using UnityEngine;
using System.Collections.Generic;
using Luck9kr.Uisystem;
using UnityEngine.AddressableAssets;
using System.Linq;


public class ARLiveSketchColorMapping : ColorMappingBase
{
    public Texture2D SetColorMapping()
    {
        My_UIManager.Instance.CurrentView.Loading = true;

        Texture2D tex = GetProcessColoredMapTexture();
        if (tex != null)
        {
            List<GameObject> coloringObjs = DefaultUtils.GetChildWithTag(transform, "coloring");
            foreach (GameObject obj in coloringObjs)
            {
                obj.GetComponent<Renderer>().material.SetTexture("_Texture", tex);
            }
        }

        My_UIManager.Instance.CurrentView.Loading = false;
        return tex;
    }

    public T LoadAddressableData<T>(string key)
    {
        if (string.IsNullOrEmpty(key) || key == "texture")
            return default;

        if (!Addressables.ResourceLocators.Any(locator => locator.Locate(key, typeof(T), out _)))
        {
            Debug.LogWarning($"Key not found: {key}");
            return default;
        }

        var data = Addressables.LoadAssetAsync<T>(key);
        return data.WaitForCompletion();
    }
}
