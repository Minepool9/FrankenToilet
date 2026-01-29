using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using UnityEngine;

using FrankenToilet.Core;


namespace FrankenToilet.greycsont;

[PatchOnEntry]
[HarmonyPatch(typeof(ShotgunHammer))]
public static class ShotgunHammerPatch
{
    private static readonly MethodInfo negate = AccessTools.Method(typeof(Vector3), "op_UnaryNegation");
        
    private static readonly MethodInfo random4 = AccessTools.Method(typeof(DirectionRandomizer), nameof(DirectionRandomizer.Randomize4Dir));
    
    private static readonly MethodInfo DeliverDamage = AccessTools.Method(typeof(ShotgunHammer), nameof(ShotgunHammer.DeliverDamage));

    [HarmonyPrefix]
    [HarmonyPatch(nameof(ShotgunHammer.Impact))]
    public static void GenerateRandomDirection() => DirectionRandomizer.GenerateRandomDirection();
    
    
    [HarmonyTranspiler]
    [HarmonyPatch(nameof(ShotgunHammer.DeliverDamage))]
    public static IEnumerable<CodeInstruction> OnTriggerEnterTranspiler(
        IEnumerable<CodeInstruction> instructions)
    {
        var matcher = new CodeMatcher(instructions);
        
        matcher
            .MatchForward(false, new CodeMatch(OpCodes.Call, negate))
            .Set(OpCodes.Call, random4)
            .MatchForward(false, new CodeMatch(OpCodes.Call, negate))
            .Set(OpCodes.Call, random4);
        
        return matcher.InstructionEnumeration();
    }
}

