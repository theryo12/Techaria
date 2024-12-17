namespace Techaria.Systems;

public abstract class Machine<TTile, TItem, TEntity>
    where TTile : Machine<TTile, TItem, TEntity>.BaseTile
    where TItem : Machine<TTile, TItem, TEntity>.BaseItem
    where TEntity : Machine<TTile, TItem, TEntity>.BaseEntity
{
    public abstract class BaseTile : ModTile
    {

        public override string Name
        {
            get
            {
                var name = GetType().FullName.Replace('+', '_').Split('.');
                Console.WriteLine($"[36m{name[name.Length - 1]}[0m");
                return name[^1];
            }
        }

        public virtual int Width { get; }
        public virtual int Height { get; }
        public virtual int OriX { get => Width / 2; }
        public virtual int OriY { get => Height / 2; }

        public Point16 GetTopLeft(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            i -= tile.TileFrameX / 18 % Width;
            j -= tile.TileFrameY / 18 % Height;
            return new Point16(i, j);
        }

        public sealed override void SetStaticDefaults()
        {
            PreStaticDefaults();

            Main.tileLavaDeath[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Width = Width;
            TileObjectData.newTile.Height = Height;
            TileObjectData.newTile.CoordinateHeights = new int[Height];
            for (int i = 0; i < Height; i++)
            {
                TileObjectData.newTile.CoordinateHeights[i] = 16;
            }
            TileObjectData.newTile.Origin = new Point16(OriX, OriY);

            ModifyTileObjectData();
            TileObjectData.addTile(Type);
        }

        public virtual void PreStaticDefaults() { }

        public virtual void ModifyTileObjectData()
        {
            TileObjectData.newTile.LavaDeath = false;
        }

        public TEntity GetTileEntity(int i, int j)
        {
            TileEntity.ByPosition.TryGetValue(GetTopLeft(i, j), out TileEntity te);
            return te as TEntity;
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            Point16 p = GetTopLeft(i, j);
            ModContent.GetInstance<TEntity>().Place(p.X, p.Y);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<TEntity>().Kill(i, j);
        }

    }

    public abstract class BaseItem : ModItem
    {
        public override string Name
        {
            get
            {
                var name = GetType().FullName.Replace('+', '_').Split('.');
                Console.WriteLine($"[36m{name[^1]}[0m");
                return name[^1];
            }
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<TTile>());
        }
    }

    public abstract class BaseEntity : ModTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y)
        {
            return Main.tile[x, y].TileType == ModContent.TileType<TTile>();
        }
    }

}
