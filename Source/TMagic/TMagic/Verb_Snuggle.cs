﻿using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using UnityEngine;


namespace TorannMagic
{

    public class Verb_Snuggle: Verb_MeleeAttack
    {

        protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
        {
            DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
            DamageInfo dinfo = new DamageInfo(DamageDefOf.Scratch, (int)(this.tool.power), 0, (float)-1, this.CasterPawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
            damageResult.hitThing = target.Thing;
            damageResult.totalDamageDealt = Mathf.Min((float)target.Thing.HitPoints, dinfo.Amount);
            for (int i = 0; i < 8; i++)
            {
                IntVec3 intVec = target.Cell + GenAdj.AdjacentCells[i];
                Pawn cleaveVictim = new Pawn();
                cleaveVictim = intVec.GetFirstPawn(target.Thing.Map);
                if (cleaveVictim != null && cleaveVictim.Faction != caster.Faction)
                {
                    DamageInfo dinfo2 = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_Cleave, (int)(this.tool.power * .6f), 0, (float)-1, this.CasterPawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
                    //damageResult.totalDamageDealt = Mathf.Min((float)cleaveVictim.HitPoints, dinfo2.Amount);
                    cleaveVictim.TakeDamage(dinfo2);
                    MoteMaker.ThrowMicroSparks(cleaveVictim.Position.ToVector3(), target.Thing.Map);
                    TM_MoteMaker.ThrowCrossStrike(cleaveVictim.Position.ToVector3Shifted(), cleaveVictim.Map, .4f);
                    TM_MoteMaker.ThrowBloodSquirt(cleaveVictim.Position.ToVector3Shifted(), cleaveVictim.Map, 1f);
                }
            }
            TM_MoteMaker.ThrowCrossStrike(target.Thing.Position.ToVector3Shifted(), target.Thing.Map, .5f);
            TM_MoteMaker.ThrowBloodSquirt(target.Thing.Position.ToVector3Shifted(), target.Thing.Map, 1.2f);
            target.Thing.TakeDamage(dinfo);
            return damageResult;  
        }
    }
}
