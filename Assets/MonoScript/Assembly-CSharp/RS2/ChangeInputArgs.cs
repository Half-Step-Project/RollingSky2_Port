using Foundation;

namespace RS2
{
	public class ChangeInputArgs : EventArgs<ChangeInputArgs>
	{
		public InputSystem.InputState inputState;

		public ChangeInputArgs Initialize(InputSystem.InputState inputstate)
		{
			inputState = inputstate;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
