
using Terraria.ModLoader.IO;

namespace Techaria.Systems;

public abstract class Resources<T> where T : Resources<T> {
    public abstract T Insert(T resource);

    public abstract T Remove(float amount);
    
    public abstract bool IsEmpty();
    
    public abstract TagCompound Save();
    public abstract void Load(TagCompound tag);
    
}
