
using Sandbox.InventoryUI;

namespace Sandbox.AABInventorySystem.AABPlayerInventorySystem
{
	public class AABPlayerInventory : AABBaseInventory
	{

		protected AABPlayerBag _bag;

		protected IClient _client;

		public bool HaveBag => _bag != null;

		public AABPlayerInventory(IClient client) : base()
		{
			_client = client;
			_bag = null;
		}
		
		public void SendInventoryInfoToClient()
		{
			OnBagSetted();
			OnInventoryChanged();
			
		}


		public void OnInventoryChanged()
		{
			PlayerInventory.InventoryUpdated(To.Single(_client),InventoryUI.InventoryUI.ConvertItemToUIJson( Items ) );
		}


		public override bool TryAddItem( AABBaseItem item )
		{
			var result = base.TryAddItem( item );
			if ( result )
			{
				OnInventoryChanged( );
			}
			return result;
		}

		public override bool TryRemoveItem( int itemId, out AABBaseItem item )
		{
			var result = base.TryRemoveItem( itemId, out item );

			if( result )
			{
				OnInventoryChanged( );
			}
			return result;
		}

		protected void OnBagSetted()
		{
			if(_bag == null)
			{
				MaxWeight= 0;
				MaxSize= 0;
				PlayerInventory.SetBagInfoInUI( To.Single( _client ),0,0 );
				return;
			}
			MaxWeight = _bag.MaxWeight;
			MaxSize= _bag.MaxSize;
			PlayerInventory.SetBagInfoInUI( To.Single( _client ), _bag.MaxSize, _bag.MaxWeight );
		}

		public void SetBag(AABBagInventory bag )
		{
			_bag = bag.Bag;
			OnBagSetted();

			foreach ( var item in bag.ItemList )
			{
				bag.TryRemoveItem( item.Id, out var ritem );
				TryAddItem( ritem );
			}
		}

		//Moves to bag inventory and remove the bag
		public AABBagInventory RemoveBag()
		{

			var result = new AABBagInventory( _bag );
			result.MoveItemsFromInventorytoBag( this );
			_bag = null;
			OnBagSetted();
			SendInventoryInfoToClient();
			return result;
		}


		[ConCmd.Server("aabinventory_sendinfotoclient")]
		public static void command_SendInventoryInfoToClient()
		{
			var caller = ConsoleSystem.Caller;
			var pinv = PlayerInvetoriesManager.GetInventory( caller );
			pinv.SendInventoryInfoToClient();

		}



	}
}
