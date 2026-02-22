using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MagicStaff.Content.NPCS
{
  public class Wizard : GlobalNPC
  {
    public override void ModifyShop(NPCShop shop)
    {
      if(!(shop.NpcType == NPCID.Wizard)) return;

      shop.Add(new Item(ModContent.ItemType<Content.Items.Weapons.MagicStaff>()){
          shopCustomPrice = Item.buyPrice(gold: 5)
          });
    }
  }
}
