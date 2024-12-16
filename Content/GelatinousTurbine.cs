using Terraria.ModLoader.IO;

namespace Techaria.Content;

using static GelatinousTurbine;
public class GelatinousTurbine : Machine<Tile, Item, Entity> {

	public class Tile : BaseTile {
		public override int width => 3;
		public override int height => 2;

		public override bool RightClick(int i, int j)
		{
			var playerItem = Main.mouseItem.IsAir ? Main.player[Main.myPlayer].HeldItem : Main.mouseItem;
			var startCount = playerItem.stack;
			(GetTileEntity(i, j) as IContain<Items>).Insert(new Items(playerItem, 0), null);
			if (startCount > playerItem.stack) return true;

			Terraria.Item.NewItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), 
				new Rectangle(i * 16, j * 16, 16, 16),
				(GetTileEntity(i, j) as IContain<Items>).Extract(1, null).item);

			return true;
		}

		public override void MouseOver(int i, int j)
		{
			var item = (GetTileEntity(i, j) as IContain<Items>).GetOutputSlotsForConnector(null)[0].item;
			var player = Main.LocalPlayer;
			
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = item.type;
			player.cursorItemIconText = item.stack.ToString() + " | " + 
					(GetTileEntity(i, j) as IContain<Power>).GetOutputSlotsForConnector(null)[0].power;
			player.noThrow = 2;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			foreach (var slot in (GetTileEntity(i, j) as IContain<Items>).Slots)
			{
				Terraria.Item.NewItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), 
					new Rectangle(i * 16, j * 16, 16, 16),
					slot.item);
			}
			base.KillMultiTile(i, j, frameX, frameY);
		}
	}

	public class Item : BaseItem {
		
	}

	public class Entity : BaseEntity, IContain<Items>, IContain<Power>
	{
		private Power power = new Power(0, 1000);
		private Items items = new Items(new (), 1);
		private int frame = 0;
		private int burnTime = 0;
		
		public static Rectangle particleRect = new(4, 22, 16, 6);	
		public static Dictionary<int, int> fuelItems = new Dictionary<int, int>()
		{
			{ItemID.Gel, 600}, //10 seconds
			{ItemID.PinkGel, 3600}, //1 minute
			{ItemID.RoyalGel, 216000}, // 1 hour
			{ItemID.VolatileGelatin, 438000} // 2 hours
		};		
		
		Power[] IContain<Power>.Slots => [power];
		Items[] IContain<Items>.Slots => [items];

		public override void Update()
		{
			if (burnTime > 0 && power.power < power.max)
			{
				burnTime -= 1;
				power.Insert(10);
				frame = burnTime / 15 % 4;
				
				for (int i = 0; i < 3; i++) 
				{
					for (int j = 0; j < 2; j++)
					{
						var tile = Main.tile[Position.X + i, Position.Y + j];
						tile.TileFrameX = (short)(54 + 18 * i);
						tile.TileFrameY = (short)(frame * 36 + j * 18);
					}
				}	

				if (new Random().Next(10) == 0) {
					var dust = Dust.NewDustDirect(new Vector2(Position.X, Position.Y) * 16 + particleRect.TopLeft(), particleRect.Width, particleRect.Height, DustID.Flare);
					//dust.velocity = new Vector2(0, -1);
					//dust.alpha = 127;
				}
				if (new Random().Next(3) == 0) {
					var dust = Dust.NewDustDirect(new Vector2(Position.X, Position.Y) * 16 + particleRect.TopLeft(), particleRect.Width, particleRect.Height, DustID.Smoke);
					dust.velocity = new Vector2(0, -1);
					dust.alpha = 127;
				}
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 2; j++)
					{
						var tile = Main.tile[Position.X + i, Position.Y + j];
						tile.TileFrameX = (short)(18 * i);
						tile.TileFrameY = (short)(j * 18);
					}
				}
			}

			if (burnTime <= 0)
			{
				burnTime = fuelItems[items.item.type];
				items.Remove(1);
			}
		}

		public Items Insert(Items res, object connector)
		{
			if (fuelItems.ContainsKey(res.item.type))
			{
				return items.Insert(res);
			}
			
			return res;
		}

		public override void SaveData(TagCompound tag)
		{
			(this as IContain<Items>).Save(tag);
			(this as IContain<Power>).Save(tag);
			tag.Add("burnTime", burnTime);
		}
		public override void LoadData(TagCompound tag)
		{
			(this as IContain<Items>).Load(tag);
			(this as IContain<Power>).Load(tag);
			burnTime = tag.Get<int>("burnTime");
		}
	}
}

