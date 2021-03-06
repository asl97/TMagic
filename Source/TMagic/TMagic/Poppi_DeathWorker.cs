﻿using RimWorld;
using System;
using Verse;

namespace TorannMagic
{
    public class Poppi_DeathWorker : DeathActionWorker
    {
        public override void PawnDied(Corpse corpse)
        {
            float radius = 2f;
            CompLeaper comp = corpse.InnerPawn.GetComp<CompLeaper>();
            if (comp != null)
            {
                radius = comp.explosionRadius;
            }
            bool flag = corpse.InnerPawn.ageTracker.CurLifeStageIndex == 0;
            if (flag)
            {
                radius = radius * Rand.Range(.6f, .8f);
            }
            else
            {
                bool flag2 = corpse.InnerPawn.ageTracker.CurLifeStageIndex == 1;
                if (flag2)
                {
                    radius = radius * Rand.Range(1f, 1.5f); 
                }
                else
                {
                    radius = radius * Rand.Range(1.3f, 1.7f);
                }
            }
            GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, DamageDefOf.Burn, corpse.InnerPawn, Rand.Range(6, 10), 0,  null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
        }
    }
}
