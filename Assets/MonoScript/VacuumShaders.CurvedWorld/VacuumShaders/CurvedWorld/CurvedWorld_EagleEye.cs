using UnityEngine;

namespace VacuumShaders.CurvedWorld
{
	[AddComponentMenu("VacuumShaders/Curved World/Eagle Eye")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class CurvedWorld_EagleEye : MonoBehaviour
	{
		[Range(1f, 180f)]
		public float fieldOfView = 60f;

		private float savedValue;

		private Camera _camer;

		public static CurvedWorld_EagleEye get;

		private void OnEnable()
		{
			get = this;
		}

		private void OnDisable()
		{
			get = null;
		}

		private void Start()
		{
			if (_camer == null)
			{
				_camer = GetComponent<Camera>();
			}
			if (_camer != null)
			{
				savedValue = _camer.fieldOfView;
			}
		}

		private void OnPreCull()
		{
			if (_camer != null)
			{
				savedValue = _camer.fieldOfView;
				_camer.fieldOfView = Mathf.Clamp(fieldOfView, 1f, 179f);
				Shader.SetGlobalMatrix("_V_CW_Camera2World", _camer.cameraToWorldMatrix);
				Shader.SetGlobalMatrix("_V_CW_World2Camera", _camer.cameraToWorldMatrix.inverse);
				get = this;
			}
			else
			{
				get = null;
			}
		}

		private void OnPreRender()
		{
			if (_camer != null)
			{
				_camer.fieldOfView = savedValue;
			}
		}
	}
}
