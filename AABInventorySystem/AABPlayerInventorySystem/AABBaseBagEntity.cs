using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.AABInventorySystem.AABPlayerInventorySystem
{

	public abstract class AABBaseBagEntity : ModelEntity, IUse
	{
		public abstract void OnApperance(Vector3 spawnpos);

		protected AABBagInventory Bag { get; set; }

		public bool IsUsable( Entity user )
		{
			return true;
		}

		public bool OnUse( Entity user )
		{

			if ( Bag == null )
			{
				Log.Error( "Somtimes went wrong, BagEntity havent PlayerBag info" );
				Delete();
			}
			if ( user is not IEntity pl ) return false;
			
			
			PlayerInvetoriesManager.ClientPickBag(pl.Client,Bag);
			Delete();
			return false;
		}


		public AABBaseBagEntity( AABBagInventory bag)
		{
			Bag = bag;
		}

		public AABBaseBagEntity()
		{

		}


		public static void CreateBagEntity(Vector3 spawnPos, AABBagInventory bag )
		{
			var ent = TypeLibrary.Create( bag.Bag.BagEntityType.Name, bag.Bag.BagEntityType) as AABBaseBagEntity;
			if ( ent == null ) return;
			ent.Bag= bag;
			ent.OnApperance(spawnPos);
		}
	}
}
