﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public static class TM_RenderQueue
    {
        //Magic
        public static readonly Material enchantMark = MaterialPool.MatFrom("Items/Gemstones/arcane_minor");

        public static readonly Color shieldColor = new Color(90f, 0f, 0f);
        public static readonly Material shieldMat = MaterialPool.MatFrom("Other/Shield", ShaderDatabase.Transparent, TM_RenderQueue.shieldColor);

        public static readonly Color manaShieldColor = new Color(127f, 0f, 255f);
        public static readonly Material manaShieldMat = MaterialPool.MatFrom("Other/Shield", ShaderDatabase.Transparent, TM_RenderQueue.manaShieldColor);

        public static readonly Material mageMarkMat = MaterialPool.MatFrom("Other/MageMark", ShaderDatabase.Transparent, Color.black);
        public static readonly Color necroMarkColor = new Color(.4f, .5f, .25f);
        public static readonly Material necroMarkMat = MaterialPool.MatFrom("Other/MageMark", ShaderDatabase.Transparent, TM_RenderQueue.necroMarkColor);
        public static readonly Color summonerMarkColor = new Color(.8f, .4f, .0f);
        public static readonly Material summonerMarkMat = MaterialPool.MatFrom("Other/MageMark", ShaderDatabase.Transparent, TM_RenderQueue.summonerMarkColor);
        public static readonly Material druidMarkMat = MaterialPool.MatFrom("Other/MageMark", ShaderDatabase.Transparent, Color.green);
        public static readonly Material paladinMarkMat = MaterialPool.MatFrom("Other/MageMark", ShaderDatabase.Transparent, Color.white);
        public static readonly Material warlockMarkMat = MaterialPool.MatFrom("Other/MageMark", ShaderDatabase.Transparent, Color.magenta);
        public static readonly Material lightningMarkMat = MaterialPool.MatFrom("Other/MageMark", ShaderDatabase.Transparent, Color.yellow);
        public static readonly Material iceMarkMat = MaterialPool.MatFrom("Other/MageMark", ShaderDatabase.Transparent, Color.blue);
        public static readonly Material fireMarkMat = MaterialPool.MatFrom("Other/MageMark", ShaderDatabase.Transparent, Color.red);
        public static readonly Color priestMarkColor = new Color(1f, 1f, .55f);
        public static readonly Material priestMarkMat = MaterialPool.MatFrom("Other/MageMark", ShaderDatabase.Transparent, TM_RenderQueue.priestMarkColor);
        public static readonly Color bardMarkColor = new Color(.8f, .8f, 0f);
        public static readonly Material bardMarkMat = MaterialPool.MatFrom("Other/MageMark", ShaderDatabase.Transparent, TM_RenderQueue.bardMarkColor);
        public static readonly Color demonkinMarkColor = new Color(.6f, 0, .25f);
        public static readonly Material demonkinMarkMat = MaterialPool.MatFrom("Other/MageMark", ShaderDatabase.Transparent, TM_RenderQueue.demonkinMarkColor);
        public static readonly Color earthMarkColor = new Color(.4f, .2f, 0f);
        public static readonly Material earthMarkMat = MaterialPool.MatFrom("Other/MageMark", ShaderDatabase.Transparent, TM_RenderQueue.earthMarkColor);

        //Might
        public static readonly Material deceptionEye = MaterialPool.MatFrom("Motes/DeceptionMote");
        public static readonly Material possessionMask = MaterialPool.MatFrom("Motes/PossessMote");

        public static readonly Material fighterMarkMat = MaterialPool.MatFrom("Other/FighterMark", ShaderDatabase.Transparent, Color.black);
        public static readonly Color facelessMarkColor = new Color(1f, .5f, .25f);
        public static readonly Material facelessMarkMat = MaterialPool.MatFrom("Other/FighterMark", ShaderDatabase.Transparent, TM_RenderQueue.facelessMarkColor);
        public static readonly Color rangerMarkColor = new Color(.3f, .6f, .0f);
        public static readonly Material rangerMarkMat = MaterialPool.MatFrom("Other/FighterMark", ShaderDatabase.Transparent, TM_RenderQueue.rangerMarkColor);
        public static readonly Color gladiatorMarkColor = new Color(0f, .35f, .75f);
        public static readonly Material gladiatorMarkMat = MaterialPool.MatFrom("Other/FighterMark", ShaderDatabase.Transparent, TM_RenderQueue.gladiatorMarkColor);
        public static readonly Material bladedancerMarkMat = MaterialPool.MatFrom("Other/FighterMark", ShaderDatabase.Transparent, Color.gray);
        public static readonly Color sniperMarkColor = new Color(.7f, 0f, 0f);
        public static readonly Material sniperMarkMat = MaterialPool.MatFrom("Other/FighterMark", ShaderDatabase.Transparent, TM_RenderQueue.sniperMarkColor);
        public static readonly Color psionicMarkColor = new Color(0f, .5f, 1f);
        public static readonly Material psionicMarkMat = MaterialPool.MatFrom("Other/PsiMark", ShaderDatabase.Transparent, TM_RenderQueue.psionicMarkColor);

    }
}
