namespace FrankenToilet.Bryan;

using FrankenToilet.Core;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary> Class for setting up my part of this mod (oh god) </summary>
[EntryPoint]
public class Femboy
{
    /// <summary> Whether to fuck tghe text (used in TextFucker) </summary>
    public static bool fuckText = false;

    /// <summary> why did i sign up for this </summary>
    [EntryPoint]
    public static void Awake()
    {
        BundleLoader.Load();

        SceneManager.sceneLoaded += (_, _) =>
        {
            fuckText = Random.Range(0, 4) == 0;
            if (SceneHelper.CurrentScene == "Main Menu")
                FindObject<Image>("Canvas/Main Menu (1)/LeftSide/Title").sprite = BundleLoader.ulakill;

            if (SceneHelper.CurrentScene == "Level 0-1")
                FindObject<Image>("Canvas/HurtScreen/Title Sound/Image").sprite = BundleLoader.UlraKil;

            if (SceneHelper.CurrentScene == "Level 7-1")
                FindObject<Image>("Canvas/HurtScreen/White").sprite = BundleLoader.Flash;

            FindObject<TextMeshProUGUI>("Canvas/Level Stats Controller/Level Stats (1)/Style Title")?.text = "AURA";
        };
    }

    /// <summary> Finds a GameObject based on name/path, doesnt matter if its enabled or not. </summary>
    public static T FindObject<T>(string Path, Scene? scene = null) where T : Component
    {
        scene ??= SceneManager.GetSceneAt(0);
        string rootSearchObj = Path;
        if (Path.IndexOf('/') != -1)
        {
            rootSearchObj = Path[..Path.IndexOf('/')];
            Path = Path[(Path.IndexOf('/') + 1)..];
        }

        var search = scene.Value.GetRootGameObjects().Where(g => g.name == rootSearchObj).FirstOrDefault();
        search = Path.IndexOf('/') == -1 ? search : search?.transform?.Find(Path)?.gameObject;
        return search?.GetComponent<T>();
    }
}
