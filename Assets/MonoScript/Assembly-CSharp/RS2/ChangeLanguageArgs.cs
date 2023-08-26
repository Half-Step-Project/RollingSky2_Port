using Foundation;
using UnityEngine;

namespace RS2
{
	public class ChangeLanguageArgs : EventArgs<ChangeLanguageArgs>
	{
		public SystemLanguage language;

		public ChangeLanguageArgs Initialize(SystemLanguage language)
		{
			this.language = language;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
