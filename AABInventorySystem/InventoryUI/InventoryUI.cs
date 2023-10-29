using Sandbox.InventoryUI.BaseInventoryUI;
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;
using System.Text.Json;
using Sandbox.AABInventorySystem;
using System.Linq;
using Sandbox.AABInventorySystem.AABBoxInventorySystem;

namespace Sandbox.InventoryUI
{
	public partial class InventoryUI : Panel
	{
		public PlayerInventory PlayerInventory { get; protected set; }

		public BoxInventoryPanel BoxInventory { get; protected set; }

		public CountSliderPanel CountSlider { get; protected set; }
		
		public InventoryUI()
		{
			if ( !Game.IsClient )
				return;

			Game.RootPanel.StyleSheet.Load( "AABInventorySystem/InventoryUI/Styles/inventoryUI.scss" );;
			Game.RootPanel.AddChild( this );
			AddClass( "inventory" );
			AddClass( "invUI_hidden" );
			PlayerInventory = AddChild<PlayerInventory>();
			BoxInventory = AddChild<BoxInventoryPanel>();
			CountSlider = AddChild<CountSliderPanel>();
			
		}


		public void OnOpened() 
		{

			UpdateOpenBoxState();
			CountSlider.Close();


		}

		public void OnClosed()
		{
			InventoryUi.BoxInventory.IsBoxOpened = false;

		}


		[GameEvent.Tick.Client]
		public void OnTick()
		{
			if ( BoxInventory.IsBoxExist )
			{

				UpdateOpenBoxState();
				
			}
			if ( !BoxInventory.IsBoxOpened )
			{
				CountSlider.Close();
			}
		}

		public void UpdateOpenBoxState()
		{
			if ( InventoryUi == null )
			{
				InitInventoryHud();
			}

			var trr = Trace.Sphere( AABBaseBox.MAX_DISTANCE_BETWEEN_BOX_AND_PLAYER, Game.LocalClient.Pawn.Position, Game.LocalClient.Pawn.Position ).DynamicOnly().RunAll();

			bool isBoxExist = false;
			foreach ( var tr in trr )
			{
				if ( tr.Entity is AABBaseBox )
				{
					isBoxExist = true;
					break;
				}

			}
			InventoryUi.BoxInventory.IsBoxExist = isBoxExist;


			if ( !InventoryUi.BoxInventory.IsBoxExist )
			{
				InventoryUi.BoxInventory.IsBoxOpened = false;
			}

			if ( InventoryUi.BoxInventory.IsBoxOpened && InventoryUi.BoxInventory.IsBoxExist )
			{
				BoxInventory.RemoveClass( "invUI_hidden" );
			}
			else
			{
				
				BoxInventory.AddClass( "invUI_hidden" );
			}

		}


		public static InventoryUI InventoryUi;

		[GameEvent.Tick.Client]
		private void TickPlayerInventory()
		{

			if ( Input.Pressed( "AABInventory" ) )
			{
				Sandbox.InventoryUI.InventoryUI.ChangeOpenStateInventoryUI();
			}
		}


		[ConCmd.Client("init_inventory")]
		public static void InitInventoryHud()
		{
			
			if ( InventoryUi != null ) return;


			InventoryUi = new InventoryUI();
			
			Log.Info( "Inited inventory UI" );

			
		}


		[ConCmd.Client( "rebuild_inventory" )]
		public static void RebuildInventoryHud()
		{
			if(InventoryUi == null ) 
			{
				InitInventoryHud();
				return;
			}
			InventoryUi.Delete();
			InventoryUi = new InventoryUI();
			Log.Info( "Rebuilded inventory UI" );


		}


		[ConCmd.Client("inventoryui_open")]
		public static void OpenInventoryUI() 
		{
			if(InventoryUi == null )
			{
				InitInventoryHud();
			}
			
			InventoryUi.OnOpened();
			InventoryUi.RemoveClass( "invUI_hidden" );


		}


		[ConCmd.Client( "inventoryui_close" )]
		public static void CloseInventoryUI()
		{
			if ( InventoryUi == null )
			{
				InitInventoryHud();
			}
			InventoryUi.OnClosed(); 
			InventoryUi.AddClass( "invUI_hidden" );
			

		}

		[ConCmd.Client( "inventoryui_changeopenstate" )]
		public static void ChangeOpenStateInventoryUI()
		{
			if ( InventoryUi == null )
			{
				InitInventoryHud();
			}

			if ( InventoryUi.HasClass( "invUI_hidden" ) )
			{
				OpenInventoryUI();
			}
			else
			{
				
				CloseInventoryUI();
			}
		}

		public static bool ShowDebug { get; protected set; }

		[ConCmd.Client( "inventoryui_changeshowdebugstatus" )]
		public static void ChangeShowDebugStatus()
		{
			if ( InventoryUi == null )
			{
				InitInventoryHud();
			}
			ShowDebug = !ShowDebug;
			
			InventoryUi.StateHasChanged();
		}

		public static string ConvertItemToUIJson(IEnumerable<AABBaseItem> items) 
		{
			var slots = new List<InventorySlot>();
			int id = 0;
			foreach ( var item in items )
			{
				if ( slots.Any( x => x.ItemInfoId == item.Info.Id ) )
				{
					
					var slot = slots.First( x => x.ItemInfoId == item.Info.Id );
					slot.ItemIds = slot.ItemIds.Append( item.Id );
					slot.Count++;
					slot.TotalWeight += (slot.TotalWeight / (slot.Count - 1));
					slot.TotalSize += (slot.TotalSize / (slot.Count - 1));

					continue;
				}
				var newItem = new InventorySlot()
				{
					Count = 1,
					SlotId = id,
					ItemIds= new List<int>() { item.Id },
					Description = item.Info.Description,
					ImgSrc = item.Info.ImgSrc,
					ItemInfoId = item.Info.Id,
					Name = item.Info.Name,
				};
				newItem.TotalSize = item.Info.Size;
				newItem.TotalWeight = item.Info.Weight;

				slots.Add( newItem );
				id++;
			}
			var slotsJson = JsonSerializer.Serialize( slots );


			return slotsJson;

		}


	}
}
