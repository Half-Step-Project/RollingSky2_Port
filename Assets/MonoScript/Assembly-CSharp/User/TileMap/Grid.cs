using System.Collections.Generic;
using UnityEngine;

namespace User.TileMap
{
	public class Grid : MonoBehaviour
	{
		private BoxCollider m_collider;

		[Label]
		public int m_id;

		public int m_x = 10;

		public int m_y = 21;

		public float m_samplingInterval = 3f;

		public float m_samplingCenterY;

		public bool m_isFrist;

		public List<Grid> m_parentGrids = new List<Grid>();

		public static bool m_isShowDraw = true;

		public static bool m_isShowCollider = false;

		public static bool m_isShowTriggerBox = false;

		public static bool m_isShowTileCollider = false;

		private Mesh m_GridMesh;

		public void RefreshCollider(bool isEnable = true)
		{
			if (m_collider == null)
			{
				m_collider = base.gameObject.GetComponent<BoxCollider>();
				if (m_collider == null)
				{
					m_collider = base.gameObject.AddComponent<BoxCollider>();
				}
			}
			m_collider.center = new Vector3((float)m_y * 1f / 2f, -0.005f, (float)m_x * 1f / 2f);
			m_collider.size = new Vector3(m_y, 0.01f, m_x);
			m_collider.enabled = isEnable;
		}

		private void OnDrawGizmos()
		{
			OnDraw();
		}

		private void OnDraw()
		{
			if (!m_isShowDraw)
			{
				return;
			}
			DrawPlane();
			Gizmos.color = Color.gray;
			for (int i = 0; i < m_x + 1; i++)
			{
				Vector3 from = base.transform.TransformPoint(new Vector3(0f, 0f, i));
				Vector3 to = base.transform.TransformPoint(new Vector3(m_y, 0f, i));
				Gizmos.DrawLine(from, to);
			}
			for (int j = 0; j < m_y + 1; j++)
			{
				Vector3 from2 = base.transform.TransformPoint(new Vector3(j, 0f, 0f));
				Vector3 to2 = base.transform.TransformPoint(new Vector3(j, 0f, m_x));
				Gizmos.DrawLine(from2, to2);
			}
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(base.transform.TransformPoint(new Vector3(0f, 0f, 0f)), base.transform.TransformPoint(new Vector3(m_y, 0f, 0f)));
			Gizmos.DrawLine(base.transform.TransformPoint(new Vector3(0f, 0f, 0f)), base.transform.TransformPoint(new Vector3(0f, 0f, m_x)));
			Gizmos.DrawLine(base.transform.TransformPoint(new Vector3(0f, 0f, m_x)), base.transform.TransformPoint(new Vector3(m_y, 0f, m_x)));
			Gizmos.DrawLine(base.transform.TransformPoint(new Vector3(m_y, 0f, 0f)), base.transform.TransformPoint(new Vector3(m_y, 0f, m_x)));
			if (m_y >= 8)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(base.transform.TransformPoint(new Vector3(8f, 0f, 0f)), base.transform.TransformPoint(new Vector3(m_y - 8, 0f, 0f)));
				Gizmos.DrawLine(base.transform.TransformPoint(new Vector3(8f, 0f, 0f)), base.transform.TransformPoint(new Vector3(8f, 0f, m_x)));
				Gizmos.DrawLine(base.transform.TransformPoint(new Vector3(8f, 0f, m_x)), base.transform.TransformPoint(new Vector3(m_y - 8, 0f, m_x)));
				Gizmos.DrawLine(base.transform.TransformPoint(new Vector3(m_y - 8, 0f, 0f)), base.transform.TransformPoint(new Vector3(m_y - 8, 0f, m_x)));
			}
			Gizmos.color = Color.cyan;
			for (int k = 0; k < m_parentGrids.Count; k++)
			{
				if (m_parentGrids[k] != null)
				{
					Vector3[] pathByPositions = Bezier.GetPathByPositions(new Vector3[2]
					{
						base.transform.TransformPoint(0f, 0f, 0f),
						m_parentGrids[k].transform.TransformPoint(0f, 0f, m_parentGrids[k].m_x)
					});
					for (int l = 0; l < pathByPositions.Length - 1; l++)
					{
						Gizmos.DrawLine(pathByPositions[l], pathByPositions[l + 1]);
					}
					Gizmos.DrawSphere(m_parentGrids[k].transform.TransformPoint(0f, 0f, m_parentGrids[k].m_x), 0.5f);
				}
			}
			Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			Vector3 zero3 = Vector3.zero;
			Vector3 zero12 = Vector3.zero;
			Vector3 zero4 = Vector3.zero;
			Vector3 zero5 = Vector3.zero;
			Vector3 zero6 = Vector3.zero;
			Vector3 zero7 = Vector3.zero;
			Vector3 zero8 = Vector3.zero;
			Vector3 zero9 = Vector3.zero;
			Vector3 zero10 = Vector3.zero;
			Vector3 zero11 = Vector3.zero;
			float num = m_samplingInterval / 2f;
			zero = base.transform.TransformPoint(0f, m_samplingCenterY, 0f);
			zero2 = base.transform.TransformPoint(m_y, m_samplingCenterY, 0f);
			zero3 = base.transform.TransformPoint(m_y, m_samplingCenterY, m_x);
			Vector3 from3 = base.transform.TransformPoint(0f, m_samplingCenterY, m_x);
			zero4 = base.transform.TransformPoint(m_y, m_samplingCenterY - num, 0f);
			zero5 = base.transform.TransformPoint(m_y, m_samplingCenterY - num, m_x);
			zero6 = base.transform.TransformPoint(0f, m_samplingCenterY - num, m_x);
			zero7 = base.transform.TransformPoint(0f, m_samplingCenterY - num, 0f);
			zero8 = base.transform.TransformPoint(m_y, m_samplingCenterY + num, 0f);
			zero9 = base.transform.TransformPoint(m_y, m_samplingCenterY + num, m_x);
			zero10 = base.transform.TransformPoint(0f, m_samplingCenterY + num, m_x);
			zero11 = base.transform.TransformPoint(0f, m_samplingCenterY + num, 0f);
			Gizmos.DrawLine(zero, zero7);
			Gizmos.DrawLine(from3, zero6);
			Gizmos.DrawLine(zero3, zero5);
			Gizmos.DrawLine(zero2, zero4);
			Gizmos.DrawLine(zero4, zero5);
			Gizmos.DrawLine(zero5, zero6);
			Gizmos.DrawLine(zero6, zero7);
			Gizmos.DrawLine(zero7, zero4);
			Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
			Gizmos.DrawLine(zero, zero11);
			Gizmos.DrawLine(zero2, zero8);
			Gizmos.DrawLine(zero3, zero9);
			Gizmos.DrawLine(from3, zero10);
			Gizmos.DrawLine(zero8, zero9);
			Gizmos.DrawLine(zero9, zero10);
			Gizmos.DrawLine(zero10, zero11);
			Gizmos.DrawLine(zero11, zero8);
		}

