﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;
using UnityEngine;


namespace TorannMagic
{
    public class Verb_CureDisease : Verb_UseAbility
    {
        private int verVal;
        private int pwrVal;
            
        protected override bool TryCastShot()
        {
            Pawn caster = base.CasterPawn;
            Pawn pawn = this.currentTarget.Thing as Pawn;

            MagicPowerSkill pwr = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_CureDisease.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_CureDisease_pwr");
            MagicPowerSkill ver = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_CureDisease.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_CureDisease_ver");
            verVal = ver.level;
            pwrVal = pwr.level;
            if (caster.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            {
                MightPowerSkill mpwr = caster.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_pwr");
                MightPowerSkill mver = caster.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_ver");
                pwrVal = mpwr.level;
                verVal = mver.level;
            }
            bool flag = pawn != null;
            if (flag)
            {
                int num = 1;
                int sevAdjustment = 0;
                if (pwrVal == 3)
                {
                    sevAdjustment = Mathf.RoundToInt(Rand.Range(1.5f, 3f));
                }
                else if (pwrVal == 2)
                {
                    sevAdjustment = Mathf.RoundToInt(Rand.Range(1.2f, 2.3f));
                }
                else if (pwrVal == 1)
                {
                    sevAdjustment = Mathf.RoundToInt(Rand.Range(1f, 1.6f));
                }
                else
                {
                    sevAdjustment = Mathf.RoundToInt(Rand.Range(0f, 1f));
                }
                if(sevAdjustment !=0 ) 
                {
                    using (IEnumerator<Hediff> enumerator = pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Hediff rec = enumerator.Current;
                            bool flag2 = num > 0;
                            if (rec.def.makesSickThought)
                            {
                                bool success = false;
                                if ( rec.def.defName == "WoundInfection" || rec.def.defName == "Flu")
                                {
                                    rec.Severity -= sevAdjustment;
                                    success = true;
                                }
                                if (verVal >= 1 && (rec.def.defName == "GutWorms" || rec.def.defName == "Malaria"))
                                {
                                    rec.Severity -= sevAdjustment;
                                    success = true;
                                }
                                if (verVal >= 2 && (rec.def.defName == "SleepingSickness" || rec.def.defName == "MuscleParasites"))
                                {
                                    rec.Severity -= sevAdjustment;
                                    success = true;
                                }
                                if (verVal == 3)
                                {
                                    rec.Severity -= sevAdjustment;
                                    success = true;
                                }
                                if(success == true)
                                {
                                    TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3(), pawn.Map, 1.5f);
                                    MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "Cure Disease" + ": " + StringsToTranslate.AU_CastSuccess, -1f);
                                }
                                else
                                {
                                    Messages.Message("TM_CureDiseaseTypeFail".Translate(), MessageTypeDefOf.NegativeEvent);
                                    MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "Cure Disease" + ": " + StringsToTranslate.AU_CastFailure, -1f);
                                }                                
                            }
                        }
                    }
                }
                else
                {
                    MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "Cure Disease" + ": " + StringsToTranslate.AU_CastFailure, -1f);
                }
                
            }
            return false;
        }
    }
}
