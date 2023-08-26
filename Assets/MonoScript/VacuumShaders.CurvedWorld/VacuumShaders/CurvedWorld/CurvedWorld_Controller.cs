using UnityEngine;

namespace VacuumShaders.CurvedWorld
{
	[AddComponentMenu("VacuumShaders/Curved World/Controller")]
	[ExecuteInEditMode]
	public class CurvedWorld_Controller : MonoBehaviour
	{
		[HideInInspector]
		public BEND_TYPE bendType;

		[HideInInspector]
		public Transform pivotPoint;

		private int _V_CW_PivotPoint_Position_ID;

		[HideInInspector]
		private Vector3 _V_CW_Bend = Vector3.zero;

		private int _V_CW_Bend_ID;

		[HideInInspector]
		private Vector3 _V_CW_Bias = Vector3.zero;

		private int _V_CW_Bias_ID;

		[HideInInspector]
		public float _V_CW_Bend_X;

		private float _V_CW_Bend_X_current = 1f;

		[HideInInspector]
		public float _V_CW_Bend_Y;

		private float _V_CW_Bend_Y_current = 1f;

		[HideInInspector]
		public float _V_CW_Bend_Z;

		private float _V_CW_Bend_Z_current = 1f;

		[HideInInspector]
		public float _V_CW_Bias_X;

		private float _V_CW_Bias_X_current = 1f;

		[HideInInspector]
		public float _V_CW_Bias_Y;

		private float _V_CW_Bias_Y_current = 1f;

		[HideInInspector]
		public float _V_CW_Bias_Z;

		private float _V_CW_Bias_Z_current = 1f;

		public static CurvedWorld_Controller get;

		private void OnEnable()
		{
			LoadIDs();
			EnableBend();
		}

		private void OnDisable()
		{
			DisableBend();
		}

		private void OnDestroy()
		{
			if (get == this)
			{
				get = null;
			}
		}

		private void Start()
		{
			if (get != null && get != this)
			{
				Debug.LogError("There is more then one CurvedWorld Global Controller in the scene.\nPlease ensure there is always exactly one CurvedWorld Global Controller in the scene.\n", get.gameObject);
			}
			get = this;
			LoadIDs();
		}

		private void Update()
		{
			if (get == null)
			{
				get = this;
			}
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			Shader.SetGlobalVector(_V_CW_PivotPoint_Position_ID, (pivotPoint == null) ? Vector3.zero : pivotPoint.transform.position);
			if (_V_CW_Bend_X_current != _V_CW_Bend_X || _V_CW_Bend_Y_current != _V_CW_Bend_Y || _V_CW_Bend_Z_current != _V_CW_Bend_Z)
			{
				_V_CW_Bend_X_current = _V_CW_Bend_X;
				_V_CW_Bend_Y_current = _V_CW_Bend_Y;
				_V_CW_Bend_Z_current = _V_CW_Bend_Z;
				_V_CW_Bend = new Vector3(_V_CW_Bend_X, _V_CW_Bend_Y, _V_CW_Bend_Z);
				Shader.SetGlobalVector(_V_CW_Bend_ID, _V_CW_Bend);
			}
			if (_V_CW_Bias_X_current != _V_CW_Bias_X || _V_CW_Bias_Y_current != _V_CW_Bias_Y || _V_CW_Bias_Z_current != _V_CW_Bias_Z)
			{
				if (_V_CW_Bias_X < 0f)
				{
					_V_CW_Bias_X = 0f;
				}
				if (_V_CW_Bias_Y < 0f)
				{
					_V_CW_Bias_Y = 0f;
				}
				if (_V_CW_Bias_Z < 0f)
				{
					_V_CW_Bias_Z = 0f;
				}
				_V_CW_Bias_X_current = _V_CW_Bias_X;
				_V_CW_Bias_Y_current = _V_CW_Bias_Y;
				_V_CW_Bias_Z_current = _V_CW_Bias_Z;
				_V_CW_Bias = new Vector3(_V_CW_Bias_X, _V_CW_Bias_Y, _V_CW_Bias_Z);
				Shader.SetGlobalVector(_V_CW_Bias_ID, _V_CW_Bias);
			}
		}

		private void LoadIDs()
		{
			_V_CW_PivotPoint_Position_ID = Shader.PropertyToID("_V_CW_PivotPoint_Position");
			_V_CW_Bend_ID = Shader.PropertyToID("_V_CW_Bend");
			_V_CW_Bias_ID = Shader.PropertyToID("_V_CW_Bias");
		}

		public void Reset()
		{
			Shader.SetGlobalVector(_V_CW_PivotPoint_Position_ID, Vector3.zero);
			_V_CW_Bend = Vector3.zero;
			_V_CW_Bias = Vector3.zero;
			_V_CW_Bend_X_current = (_V_CW_Bend_X = 0f);
			_V_CW_Bend_Y_current = (_V_CW_Bend_Y = 0f);
			_V_CW_Bend_Z_current = (_V_CW_Bend_Z = 0f);
			_V_CW_Bias_X_current = (_V_CW_Bias_X = 0f);
			_V_CW_Bias_Y_current = (_V_CW_Bias_Y = 0f);
			_V_CW_Bias_Z_current = (_V_CW_Bias_Z = 0f);
		}

