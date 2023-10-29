using Sandbox.InventoryUI.BaseInventoryUI;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Sandbox.InventoryUI
{
	public partial class PlayerInventory : BaseInventoryPanel
	{
		public PlayerInventory()
		{
			var arr = new List<InventorySlot>();
			Slots = arr;
			HeadTitle = "BACKPACK CONTENT";
			ConsoleSystem.Run( "aabinventory_sendinfotoclient" );
		}

		protected override void OnItemSlotClicked(int slotId, MouseButtons mouseButton ) 
		{
			MoveBetweenInventories( slotId, mouseButton, "aabinventory_movetobox" );

		
		}

		public void SetSlots(IEnumerable<InventorySlot> slots )
		{
			Slots = slots;
			TotalWeight = 0;
			TotalSize = 0;
			foreach(var item in Slots )
			{

				TotalWeight +=item.TotalWeight;
				TotalSize += item.TotalSize;
			}
			StateHasChanged();
		}

		public void SetBagInfo(int maxSize, int maxWeight )
		{
			MaxWeight = maxWeight;
			MaxSize = maxSize;
			StateHasChanged();
		}

		[ClientRpc]
		public static void InventoryUpdated(string slotsJson)
		{

			if ( InventoryUI.InventoryUi == null )
			{
				InventoryUI.InitInventoryHud();
			}
			IEnumerable<InventorySlot> slots = JsonSerializer.Deserialize<IEnumerable<InventorySlot>>(slotsJson);
			InventoryUI.InventoryUi.PlayerInventory.SetSlots( slots );
		}

		[ClientRpc]
		public static void SetBagInfoInUI( int maxSize, int maxWeight)
		{
			if ( InventoryUI.InventoryUi == null )
			{
				InventoryUI.InitInventoryHud();
			}
			InventoryUI.InventoryUi.PlayerInventory.SetBagInfo( maxSize, maxWeight );
		}

	}
}
