using System.Collections;
using UnityEngine;

namespace FastShadow
{
	public sealed class FastShadowManager : MonoBehaviour
	{
		private static FastShadowManager _instance;

		private Hashtable shadowMeshes = new Hashtable();

		private Hashtable shadowMeshesStatic = new Hashtable();

		private int frameCalcedFustrum;

		private Plane[] fustrumPlanes;

		public static FastShadowManager instance
		{
			get
			{
				if (_instance == null)
				{
					FastShadowManager fastShadowManager = Object.FindObjectOfType<FastShadowManager>();
					if (fastShadowManager == null)
					{
						_instance = new GameObject("FastShadowManager").AddComponent<FastShadowManager>();
					}
					else
					{
						_instance = fastShadowManager;
					}
				}
				return _instance;
			}
		}

		private void Start()
		{
			FastShadowManager[] array = Object.FindObjectsOfType<FastShadowManager>();
			if (array.Length > 1)
			{
				Debug.LogWarning("There should only be one FS_ShadowManger in the scene. Found " + array.Length);
			}
		}

		private void OnDestroy()
		{
			shadowMeshes.Clear();
			shadowMeshesStatic.Clear();
		}

		public void RegisterGeometry(FastShadow shadow, MeshKey meshKey)
		{
			FastShadowMesh fastShadowMesh;
			if (meshKey.isStatic)
			{
				if (!shadowMeshesStatic.ContainsKey(meshKey))
				{
					GameObject obj = new GameObject("ShadowMeshStatic_" + meshKey.material.name);
					obj.transform.parent = base.transform;
					fastShadowMesh = obj.AddComponent<FastShadowMesh>();
					fastShadowMesh.shadowMaterial = shadow.shadowMaterial;
					fastShadowMesh.isStatic = true;
					shadowMeshesStatic.Add(meshKey, fastShadowMesh);
				}
				else
				{
					fastShadowMesh = (FastShadowMesh)shadowMeshesStatic[meshKey];
				}
			}
			else if (!shadowMeshes.ContainsKey(meshKey))
			{
				GameObject obj2 = new GameObject("ShadowMesh_" + meshKey.material.name);
				obj2.transform.parent = base.transform;
				fastShadowMesh = obj2.AddComponent<FastShadowMesh>();
				fastShadowMesh.shadowMaterial = shadow.shadowMaterial;
				fastShadowMesh.isStatic = false;
				shadowMeshes.Add(meshKey, fastShadowMesh);
			}
			else
			{
				fastShadowMesh = (FastShadowMesh)shadowMeshes[meshKey];
			}
			fastShadowMesh.RegisterGeometry(shadow);
		}

		public Plane[] GetCameraFustrumPlanes()
		{
			if (Time.frameCount != frameCalcedFustrum || fustrumPlanes == null)
			{
				Camera main = Camera.main;
				if (main == null)
				{
					Debug.LogWarning("No main camera could be found for visibility culling.");
					fustrumPlanes = null;
				}
				else
				{
					fustrumPlanes = GeometryUtility.CalculateFrustumPlanes(main);
					frameCalcedFustrum = Time.frameCount;
				}
			}
			return fustrumPlanes;
		}
	}
}
