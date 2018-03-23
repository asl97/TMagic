﻿using RimWorld;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public class FlyingObject_DeathBolt : ThingWithComps
    {

        private static readonly Color lightningColor = new Color(160f, 160f, 160f);
        private static readonly Material OrbMat = MaterialPool.MatFrom("Spells/deathbolt", false);

        protected Vector3 origin;
        protected Vector3 destination;

        private int age = -1;
        private float arcaneDmg = 1;
        private bool powered = false;
        public Matrix4x4 drawingMatrix = default(Matrix4x4);
        public Vector3 drawingScale;
        public Vector3 drawingPosition;

        private int pwrVal = 0;
        private int verVal = 0;
        float radius = 1.4f;

        protected float speed = 30f;
        protected int ticksToImpact;
        private bool impacted = false;
        protected int ticksFollowingImpact =0;

        protected Thing launcher;
        protected Thing assignedTarget;
        protected Thing flyingThing;
        Pawn pawn;

        public DamageInfo? impactDamage;

        public bool damageLaunched = true;

        public bool explosion = false;

        private bool initialized = true;        

        protected int StartingTicksToImpact
        {
            get
            {
                int num = Mathf.RoundToInt((this.origin - this.destination).magnitude / (this.speed / 100f));
                bool flag = num < 1;
                if (flag)
                {
                    num = 1;
                }
                return num;
            }
        }

        protected IntVec3 DestinationCell
        {
            get
            {
                return new IntVec3(this.destination);
            }
        }

        public virtual Vector3 ExactPosition
        {
            get
            {
                Vector3 b = (this.destination - this.origin) * (1f - (float)this.ticksToImpact / (float)this.StartingTicksToImpact);
                return this.origin + b + Vector3.up * this.def.Altitude;
            }
        }

        public virtual Quaternion ExactRotation
        {
            get
            {
                return Quaternion.LookRotation(this.destination - this.origin);
            }
        }

        public override Vector3 DrawPos
        {
            get
            {
                return this.ExactPosition;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<Vector3>(ref this.origin, "origin", default(Vector3), false);
            Scribe_Values.Look<Vector3>(ref this.destination, "destination", default(Vector3), false);
            Scribe_Values.Look<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
            Scribe_Values.Look<int>(ref this.pwrVal, "pwrVal", 0, false);
            Scribe_Values.Look<int>(ref this.verVal, "verVal", 0, false);
            Scribe_Values.Look<float>(ref this.radius, "radius", 1.4f, false);
            Scribe_Values.Look<bool>(ref this.damageLaunched, "damageLaunched", true, false);
            Scribe_Values.Look<bool>(ref this.explosion, "explosion", false, false);
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
            Scribe_Values.Look<bool>(ref this.impacted, "impacted", false, false);
            Scribe_Values.Look<bool>(ref this.powered, "powered", false, false);
            Scribe_References.Look<Thing>(ref this.assignedTarget, "assignedTarget", false);
            Scribe_References.Look<Thing>(ref this.launcher, "launcher", false);
            Scribe_References.Look<Thing>(ref this.flyingThing, "flyingThing", false);
        }

        private void Initialize()
        {
            if (pawn != null)
            {
                MoteMaker.MakeStaticMote(pawn.TrueCenter(), pawn.Map, ThingDefOf.Mote_ExplosionFlash, 12f);
                SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);
                MoteMaker.ThrowDustPuff(pawn.Position, pawn.Map, Rand.Range(1.2f, 1.8f));
                CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
                if (comp.PowerModifier > 0)
                {
                    this.arcaneDmg += .2f;
                    comp.PowerModifier--;
                    this.powered = true;
                }
            }
            this.initialized = false;
        }

        public void Launch(Thing launcher, LocalTargetInfo targ, Thing flyingThing, DamageInfo? impactDamage)
        {
            this.Launch(launcher, base.Position.ToVector3Shifted(), targ, flyingThing, impactDamage);
        }

        public void Launch(Thing launcher, LocalTargetInfo targ, Thing flyingThing)
        {
            this.Launch(launcher, base.Position.ToVector3Shifted(), targ, flyingThing, null);
        }

        public void Launch(Thing launcher, Vector3 origin, LocalTargetInfo targ, Thing flyingThing, DamageInfo? newDamageInfo = null)
        {
            bool spawned = flyingThing.Spawned;
            pawn = launcher as Pawn;
            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            foreach (MagicPower current in comp.MagicData.MagicPowersN)
            {
                if ((current.abilityDef == TorannMagicDefOf.TM_DeathBolt || current.abilityDef == TorannMagicDefOf.TM_DeathBolt_I || current.abilityDef == TorannMagicDefOf.TM_DeathBolt_II || current.abilityDef == TorannMagicDefOf.TM_DeathBolt_III))
                {
                    if (current.level == 0)
                    {
                        this.radius = 1.4f;
                    }
                    else if (current.level == 1)
                    {
                        this.radius = 2f;
                    }
                    else if (current.level == 2)
                    {
                        this.radius = 2f;
                    }
                    else
                    {
                        this.radius = 2.4f;
                    }
                }
            }

            this.arcaneDmg = comp.arcaneDmg;
            MagicPowerSkill pwr = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_DeathBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_DeathBolt_pwr");
            MagicPowerSkill ver = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_DeathBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_DeathBolt_ver");
            verVal = ver.level;
            pwrVal = pwr.level;
            if (spawned)
            {
                flyingThing.DeSpawn();
            }
            this.launcher = launcher;
            this.origin = origin;
            this.impactDamage = newDamageInfo;
            this.speed = this.def.projectile.speed;
            this.flyingThing = flyingThing;
            bool flag = targ.Thing != null;
            if (flag)
            {
                this.assignedTarget = targ.Thing;
            }
            float distanceAccuracyModifier = (targ.Cell.ToVector3Shifted() - this.launcher.Position.ToVector3Shifted()).MagnitudeHorizontal() *.1f;
            this.destination = targ.Cell.ToVector3Shifted() + new Vector3(Rand.Range(-distanceAccuracyModifier, distanceAccuracyModifier), 0f, Rand.Range(-distanceAccuracyModifier, distanceAccuracyModifier));
            this.ticksToImpact = this.StartingTicksToImpact;
            this.Initialize();
        }

        public override void Tick()
        {
            base.Tick();
            age++;
            if (this.ticksToImpact >= 0)
            {
                DrawEffects(this.ExactPosition, base.Map);
            }
            this.ticksToImpact--;
            this.ticksFollowingImpact--;
            base.Position = this.ExactPosition.ToIntVec3();
            bool flag = !this.ExactPosition.InBounds(base.Map);
            if (flag)
            {
                this.ticksToImpact++;
                this.Destroy(DestroyMode.Vanish);
            }
            else
            {                                           
                bool flag2 = this.ticksToImpact <= 0 && !impacted;
                if (flag2)
                {
                    bool flag3 = this.DestinationCell.InBounds(base.Map);
                    if (flag3)
                    {
                        base.Position = this.DestinationCell;
                    }
                    this.ImpactSomething();
                }
            }

            if(this.impacted)
            {
                if (this.ticksFollowingImpact > 0 && Find.TickManager.TicksGame % 5 == 0)
                {
                    CellRect cellRect = CellRect.CenteredOn(base.Position, 2);
                    cellRect.ClipInsideMap(base.Map);
                    IntVec3 spreadingDarknessCell;
                    if(!(cellRect.CenterCell.GetTerrain(base.Map).passability == Traversability.Impassable) && !cellRect.CenterCell.IsValid || !cellRect.CenterCell.InBounds(base.Map))
                    {
                        this.ticksFollowingImpact = -1;
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        spreadingDarknessCell = cellRect.RandomCell;
                        if (spreadingDarknessCell.IsValid && spreadingDarknessCell.InBounds(base.Map))
                        {
                            GenExplosion.DoExplosion(spreadingDarknessCell, base.Map, .4f, TMDamageDefOf.DamageDefOf.TM_DeathBolt, this.launcher as Pawn, Mathf.RoundToInt((Rand.Range(.4f * this.def.projectile.damageAmountBase, .8f * this.def.projectile.damageAmountBase) + (3f * pwrVal)) * this.arcaneDmg), this.def.projectile.soundExplode, def, null, null, 0f, 1, false, null, 0f, 0, 0.0f, true);
                            TM_MoteMaker.ThrowDiseaseMote(base.Position.ToVector3Shifted(), base.Map, .6f);
                            if (powered)
                            {
                                TM_MoteMaker.ThrowBoltMote(base.Position.ToVector3Shifted(), base.Map, 0.3f);
                            }
                        }
                    }
                }
                
                if(this.ticksFollowingImpact < 0)
                {
                    this.Destroy(DestroyMode.Vanish);
                }
            }
        }

        public void DrawEffects(Vector3 effectVec, Map map)
        {
            effectVec.x += Rand.Range(-0.4f, 0.4f);
            effectVec.z += Rand.Range(-0.4f, 0.4f);
            TM_MoteMaker.ThrowDiseaseMote(effectVec, map, 0.4f, 0.1f, .01f, 0.35f);
        }

        public override void Draw()
        {
            bool flag = this.flyingThing != null && !this.impacted;
            if (flag)
            {
                Graphics.DrawMesh(MeshPool.plane10, this.DrawPos, this.ExactRotation, this.flyingThing.def.DrawMatSingle, 0);
                base.Comps_PostDraw();
            }
        }

        private void ImpactSomething()
        {
            bool flag = this.assignedTarget != null;
            if (flag)
            {
                Pawn pawn = this.assignedTarget as Pawn;
                bool flag2 = pawn != null && pawn.GetPosture() != PawnPosture.Standing && (this.origin - this.destination).MagnitudeHorizontalSquared() >= 20.25f && Rand.Value > 0.2f;
                if (flag2)
                {
                    this.Impact(null);
                }
                else
                {
                    this.Impact(this.assignedTarget);
                }
            }
            else
            {
                this.Impact(null);
            }
        }

        protected virtual void Impact(Thing hitThing)
        {
            bool flag = hitThing == null;
            if (flag)
            {
                Pawn pawn;
                bool flag2 = (pawn = (base.Position.GetThingList(base.Map).FirstOrDefault((Thing x) => x == this.assignedTarget) as Pawn)) != null;
                if (flag2)
                {
                    hitThing = pawn;
                }
            }        

            GenExplosion.DoExplosion(base.Position, base.Map, this.radius, TMDamageDefOf.DamageDefOf.TM_DeathBolt, this.launcher as Pawn, Mathf.RoundToInt((Rand.Range(.6f*this.def.projectile.damageAmountBase, 1.1f*this.def.projectile.damageAmountBase) + (5f * pwrVal)) * this.arcaneDmg), this.def.projectile.soundExplode, def, null, null, 0f, 1, false, null, 0f, 0, 0.0f, true);

            this.ticksFollowingImpact = this.verVal * 15;
            this.impacted = true;
        }        
    }
}
