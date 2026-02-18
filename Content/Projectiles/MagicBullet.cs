using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.DataStructures;

namespace MagicStaff.Content.Projectiles
{
  public class MagicBullet : ModProjectile
  {

    public override void SetDefaults()
    {
      Projectile.CloneDefaults(ProjectileID.RocketI);
      Projectile.width = 16;
      Projectile.height = 16;

      Projectile.aiStyle = -1;
      Projectile.DamageType = DamageClass.Ranged;
      Projectile.timeLeft = 300;
      Projectile.tileCollide = true;
      Projectile.friendly = true;

      Projectile.penetrate = 1;
    }
    public override void AI()
    {
      Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

      float maxSpeed = 40f;
      float acceleration = 0.7f;
      var (clsNpc, minDist) = FindEnemy();

      if(clsNpc != null){
        Vector2 dir = (clsNpc.Center - Projectile.Center);

        if(dir.LengthSquared() > 0.0001f)
          dir.Normalize();
        else
          dir = Vector2.Zero;


        float currentSpeed = Projectile.velocity.Length();

        if(currentSpeed < maxSpeed)
          currentSpeed += acceleration;

        dir *= currentSpeed;
        Projectile.velocity += (dir - Projectile.velocity) * 0.05f;

        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

        float exploadDistance = 1600f;
        if(minDist < exploadDistance )
          Projectile.Kill();
      }

      Lighting.AddLight(Projectile.Center, 0.1f, 0.6f, 0.9f);

      if(Main.rand.NextBool(2))
      {
        int dustBlue = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
        Dust blueDust = Main.dust[dustBlue];
        blueDust.noGravity = true;
        blueDust.velocity *= 0.1f;
        blueDust.scale = 1.2f;
      }
      if(Main.rand.NextBool(3))
      {
        int smokeDust = Dust.NewDust(
            Projectile.position, Projectile.width, Projectile.height, DustID.Smoke
            );
        Dust dustSmoke = Main.dust[smokeDust];
        dustSmoke.velocity *= 0.2f;
        dustSmoke.scale = 0.8f;
      }

    }
    public (NPC? clsNpc, float minDist) FindEnemy(float minDist=1000000f)
    {
      NPC clsNpc = null;
      int maxNpcs = Main.maxNPCs;

      for(int i =0; i < maxNpcs; i++)
      {
        NPC npc = Main.npc[i];

        if(npc.active && !npc.friendly)
        {

          float distEnemy = Vector2.DistanceSquared(npc.Center, Projectile.Center);
          if(distEnemy < minDist)
          {
            minDist = distEnemy;
            clsNpc = npc;
          }
        }
      }
      return (clsNpc, minDist);
    }
    public override void Kill(int timeLeft)
    {
      Projectile.Resize(64, 64);

      Projectile.penetrate = -1;
      Projectile.maxPenetrate = -1;
      // Projectile.usesLocalNPCImmunity = true;
      // Projectile.localNPCHitCooldown = 10;

      int maxNPCs = Main.maxNPCs;
      Player player = Main.player[Projectile.owner];
      for(int i =0; i< maxNPCs; i++)
      {
        NPC npc = Main.npc[i];

        if(npc.active && !npc.friendly && npc.Hitbox.Intersects(Projectile.Hitbox))
        {
          int finalDamage = (int)player.GetDamage(DamageClass.Ranged).ApplyTo(Projectile.damage);
          npc.StrikeNPC(new NPC.HitInfo()
          {
            Damage = finalDamage,
            SourceDamage = Projectile.damage,
            Knockback = Projectile.knockBack,
            HitDirection = Projectile.Center.X < npc.Center.X ? -1 : 1,
            Crit = false,
            DamageType = DamageClass.Ranged,
          });
        }
      }
      SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

      int dustTimes = 20;

      for(int i =0; i<dustTimes; i++)
      {
        int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
        Dust blueDust = Main.dust[dust];
        blueDust.velocity = Main.rand.NextVector2Circular(5f, 5f);
        blueDust.noGravity = true;
        blueDust.scale = 1.5f;
      }
      for(int i =0; i<10; i++)
      {
        int smoke = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
        Dust ssmoke = Main.dust[smoke];
        ssmoke.velocity = Main.rand.NextVector2Circular(2f, 2f);
        ssmoke.scale = 1.5f;
      }
    }

  }
}
