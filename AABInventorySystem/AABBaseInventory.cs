using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.AABInventorySystem
{

	//Only for server-side
	public class AABBaseInventory
	{

		public string SerializedSlots => InventoryUI.InventoryUI.ConvertItemToUIJson( Items );
		public int MaxSize { get; protected set; }

		public int MaxWeight { get; protected set; }

		public int CurrentSize { get; protected set; }

		public int CurrentWeight { get; protected set; }

		public IEnumerable<AABBaseItemDemonstation> ItemList => GetItemList();

		protected List<AABBaseItem> Items { get; set; }


		private IEnumerable<AABBaseItemDemonstation> GetItemList()
		{
			var result = new List<AABBaseItemDemonstation>();
			foreach(var item in Items )
			{
				result.Add(new AABBaseItemDemonstation() { Id = item.Id, Info= item.Info });
			}

			return result;
		}

		public AABBaseInventory(int maxSize, int maxWeight)
		{
			Game.AssertServer();
			MaxSize = maxSize;
			MaxWeight = maxWeight;
			CurrentSize= 0;
			CurrentWeight= 0;
			Items = new List<AABBaseItem>();
		}

		public AABBaseInventory()
		{
			Game.AssertServer();
			MaxWeight= 0;
			MaxSize= 0;
			CurrentSize = 0;
			CurrentWeight = 0;
			Items = new List<AABBaseItem>();
		}

		public virtual bool TryAddItem( AABBaseItem item )
		{
			Game.AssertServer();

			if(!CanItemBeAdded(item)) return false;
			Items.Add( item );
			CurrentSize += item.Info.Size;
			return true;


		}

		public virtual bool TryRemoveItem( int itemId, out AABBaseItem item)
		{
			Game.AssertServer();

			bool isFinded = false;
			AABBaseItem fitem = null;
			foreach ( var pitem in Items )
			{
				if(pitem.Id == itemId )
				{
					isFinded= true;
					fitem = pitem;
					break;
				}
			}


			item = fitem;
			if ( isFinded )
			{
				Items.Remove( item );
				CurrentSize-= item.Info.Size;
			}
			return isFinded;


		}

		public virtual bool CanItemBeAdded(AABBaseItem item )
		{
			return !(item.Info.Size + CurrentSize > MaxSize || (item.Info.Weight + CurrentWeight) > MaxWeight);
		}

		public virtual bool CanItemBeAdded( AABBaseItemDemonstation item )
		{
			return !(item.Info.Size + CurrentSize > MaxSize || (item.Info.Weight + CurrentWeight) > MaxWeight);
		}




	}
}
