using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.AABInventorySystem.AABPlayerInventorySystem
{
	public class AABPlayerBag
	{
		public int MaxWeight { get; protected set; }

		public int MaxSize { get; protected set; }

		public Type BagEntityType { get; protected set; }


		public AABPlayerBag(int maxWeight, int maxSize, Type bagEntityType)
		{
			Game.AssertServer();
			MaxWeight = maxWeight;
			MaxSize= maxSize;
			if(!bagEntityType.IsSubclassOf( typeof( AABBaseBagEntity ) ) )
			{
				throw new ArgumentException( "bagEntityType is not subclass of AABBaseBagEntity" );
			}
			BagEntityType= bagEntityType;
		}



	}
}
