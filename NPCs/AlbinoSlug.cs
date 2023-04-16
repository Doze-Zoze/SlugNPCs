using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.Utilities;

namespace SlugNPCs.NPCs
{
    [AutoloadHead]
    public class AlbinoSlug : ModNPC
	{
        public override string Texture => "SlugNPCs/NPCs/KeratostomSlug_Albino";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Albino Slug");
            Main.npcFrameCount[Type] = 4;
            NPCID.Sets.CannotSitOnFurniture[Type] = true;

            /*NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f,
                Direction = 1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);*/
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 34;
            NPC.height = 18;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0.5f;
            NPC.housingCategory = HousingCategoryID.PetNPCs;
        }


        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                {
                    continue;
                }

                // Player has to have either an ExampleItem or an ExampleBlock in order for the NPC to spawn
                if (NPC.downedBoss1)//(player.inventory.Any(item => item.type == ItemID.PearlwoodSword) && !Main.npc.Any(npc => npc.type == NPC.type))
                {
                    return true;
                }
            }
            return false;
        }

                public override string GetChat()
        {
            Main.player[Main.myPlayer].currentShoppingSettings.HappinessReport = "";
            if (NPC.DirectionTo(Main.player[Main.myPlayer].Center).X < 0) 
            {
                NPC.spriteDirection = 1;
                NPC.direction = 1;
            } else
            {
                NPC.spriteDirection = -1;
                NPC.direction = -1;
            }
            WeightedRandom<string> chat = new WeightedRandom<string>();

            chat.Add("Squish");


            return chat; // chat is implicitly cast to a string.
        }

        public override void SetChatButtons(ref string button, ref string button2)
        { // What the chat buttons are when you open up the chat UI
            button = "Pet";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                //Main.LocalPlayer.isPettingAnimal= true;
                var mplayer = Main.LocalPlayer.GetModPlayer<SlugPetting>();
                mplayer.PetAnimal(NPC.whoAmI);

                    return;
            }
        }
        public override void PostAI()
        {
            NPC.localAI[0]++;
            if (NPC.localAI[0] % 10 == 0)
            {
                NPC.frameCounter++;
            }
            if (NPC.frameCounter > 3)
            {
                NPC.frameCounter = 0;
            }
            NPC.frame = new Rectangle(0, (int)NPC.frameCounter*72/4, 34, 72 / 4);

            if (NPC.velocity.X > 0)
            {
                NPC.spriteDirection = -1;
            }
            if (NPC.velocity.X < 0)
            {
                NPC.spriteDirection = 1;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var opacity = ((NPC.localAI[0] % 120 < 60 ? (NPC.localAI[0] % 120) / 60 : (120-(NPC.localAI[0]) % 120) / 60));
            Color color = Color.White;
            color.A = 0;
            Texture2D texture = ModContent.Request<Texture2D>(Texture, (AssetRequestMode)2).Value;
            Texture2D glowTex = ModContent.Request<Texture2D>(Texture + "Glow", (AssetRequestMode)2).Value;
            var flip = SpriteEffects.None;
            if (NPC.spriteDirection == 1) {
                flip = SpriteEffects.FlipHorizontally;
            } 
            Main.EntitySpriteDraw(texture, NPC.Center - Main.screenPosition + new Vector2(0, 4), new Rectangle(0, texture.Height / 4 * (int)NPC.frameCounter, texture.Width, texture.Height / 4), drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 8), 1, flip, 0);
            Main.EntitySpriteDraw(glowTex, NPC.Center - Main.screenPosition + new Vector2(0, 2), new Rectangle(0, glowTex.Height / 4 * (int)NPC.frameCounter, glowTex.Width, glowTex.Height / 4), color * opacity, NPC.rotation, new Vector2(glowTex.Width / 2, glowTex.Height / 8), 1, flip, 0);
            return false;
        }
    }
}