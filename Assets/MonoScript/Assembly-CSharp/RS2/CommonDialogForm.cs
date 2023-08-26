using UnityEngine;

namespace RS2
{
	public class CommonDialogForm : UGUIForm
	{
		private CommonDialogController commonDialogController;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			if (commonDialogController == null)
			{
				commonDialogController = base.gameObject.AddComponent<CommonDialogController>();
			}
			commonDialogController.Init();
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			commonDialogController.OnOpen();
			base.gameObject.transform.localPosition = new Vector3(base.gameObject.transform.localPosition.x, base.gameObject.transform.localPosition.y, -500f);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			commonDialogController.OnClose();
		}

		public CommonDialogController GetController()
		{
			return commonDialogController;
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			commonDialogController.Release();
		}
	}
}
