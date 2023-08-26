using UnityEngine;

public class Reflect : MonoBehaviour
{
	public float ClipPlaneOffset;

	public float MinBoundRelativeHeight = 0.04f;

	public Color ClearColor = Color.clear;

	private Camera m_ReflectionCamera;

	private Transform m_ReflectionPlaneTransform;

	private Material m_ReflectionMaterial;

	private RenderTexture m_ReflectionTexture;

	private void Start()
	{
		m_ReflectionPlaneTransform = base.transform;
		m_ReflectionMaterial = GetComponent<Renderer>().sharedMaterial;
		int width = Mathf.FloorToInt((float)Camera.main.pixelWidth * 0.5f);
		int height = Mathf.FloorToInt((float)Camera.main.pixelHeight * 0.5f);
		int depth = 16;
		m_ReflectionTexture = new RenderTexture(width, height, depth, RenderTextureFormat.RGB565);
		m_ReflectionTexture.Create();
		m_ReflectionTexture.hideFlags = HideFlags.DontSave;
		GameObject gameObject = new GameObject();
		gameObject.name = "reflectCamera";
		m_ReflectionCamera = gameObject.AddComponent<Camera>();
		m_ReflectionCamera.CopyFrom(Camera.main);
		m_ReflectionCamera.clearFlags = CameraClearFlags.Color;
		m_ReflectionCamera.backgroundColor = ClearColor;
		m_ReflectionCamera.cullingMask = 1 << LayerMask.NameToLayer("Reflectable");
		m_ReflectionCamera.targetTexture = m_ReflectionTexture;
		m_ReflectionCamera.enabled = false;
	}

	public void OnWillRenderObject()
	{
		RenderReflection();
	}

	protected void OnDestroy()
	{
		if ((bool)m_ReflectionCamera)
		{
			m_ReflectionCamera.targetTexture = null;
			Object.DestroyImmediate(m_ReflectionCamera.gameObject);
			m_ReflectionCamera = null;
		}
		m_ReflectionTexture.Release();
		m_ReflectionTexture = null;
		m_ReflectionMaterial = null;
		m_ReflectionPlaneTransform = null;
	}

	private void RenderReflection()
	{
		Vector3 up = m_ReflectionPlaneTransform.up;
		Vector3 position = base.transform.position;
		Camera main = Camera.main;
		float w = 0f - Vector3.Dot(up, position) - ClipPlaneOffset;
		Vector4 plane = new Vector4(up.x, up.y, up.z, w);
		Matrix4x4 reflectionMat;
		CalculateReflectionMatrix(out reflectionMat, plane);
		Vector3 position2 = main.transform.position;
		Vector3 position3 = reflectionMat.MultiplyPoint(position2);
		m_ReflectionCamera.worldToCameraMatrix = main.worldToCameraMatrix * reflectionMat;
		Vector4 clipPlane = CameraSpacePlane(m_ReflectionCamera, position, up, 1f);
		m_ReflectionCamera.projectionMatrix = main.CalculateObliqueMatrix(clipPlane);
		m_ReflectionCamera.cullingMatrix = main.projectionMatrix * main.worldToCameraMatrix;
		bool invertCulling = GL.invertCulling;
		GL.invertCulling = !invertCulling;
		m_ReflectionCamera.transform.position = position3;
		Vector3 eulerAngles = main.transform.eulerAngles;
		m_ReflectionCamera.transform.eulerAngles = new Vector3(0f - eulerAngles.x, eulerAngles.y, eulerAngles.z);
		m_ReflectionCamera.Render();
		GL.invertCulling = invertCulling;
		m_ReflectionCamera.targetTexture.wrapMode = TextureWrapMode.Repeat;
		m_ReflectionMaterial.SetTexture("_ReflectTex", m_ReflectionCamera.targetTexture);
	}

	private Vector4 CameraSpacePlane(Camera camera, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 point = pos + normal * ClipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = camera.worldToCameraMatrix;
		Vector3 lhs = worldToCameraMatrix.MultiplyPoint(point);
		Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(rhs.x, rhs.y, rhs.z, 0f - Vector3.Dot(lhs, rhs));
	}

	private static void CalculateReflectionMatrix(out Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
		reflectionMat.m01 = -2f * plane[0] * plane[1];
		reflectionMat.m02 = -2f * plane[0] * plane[2];
		reflectionMat.m03 = -2f * plane[3] * plane[0];
		reflectionMat.m10 = -2f * plane[1] * plane[0];
		reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
		reflectionMat.m12 = -2f * plane[1] * plane[2];
		reflectionMat.m13 = -2f * plane[3] * plane[1];
		reflectionMat.m20 = -2f * plane[2] * plane[0];
		reflectionMat.m21 = -2f * plane[2] * plane[1];
		reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
		reflectionMat.m23 = -2f * plane[3] * plane[2];
		reflectionMat.m30 = 0f;
		reflectionMat.m31 = 0f;
		reflectionMat.m32 = 0f;
		reflectionMat.m33 = 1f;
	}

	private static float Sign(float x)
	{
		if (x > 0f)
		{
			return 1f;
		}
		if (x < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	private static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 b = projection.inverse * new Vector4(Sign(clipPlane.x), Sign(clipPlane.y), 1f, 1f);
		Vector4 vector = clipPlane * (2f / Vector4.Dot(clipPlane, b));
		projection[2] = vector.x - projection[3];
		projection[6] = vector.y - projection[7];
		projection[10] = vector.z - projection[11];
		projection[14] = vector.w - projection[15];
	}
}
