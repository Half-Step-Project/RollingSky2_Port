namespace RisingWin.Library
{
	public class AxisName
	{
		public const int SINGLE_INDEX = 4;

		public static string[] HORIZONTAL = new string[5] { "Horizontal 1", "Horizontal 2", "Horizontal 3", "Horizontal 4", "Horizontal" };

		public static string[] VERTICAL = new string[5] { "Vertical 1", "Vertical 2", "Vertical 3", "Vertical 4", "Vertical" };

		public static string[] HORIZONTAL_R = new string[5] { "HorizontalR 1", "HorizontalR 2", "HorizontalR 3", "HorizontalR 4", "HorizontalR" };

		public static string[] VERTICAL_R = new string[5] { "VerticalR 1", "VerticalR 2", "VerticalR 3", "VerticalR 4", "VerticalR" };

		public static string GetAxisName(AxisType axis)
		{
			switch (axis)
			{
			case AxisType.HORIZONTAL:
				return HORIZONTAL[4];
			case AxisType.VERTICAL:
				return VERTICAL[4];
			case AxisType.HORIZONTAL_R:
				return HORIZONTAL_R[4];
			case AxisType.VERTICAL_R:
				return VERTICAL_R[4];
			default:
				return string.Empty;
			}
		}

		public static string GetAxisName(AxisType axis, int index)
		{
			switch (axis)
			{
			case AxisType.HORIZONTAL:
				return HORIZONTAL[index];
			case AxisType.VERTICAL:
				return VERTICAL[index];
			case AxisType.HORIZONTAL_R:
				return HORIZONTAL_R[4];
			case AxisType.VERTICAL_R:
				return VERTICAL_R[4];
			default:
				return string.Empty;
			}
		}
	}
}
