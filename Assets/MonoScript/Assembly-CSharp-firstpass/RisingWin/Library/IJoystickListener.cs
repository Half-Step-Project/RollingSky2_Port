namespace RisingWin.Library
{
	public interface IJoystickListener
	{
		void OnDirectionTrigger(JoystickDirection pDirection, float pValue);

		void OnConsequentDirectionTrigger(JoystickDirection pDirection, float pValue);

		void OnButtonTrigger(JoystickButton pButton);
	}
}
