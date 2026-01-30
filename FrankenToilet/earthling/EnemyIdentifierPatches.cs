using System;
using UnityEngine;
using HarmonyLib;
using FrankenToilet.Core;

namespace FrankenToilet.earthling;

[PatchOnEntry]
[HarmonyPatch(typeof(EnemyIdentifier))]
public class EnemyIdentifierPatches
{
    [HarmonyPrefix]
    [HarmonyPatch("Death", new Type[]{typeof(bool)})]
    public static void SpawnFishOnDeath(EnemyIdentifier __instance) 
    {
        if (__instance.dead) return;

        FishObject fish = FishProvider.GetRandomFish();
        ItemIdentifier fishPickup = FishProvider.CreateFishPickup(fish);
        GameObject fishInstance = GameObject.Instantiate(fishPickup.gameObject, __instance.transform.position, __instance.transform.rotation, __instance.transform.parent);
        fishInstance.GetComponent<Rigidbody>().velocity += Vector3.up * 50;

        FishingHUD.Instance.ShowHUD();
        FishingHUD.Instance.ShowFishCaught(true, fish);
        FishManager.Instance.UnlockFish(fish);
    }
}