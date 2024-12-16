
using Terraria.ModLoader.IO;

namespace Techaria.Systems;

public class Power : Resources<Power> {
    public float power;
    public float max;

    public Power() : this(0, 0) {}
    public Power(float power, float max)
    {
        this.power = power;
        this.max = max;
    }

    public override Power Insert(Power resource)
    {
        resource.power -= Insert(resource.power);
        return resource;
    }

    public float Insert(float amt)
    {
        float amount = Math.Min(max - power, amt);
        power += amount;
        return amt - amount;
    }

    public override Power Remove(float amount)
    {
        float amt = Math.Min(power, amount);
        power -= amt;
        return new Power(amt, 0);
    }
    
    public override bool IsEmpty() => power <= 0;
    public override TagCompound Save()
    {
        var ret = new TagCompound();
        ret.Add("power", power);
        return ret;
    }

    public override void Load(TagCompound tag)
    {
        power = tag.Get<float>("power");
    }
}
