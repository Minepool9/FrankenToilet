using FrankenToilet.Core;
using FrankenToilet.triggeredidiot;
using HarmonyLib;
using System;
using UnityEngine;

namespace FrankenToilet.duviz;

public class HealthRemover : MonoBehaviour
{
    public static float percentage;

    public void Update()
    {
        if (NewMovement.instance == null) { percentage = 0; return; }
        if (!NewMovement.instance.activated) { percentage = 0; return; }
    }
}

[PatchOnEntry]
[HarmonyPatch(typeof(HealthBar), "Update")]
public class HealthPatch
{
    [HarmonyPostfix]
    public static void Postfix(HealthBar __instance)
    {
        if (__instance.hpText != null)
        {
            __instance.hpText.text = $"{__instance.hp.ToString("F0")} // {HealthRemover.percentage.ToString("F2")}%";
        }
    }
}

[PatchOnEntry]
[HarmonyPatch(typeof(NewMovement), "GetHurt")]
public class NewMovementHurtPatch
{
    [HarmonyPrefix]
    public static void Prefix(HealthBar __instance, ref int damage)
    {
        HealthRemover.percentage = Mathf.Min(damage + UnityEngine.Random.Range(0, 1f) + HealthRemover.percentage, 5000);
        damage *= (int)MathF.Min(HealthRemover.percentage / 100 + 1, 100000);
    }
    [HarmonyPostfix]
    public static void Post(HealthBar __instance)
    {
        NewMovement.instance.rb.velocity += new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * HealthRemover.percentage; ;

        if (HealthRemover.percentage > 5000)
            DeltaruneExplosion.ExplodePlayer();
    }
}