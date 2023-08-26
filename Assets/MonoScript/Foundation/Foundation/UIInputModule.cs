using UnityEngine;
using UnityEngine.EventSystems;

namespace Foundation
{
	public sealed class UIInputModule : StandaloneInputModule
	{
		public static UIInput UIInput { get; set; }

		protected override void Start()
		{
			if (m_InputOverride == null)
			{
				if (UIInput != null)
				{
					Object.Destroy(UIInput);
				}
				BaseInput[] components = GetComponents<BaseInput>();
				foreach (BaseInput baseInput in components)
				{
					if (!(baseInput == null) && (object)baseInput.GetType() == typeof(UIInput))
					{
						UIInput = (UIInput)baseInput;
						m_InputOverride = baseInput;
						break;
					}
				}
				if (m_InputOverride == null)
				{
					m_InputOverride = (UIInput = base.gameObject.AddComponent<UIInput>());
				}
			}
			else if (UIInput == null)
			{
				UIInput = m_InputOverride as UIInput;
			}
		}
	}
}
