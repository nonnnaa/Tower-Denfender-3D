using System;
using System.Collections.Generic;

namespace RapidIcon_1_7_2
{
	[Serializable]
	public class IconSetData
	{
		public List<IconSet> iconSets;

		public IconSetData()
		{
			iconSets = new List<IconSet>();
		}
	}
}