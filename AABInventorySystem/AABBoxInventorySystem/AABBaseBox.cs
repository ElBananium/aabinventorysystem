using Sandbox.AABInventorySystem.AABPlayerInventorySystem;
using Sandbox.InventoryUI;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Sandbox.AABInventorySystem.AABBoxInventorySystem
{

	public abstract partial class AABBaseBox : ModelEntity, IUse
	{
		public const float MAX_DISTANCE_BETWEEN_BOX_AND_PLAYER = 100f;

		public AABBoxInventory BoxInventory { get; protected set; }


		public bool IsUsable( Entity user )
		{
			return true;
		}

		public bool OnUse( Entity user )
		{
			if ( user is not IEntity pl ) return false;
			if ( (pl.Position - Position).Length > MAX_DISTANCE_BETWEEN_BOX_AND_PLAYER ) return false;




			BoxInventoryPanel.SendBoxInfoAndOpenInventory( To.Single( pl.Client ), BoxInventory.SerializedSlots, BoxInventory.MaxWeight, BoxInventory.MaxSize, NetworkIdent );




			return false;
		}

		public override void Spawn()
		{
			base.Spawn();

			BoxInventory = BuildBoxInventory();


		}


		protected abstract AABBoxInventory BuildBoxInventory();


		internal static AABBaseBox FindBoxNearPlayer( IEntity p, int boxNetworkIdent )
		{
			var trr = Trace.Sphere( MAX_DISTANCE_BETWEEN_BOX_AND_PLAYER, p.Position, p.Position ).DynamicOnly().RunAll();



			foreach ( var tr in trr )
			{
				if ( tr.Entity is AABBaseBox && tr.Entity.NetworkIdent == boxNetworkIdent )
				{
					return tr.Entity as AABBaseBox;


				}
			}
			return null;
		}

		protected static void TryMoveBetweenInventories( AABBaseInventory from, AABBaseInventory to, int itemId )
		{
			var pItem = from.ItemList.FirstOrDefault( x => x.Id == itemId );
			if ( pItem == null ) return;
			if ( !to.CanItemBeAdded( pItem ) ) return;


			if ( !from.TryRemoveItem( itemId, out var item ) ) return;

			if ( !to.TryAddItem( item ) )
			{
				from.TryAddItem( item );
			}
		}


		[ConCmd.Server( "aabinventory_movetobox" )]
		public static void MoveToBox( int boxNetworkIdent, string itemIdsJson )
		{
			var p = ConsoleSystem.Caller.Pawn as IEntity;

			if ( p == null ) return;

			var bag = FindBoxNearPlayer( p, boxNetworkIdent );
			if ( bag == null ) return;
			var itemIds = JsonSerializer.Deserialize<List<int>>( itemIdsJson );
			var pinv = PlayerInvetoriesManager.GetInventory( p.Client );
			foreach(var itemId in itemIds )
			{
				TryMoveBetweenInventories( pinv, bag.BoxInventory, itemId );
			}
			
			BoxInventoryPanel.SendBoxInfo( To.Single( p.Client ), bag.BoxInventory.SerializedSlots, bag.BoxInventory.MaxWeight, bag.BoxInventory.MaxSize, bag.NetworkIdent );



		}
		[ConCmd.Server( "aabinventory_movefrombox" )]
		public static void MoveFromBox( int boxNetworkIdent, string itemIdsJson )
		{
			var p = ConsoleSystem.Caller.Pawn as IEntity;

			if ( p == null ) return;

			var bag = FindBoxNearPlayer( p, boxNetworkIdent );
			if ( bag == null ) return;

			var itemIds = JsonSerializer.Deserialize<List<int>>( itemIdsJson );
			var pinv = PlayerInvetoriesManager.GetInventory( p.Client );
			foreach ( var itemId in itemIds )
			{
				TryMoveBetweenInventories( bag.BoxInventory, pinv, itemId );
			}
			BoxInventoryPanel.SendBoxInfo( To.Single( p.Client ), bag.BoxInventory.SerializedSlots, bag.BoxInventory.MaxWeight, bag.BoxInventory.MaxSize, bag.NetworkIdent );
		}


	}
}
