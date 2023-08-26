using UnityEngine;

namespace Foundation
{
	public interface IHearable
	{
		Vector3 Position { get; }

		bool ActiveAndEnabled { get; }
	}
}
