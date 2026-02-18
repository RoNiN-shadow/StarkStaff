using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using MagicStaff.Content.Projectiles;
using System;

namespace MagicStaff.Content.Items.Weapons
{
  public class MagicStaff : ModItem
  {

    public override void SetDefaults()
    {
      Item.width = 40;
      Item.height = 40;
      Item.useTime = 20;
      Item.useAnimation = 20;
      Item.useStyle = ItemUseStyleID.Shoot;
      Item.knockBack = 6;
      Item.value = Item.buyPrice(silver: 10);
      Item.rare = ItemRarityID.Blue;
      Item.UseSound =SoundID.Item1;
      Item.autoReuse = true;
      Item.DamageType = DamageClass.Ranged;
      Item.noMelee = true;

      Item.damage = 15;
      Item.shoot = ModContent.ProjectileType<MagicBullet>();
      Item.shootSpeed = 10f;
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
        Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {


      int projectilesAmmount = 6;
      for(int i =0; i<projectilesAmmount; i++)
      {
        float radius = 15f;
        float angle = MathHelper.TwoPi / projectilesAmmount * i;

        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 offset = new Vector2(x, y) * radius;

        Vector2 spawnPos = player.Center + offset;

        float spreadAngle = MathHelper.ToRadians(Main.rand.NextFloat(-45f, 45f));
        float spreadVel = Main.rand.NextFloat(0.8f, 1.1f);
        Vector2 pertuted = velocity.RotatedBy(spreadAngle) * spreadVel;

        Projectile.NewProjectile(source, spawnPos, pertuted, type, damage, knockback, player.whoAmI);
      }
      return false;
    }
    public override void AddRecipes()
    {
      CreateRecipe().AddIngredient(ItemID.Wood)
          .AddTile(TileID.WorkBenches)
          .Register();
    }
  }
      
}

