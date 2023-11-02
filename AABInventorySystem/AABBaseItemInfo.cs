using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.AABInventorySystem
{
	public class AABBaseItemInfo
	{
		public int Id { get; protected set; }

		public string Description { get; protected set; }

		public string ImgSrc { get; protected set; }

		public string Name { get; protected set; }

		public int Weight { get; protected set; }

		public int Size { get; protected set; }

		public AABBaseItemInfo(string description, string imgSrc, string name, int weight, int size)
		{
			Game.AssertServer();
			Id = AABBaseItemInfo.AllocateAABBaseItemInfoId();
			Description = description;
			ImgSrc = imgSrc;
			Name = name;
			Weight = weight;
			Size = size;
		}


		protected static int LastId = -1;

		public static int AllocateAABBaseItemInfoId()
		{
			LastId++;
			return LastId;
		}
	}
}
