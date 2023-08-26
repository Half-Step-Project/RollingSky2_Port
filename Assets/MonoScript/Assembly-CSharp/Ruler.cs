using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Ruler : MonoBehaviour
{
	public float m_speed = 6.09f;

	public float m_frameRate = 60f;

	public List<Vector3> m_positions = new List<Vector3>
	{
		new Vector3(0f, 0f, 0f),
		new Vector3(0f, 0f, 1f)
	};
}
