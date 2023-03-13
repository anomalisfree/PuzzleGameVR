using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Controllers
{
    public class BundleLoaderController : BaseController
    {
        private readonly List<string> _loadedBundles = new List<string>();

        public void LoadBundle(string bundleName, Action bundleLoaded)
        {
            StartCoroutine( LoadBundleAsync(bundleName, bundleLoaded));
        }

        private IEnumerator LoadBundleAsync(string bundleName, Action bundleLoaded)
        {
            if (_loadedBundles.Contains(bundleName))
            {
                bundleLoaded?.Invoke();
            }
            else
            {
                _loadedBundles.Add(bundleName);
                
                var assetBundle =
                    AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, bundleName));
                yield return assetBundle;

                var loadedAssetBundle = assetBundle.assetBundle;

                if (loadedAssetBundle == null)
                {
                    Debug.LogError($"Failed to load AssetBundle: {bundleName}");
                }
                else
                {
                    bundleLoaded?.Invoke();
                }
            }
        }
    }
}