		private void DrawPlane()
		{
			if (m_GridMesh == null)
			{
				m_GridMesh = new Mesh();
			}
			Gizmos.color = new Color(0f, 0f, 0f, 0.5f);
			Vector3[] vertices = new Vector3[4]
			{
				new Vector3(0f, 0f, 0f),
				new Vector3(0f, 0f, m_x),
				new Vector3(m_y, 0f, 0f),
				new Vector3(m_y, 0f, m_x)
			};
			int[] triangles = new int[6] { 0, 1, 2, 3, 2, 1 };
			m_GridMesh.vertices = vertices;
			m_GridMesh.triangles = triangles;
			m_GridMesh.RecalculateNormals();
			Gizmos.DrawMesh(m_GridMesh, base.transform.position, base.transform.rotation);
		}

		public bool IsOnGrid(Vector3 position)
		{
			Vector3 vector = base.transform.InverseTransformPoint(position);
			if (vector.x >= 0f && vector.x <= (float)m_y && vector.z >= 0f && vector.z < (float)m_x)
			{
				float y = vector.y;
				float num = m_samplingInterval / 2f;
				float num2 = m_samplingCenterY - num;
				float num3 = m_samplingCenterY + num;
				if (y >= num2 && y <= num3)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsOnGridForIgnoremSamplingInterval(Vector3 position)
		{
			Vector3 vector = base.transform.InverseTransformPoint(position);
			if (vector.x >= 0f && vector.x <= (float)m_y && vector.z >= 0f && vector.z <= (float)m_x)
			{
				return true;
			}
			return false;
		}

		public Point GetPointByPosition(Vector3 position)
		{
			Vector3 vector = base.transform.InverseTransformPoint(position);
			return new Point(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.z));
		}

		private void FindParentGrids(Grid grid, ref List<Grid> grids)
		{
			for (int i = 0; i < grid.m_parentGrids.Count; i++)
			{
				if (grid.m_parentGrids[i] != null && !grids.Contains(grid.m_parentGrids[i]))
				{
					grids.Add(grid.m_parentGrids[i]);
					FindParentGrids(grid.m_parentGrids[i], ref grids);
				}
			}
		}

		public Grid[] GetParentGridsByGrid()
		{
			List<Grid> grids = new List<Grid>();
			FindParentGrids(this, ref grids);
			return grids.ToArray();
		}
	}
}
