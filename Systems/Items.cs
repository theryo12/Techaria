

using Terraria.ModLoader.IO;

namespace Techaria.Systems;

public class Items(Item item, float maxStackMultiplier) : Resources<Items>
{
    public Item item = item;
    public float maxStackMultiplier = maxStackMultiplier;

    public Items() : this(new Item(), 0) { }

    public override Items Insert(Items resource)
    {
        if (item == null || item.IsAir)
        {
            item = resource.item.Clone();
            resource.item.TurnToAir();
        }
        else if (item.type == resource.item.type)
        {
            int count = Math.Min((int)(maxStackMultiplier * item.maxStack) - item.stack, resource.item.stack);
            item.stack += count;
            resource.item.stack -= count;
            if (resource.item.stack <= 0)
            {
                resource.item.TurnToAir();
            }
        }
        return resource;
    }

    public override Items Remove(float amount)
    {
        int amt = Math.Min((int)amount, item.stack);
        Items ret = new(item.Clone(), 0);
        ret.item.stack = amt;
        item.stack -= amt;
        if (item.stack <= 0)
        {
            item.TurnToAir();
        }
        return ret;
    }

    public override bool IsEmpty() => item.stack <= 0;
    public override TagCompound Save()
    {
        var ret = new TagCompound
        {
            { "item", item }
        };
        return ret;
    }

    public override void Load(TagCompound tag)
    {
        item = tag.Get<Item>("item");
    }
}
