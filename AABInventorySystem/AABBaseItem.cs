using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.AABInventorySystem
{
	public class AABBaseItem
	{
		public int Id { get; protected set; }


		public AABBaseItemInfo Info { get; protected set; }


		public AABBaseItem(AABBaseItemInfo info)
		{	
			Game.AssertServer();
			Id = AllocateNextAABBaseItem();
			Info = info;
		}


		protected static int LastItemId = -1;
		public static int AllocateNextAABBaseItem()
		{
			LastItemId++;
			return LastItemId;
		}
	}


	public class AABBaseItemDemonstation
	{
		public int Id { get; set; }

		public AABBaseItemInfo Info { get; set; }
	}
}
