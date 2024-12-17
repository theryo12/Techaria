
using Terraria.ModLoader.IO;

namespace Techaria.Systems;

public interface IContain<T> where T : Resources<T>, new()
{

	public T[] GetInputSlotsForConnector(object connector)
	{
		return InputSlots;
	}

	public T[] InputSlots => Slots;

	public T[] Slots { get; }

	public T[] OutputSlots => Slots;

	public T[] GetOutputSlotsForConnector(object _)
	{
		return InputSlots;
	}

	public T InsertToSlot(T to, T from)
	{
		return to.Insert(from);
	}

	public T Insert(T res, object connector)
	{
		foreach (var slot in GetInputSlotsForConnector(connector))
		{
			InsertToSlot(slot, res);
			if (res.IsEmpty()) break;
		}
		return res;
	}

	public T Extract(float amount, object connector)
	{
		var slots = GetOutputSlotsForConnector(connector);
		foreach (var slot in slots)
		{
			if (!slot.IsEmpty())
			{
				return slot.Remove(amount);
			}
		}

		return new();
	}

	public void Save(TagCompound nbt)
	{
		Console.WriteLine(typeof(T).Name);
		nbt.Add(typeof(T).Name, (from slot in Slots select slot.Save()).ToArray());
	}

	public void Load(TagCompound nbt)
	{
		var slots = Slots;
		var data = nbt.Get<TagCompound[]>(typeof(T).Name);
		for (int i = 0; i < Math.Min(slots.Length, data.Length); i++)
		{
			slots[i].Load(data[i]);
		}
	}
}
