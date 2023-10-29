using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.AABInventorySystem.AABPlayerInventorySystem
{
	public class AABBagInventory : AABBaseInventory
	{
		public AABPlayerBag Bag { get; protected set; }

		public AABBagInventory(AABPlayerBag bag) : base(bag.MaxSize,bag.MaxWeight)
		{
			Bag = bag;

		}

		public void MoveItemsFromInventorytoBag(AABPlayerInventory inventory )
		{
			var list = inventory.ItemList;
			foreach (var item in list) 
			{

				inventory.TryRemoveItem( item.Id, out var removedItem );
				TryAddItem( removedItem );


			}
		}
	}
}
