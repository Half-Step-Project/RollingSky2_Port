using System.Collections.Generic;
using UnityEngine;

namespace VacuumShaders.CurvedWorld
{
	[ExecuteInEditMode]
	[AddComponentMenu("VacuumShaders/Curved World/Mesh Bounds Corrector")]
	public class CurvedWorld_MeshBoundsCorrector : MonoBehaviour
	{
		public float meshBoundsScale = 1f;

		private float currentMeshBoundsScale;

		private Vector3 boundsSize;

		private Bounds origBounds;

		private SkinnedMeshRenderer skinnedMeshRenderer;

		private MeshFilter meshFilter;

		private bool bIsSkinned;

		private static Dictionary<int, Bounds> boundsDictionary;

		private void OnEnable()
		{
			currentMeshBoundsScale = -1f;
		}

		private void Start()
		{
			if (boundsDictionary == null)
			{
				boundsDictionary = new Dictionary<int, Bounds>();
			}
			meshFilter = GetComponent<MeshFilter>();
			skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
			if (meshFilter != null && meshFilter.sharedMesh != null)
			{
				bIsSkinned = false;
				if (boundsDictionary.ContainsKey(meshFilter.sharedMesh.GetInstanceID()))
				{
					origBounds = boundsDictionary[meshFilter.sharedMesh.GetInstanceID()];
				}
				else
				{
					origBounds = meshFilter.sharedMesh.bounds;
					boundsDictionary.Add(meshFilter.sharedMesh.GetInstanceID(), origBounds);
				}
				boundsSize = origBounds.size;
				float num = 1f;
				if (boundsSize.x > num)
				{
					num = boundsSize.x;
				}
				if (boundsSize.y > num)
				{
					num = boundsSize.y;
				}
				if (boundsSize.z > num)
				{
					num = boundsSize.z;
				}
				boundsSize.x = (boundsSize.y = (boundsSize.z = num));
			}
			else if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null)
			{
				bIsSkinned = true;
				if (boundsDictionary.ContainsKey(skinnedMeshRenderer.sharedMesh.GetInstanceID()))
				{
					origBounds = boundsDictionary[skinnedMeshRenderer.sharedMesh.GetInstanceID()];
				}
				else
				{
					origBounds = skinnedMeshRenderer.sharedMesh.bounds;
					boundsDictionary.Add(skinnedMeshRenderer.sharedMesh.GetInstanceID(), origBounds);
				}
				boundsSize = origBounds.size;
				float num2 = 1f;
				if (boundsSize.x > num2)
				{
					num2 = boundsSize.x;
				}
				if (boundsSize.y > num2)
				{
					num2 = boundsSize.y;
				}
				if (boundsSize.z > num2)
				{
					num2 = boundsSize.z;
				}
				boundsSize.x = (boundsSize.y = (boundsSize.z = num2));
			}
			else
			{
				Debug.LogWarning("CurvedWorld_MeshBoundsCorrector: " + base.gameObject.name + " has no mesh.", base.gameObject);
				base.enabled = false;
			}
			currentMeshBoundsScale = 0f;
		}

		private void Update()
		{
			if (currentMeshBoundsScale == meshBoundsScale)
			{
				return;
			}
			if (meshBoundsScale < 0f)
			{
				meshBoundsScale = 0f;
			}
			currentMeshBoundsScale = meshBoundsScale;
			if (bIsSkinned)
			{
				if (skinnedMeshRenderer != null)
				{
					skinnedMeshRenderer.localBounds = new Bounds(skinnedMeshRenderer.localBounds.center, boundsSize * meshBoundsScale);
				}
			}
			else if (meshFilter != null && meshFilter.sharedMesh != null)
			{
				meshFilter.sharedMesh.bounds = new Bounds(meshFilter.sharedMesh.bounds.center, boundsSize * meshBoundsScale);
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			if (bIsSkinned && skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null)
			{
				Gizmos.DrawWireCube(base.transform.TransformPoint(skinnedMeshRenderer.localBounds.center), boundsSize * meshBoundsScale);
			}
			else if (meshFilter != null && meshFilter.sharedMesh != null)
			{
				Gizmos.DrawWireCube(base.transform.TransformPoint(meshFilter.sharedMesh.bounds.center), boundsSize * meshBoundsScale);
			}
		}

		private void OnDisable()
		{
			if (bIsSkinned)
			{
				if (skinnedMeshRenderer != null)
				{
					skinnedMeshRenderer.sharedMesh.bounds = origBounds;
				}
			}
			else if (meshFilter != null && meshFilter.sharedMesh != null)
			{
				meshFilter.sharedMesh.bounds = origBounds;
			}
		}
	}
}
