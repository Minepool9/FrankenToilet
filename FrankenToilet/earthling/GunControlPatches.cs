using UnityEngine;
using HarmonyLib;
using FrankenToilet.Core;

namespace FrankenToilet.earthling;

[PatchOnEntry]
[HarmonyPatch(typeof(GunControl))]
public class GunControlPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GunControl), "Start")]
    private static void EnableFishing()
    {
        if (GameObject.FindObjectOfType<FishingHUD>() != null) return;

        GameObject fishManagerObj = new GameObject("FishManager");
        fishManagerObj.SetActive(value: false);

        FishObject[] fishes = FishProvider.GetFishes();
        FishDescriptor[] fishDescriptors = new FishDescriptor[fishes.Length];

        for (int i = 0; i < fishes.Length; i++)
        {
            fishDescriptors[i] = new FishDescriptor();
            fishDescriptors[i].fish = fishes[i];
            fishDescriptors[i].chance = 1;
        }

        FishDB fishDB = ScriptableObject.CreateInstance<FishDB>();
        fishDB.foundFishes = fishDescriptors;

        fishManagerObj.AddComponent<FishManager>().fishDbs = new FishDB[] { fishDB };

        fishManagerObj.SetActive(value: true);

        foreach (FishObject fish in fishes)
        {
            if (!FishManager.Instance.recognizedFishes.ContainsKey(fish))
            {
                FishManager.Instance.recognizedFishes.Add(fish, value: false);
            }
        }

        GameObject fishingCanvas = AssetHelper.LoadPrefab("Assets/Prefabs/UI/FishingCanvas.prefab");
        GameObject fishingCanvasClone = GameObject.Instantiate(fishingCanvas);
    }
}
