using Foundation;

namespace RS2
{
	public struct BuiltinDialogParams
	{
		public string ProgreeMessage { get; set; }

		public bool PauseGame { get; set; }

		public string ConfirmText { get; set; }

		public Action<object> OnClickConfirm { get; set; }

		public string CancelText { get; set; }

		public Action<object> OnClickCancel { get; set; }

		public string UserData { get; set; }

		public BuiltinDialogShowType ShowType { get; set; }

		public string AlertMessage { get; set; }

		public string InfoMessage { get; set; }

		public float Progress { get; set; }
	}
}
