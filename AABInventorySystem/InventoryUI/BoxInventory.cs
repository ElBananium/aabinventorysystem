using Sandbox.InventoryUI.BaseInventoryUI;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using static Sandbox.Clothing;

namespace Sandbox.InventoryUI
{
	public partial class BoxInventoryPanel : BaseInventoryPanel
	{

		public bool IsBoxExist { get; set; }

		public bool IsBoxOpened { get; set; }

		public int BoxNetworkIdent { get; set; }

		public BoxInventoryPanel()
		{
			var arr = new List<InventorySlot>();
			Slots = arr;
			
			HeadTitle = "BOX CONTENT";

			//AddClass( "invUI_hidden" );
		}


		protected override void OnItemSlotClicked( int slotId, MouseButtons mouseButton )
		{
			MoveBetweenInventories(slotId, mouseButton, "aabinventory_movefrombox" );

		} // 

		public void SetBoxInfo( IEnumerable<InventorySlot> slots, int maxWeight, int maxSize, int boxNetworkIdent)
		{
			Slots = slots;
			TotalWeight = 0;
			TotalSize = 0;
			foreach(var slot in slots)
			{
				TotalWeight += slot.TotalWeight;
				TotalSize += slot.TotalSize;
			}
			MaxWeight = maxWeight;
			MaxSize = maxSize;
			IsBoxOpened = true;
			BoxNetworkIdent = boxNetworkIdent;
			StateHasChanged();
		}


		[ClientRpc]
		public static void SendBoxInfoAndOpenInventory( string slotsJson, int MaxWeight, int MaxSize, int BoxNetworkIdent )
		{
			SendBoxInfo(slotsJson,MaxWeight,MaxSize,BoxNetworkIdent);
			InventoryUI.OpenInventoryUI();
		}
		[ClientRpc]
		public static void SendBoxInfo( string slotsJson, int MaxWeight, int MaxSize, int BoxNetworkIdent )
		{
			if ( InventoryUI.InventoryUi == null )
			{
				InventoryUI.InitInventoryHud();
			}
			IEnumerable<InventorySlot> slots = JsonSerializer.Deserialize<IEnumerable<InventorySlot>>( slotsJson );
			InventoryUI.InventoryUi.BoxInventory.SetBoxInfo( slots, MaxWeight, MaxSize, BoxNetworkIdent );
			
		}

	}
}
