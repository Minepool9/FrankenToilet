using UnityEngine;
using System.Reflection;

using FrankenToilet.Core;


namespace FrankenToilet.greycsont;


[EntryPoint]
public static class AssetBundleHelper
{
    const string noteSkinPath = "FrankenToilet.greycsont.noteskinstealfrometterna.bundle";
    
    public static AssetBundle noteSkin;
    
    public static Sprite[] noteSprites;
    
    [EntryPoint]
    public static void Initialize()
    {
        if (noteSkin == null) noteSkin = LoadAssetBundle(noteSkinPath);
        noteSprites = noteSkin.LoadAssetWithSubAssets<Sprite>("arrow");
        
        LogHelper.LogDebug($"sprite length: {noteSprites.Length}");
    }

    public static AssetBundle LoadAssetBundle(string assetBundlePath)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(assetBundlePath);
        if (stream == null)
        {
            LogHelper.LogError($"FUCK YOU UNITY");
        }
        
        LogHelper.LogInfo($"Loaded AssetBundle: {assetBundlePath}");
        
        return AssetBundle.LoadFromStream(stream);;
    }

}