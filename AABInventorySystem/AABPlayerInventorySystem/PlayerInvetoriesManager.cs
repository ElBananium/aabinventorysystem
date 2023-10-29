using System.Collections.Generic;

namespace Sandbox.AABInventorySystem.AABPlayerInventorySystem
{
	public class PlayerInvetoriesManager
	{
		//key is client steamId
		protected static Dictionary<long, AABPlayerInventory> ClientsInventories = new();


		[GameEvent.Server.ClientJoined]
		public static void OnClientJoined( ClientJoinedEvent e )
		{
			ClientsInventories.Add( e.Client.SteamId, new(e.Client) );
		}
		[GameEvent.Server.ClientDisconnect]
		public static void OnClientDisconnected(ClientDisconnectEvent e )
		{
			ClientsInventories.Remove( e.Client.SteamId );
		}

		public static AABPlayerInventory GetInventory(long steamId )
		{
			return ClientsInventories[steamId];
		}

		public static AABPlayerInventory GetInventory( IClient cl )
		{
			return ClientsInventories[cl.SteamId];
		}


		public static void ClientPickBag( IClient cl, AABBagInventory bag )
		{
			var AABInventory = GetInventory( cl.SteamId );
			if ( AABInventory.HaveBag )
			{
				ClientDropBag(cl);
			}


			AABInventory.SetBag( bag );
		}

		public static void ClientDropBag(IClient cl)
		{
			var AABInventory = GetInventory( cl.SteamId );
			if ( !AABInventory.HaveBag ) return;

			var bag = AABInventory.RemoveBag();
			var tr = Trace.Ray( cl.Pawn.AimRay, 1000f ).StaticOnly().Run();
			AABBaseBagEntity.CreateBagEntity( tr.HitPosition, bag );

		}



		
	}
}
