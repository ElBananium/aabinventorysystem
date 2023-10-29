using System.Collections.Generic;

namespace Sandbox.InventoryUI.BaseInventoryUI;

public class InventorySlot
{
	public int SlotId { get; set; }

	public IEnumerable<int> ItemIds { get; set; }
	public int ItemInfoId { get; set; }
	public string Name { get; set; }

	public string Description { get; set; }

	public int TotalSize { get; set; }

	public int TotalWeight { get; set; }
	public string ImgSrc { get; set; }

	public int Count { get; set; }


}