		public void ForceUpdate()
		{
			LoadIDs();
			Shader.SetGlobalVector(_V_CW_PivotPoint_Position_ID, (pivotPoint == null) ? Vector3.zero : pivotPoint.transform.position);
			_V_CW_Bend = new Vector3(_V_CW_Bend_X, _V_CW_Bend_Y, _V_CW_Bend_Z);
			Shader.SetGlobalVector(_V_CW_Bend_ID, _V_CW_Bend);
			_V_CW_Bias = new Vector3(_V_CW_Bias_X, _V_CW_Bias_Y, _V_CW_Bias_Z);
			Shader.SetGlobalVector(_V_CW_Bias_ID, _V_CW_Bias);
		}

		public void EnableBend()
		{
			ForceUpdate();
		}

		public void DisableBend()
		{
			LoadIDs();
			Shader.SetGlobalVector(_V_CW_PivotPoint_Position_ID, Vector3.zero);
			Shader.SetGlobalVector(_V_CW_Bend_ID, Vector3.zero);
			Shader.SetGlobalVector(_V_CW_Bias_ID, Vector3.zero);
		}

		public static Vector3 TransformPoint(Vector3 _transformPoint, BEND_TYPE _bendType, Vector3 _bendSize, Vector3 _bendBias, Vector3 _pivotPoint)
		{
			switch (_bendType)
			{
			case BEND_TYPE.ClassicRunner:
			{
				Vector3 vector = _transformPoint - _pivotPoint;
				float num3 = Mathf.Max(0f, vector.z - _bendBias.x);
				float num4 = Mathf.Max(0f, vector.z - _bendBias.y);
				vector = new Vector3((0f - _bendSize.y) * num4 * num4, _bendSize.x * num3 * num3, 0f) * 0.001f;
				return _transformPoint + vector;
			}
			case BEND_TYPE.LittlePlanet:
			{
				Vector3 vector3 = _transformPoint - _pivotPoint;
				float num8 = Mathf.Max(0f, Mathf.Abs(vector3.z) - _bendBias.x) * ((vector3.z < 0f) ? (-1f) : 1f);
				float num9 = Mathf.Max(0f, Mathf.Abs(vector3.x) - _bendBias.z) * ((vector3.x < 0f) ? (-1f) : 1f);
				vector3 = new Vector3(0f, (_bendSize.x * num8 * num8 + _bendSize.z * num9 * num9) * 0.001f, 0f);
				return _transformPoint + vector3;
			}
			case BEND_TYPE.Universal:
			{
				Vector3 vector2 = _transformPoint - _pivotPoint;
				float num5 = Mathf.Max(0f, Mathf.Abs(vector2.z) - _bendBias.x) * ((vector2.z < 0f) ? (-1f) : 1f);
				float num6 = Mathf.Max(0f, Mathf.Abs(vector2.z) - _bendBias.y) * ((vector2.z < 0f) ? (-1f) : 1f);
				float num7 = Mathf.Max(0f, Mathf.Abs(vector2.x) - _bendBias.z) * ((vector2.x < 0f) ? (-1f) : 1f);
				vector2 = new Vector3((0f - _bendSize.y) * num6 * num6, _bendSize.x * num5 * num5 + num7 * num7 * _bendSize.z, 0f) * 0.001f;
				return _transformPoint + vector2;
			}
			case BEND_TYPE.Perspective2D:
			{
				Vector3 point = _transformPoint - _pivotPoint;
				point = Camera.main.worldToCameraMatrix.MultiplyPoint(point);
				float num = Mathf.Max(0f, Mathf.Abs(point.y) - _bendBias.x) * ((point.y < 0f) ? (-1f) : 1f);
				num *= num;
				float num2 = Mathf.Max(0f, Mathf.Abs(point.x) - _bendBias.y) * ((point.x < 0f) ? (-1f) : 1f);
				num2 *= num2;
				Vector3 point2 = point;
				point2.z -= (_bendSize.x * num + _bendSize.y * num) * 0.001f;
				return Camera.main.worldToCameraMatrix.inverse.MultiplyPoint(point2);
			}
			default:
				return _transformPoint;
			}
		}

		public Vector3 TransformPoint(Vector3 _transformPoint)
		{
			if (!base.enabled)
			{
				return _transformPoint;
			}
			return TransformPoint(_transformPoint, bendType, GetBend(), GetBias(), (pivotPoint == null) ? Vector3.zero : pivotPoint.position);
		}

		public Vector3 GetBend()
		{
			return _V_CW_Bend;
		}

		public void SetBend(Vector3 _newBend)
		{
			_V_CW_Bend_X = _newBend.x;
			_V_CW_Bend_Y = _newBend.y;
			_V_CW_Bend_Z = _newBend.z;
			_V_CW_Bend = new Vector3(_V_CW_Bend_X, _V_CW_Bend_Y, _V_CW_Bend_Z);
		}

		public Vector3 GetBias()
		{
			return _V_CW_Bias;
		}

		public void SetBias(Vector3 _newBias)
		{
			if (_newBias.x < 0f)
			{
				_newBias.x = 0f;
			}
			if (_newBias.y < 0f)
			{
				_newBias.y = 0f;
			}
			if (_newBias.z < 0f)
			{
				_newBias.z = 0f;
			}
			_V_CW_Bias_X = _newBias.x;
			_V_CW_Bias_Y = _newBias.y;
			_V_CW_Bias_Z = _newBias.z;
			_V_CW_Bias = new Vector3(_V_CW_Bias_X, _V_CW_Bias_Y, _V_CW_Bias_Z);
		}
	}
}
