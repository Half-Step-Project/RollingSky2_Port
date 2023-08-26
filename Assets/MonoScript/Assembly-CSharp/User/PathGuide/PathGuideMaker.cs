using System.Collections.Generic;
using UnityEngine;

namespace User.PathGuide
{
	public class PathGuideMaker : MonoBehaviour
	{
		[HideInInspector]
		public RolePathTable mRolePathTable;

		[HideInInspector]
		public int mStartIndex;

		[HideInInspector]
		public int mEndIndex;

		[HideInInspector]
		public bool mIgnoreRotate = true;

		[HideInInspector]
		public Vector3 mOffset = new Vector3(0f, 0.5f, 0f);

		[HideInInspector]
		public float mHandleRadius = 0.5f;

		[HideInInspector]
		public int mSmooth = 10;

		public PathGuideMakerData GetPathGuideMakerData()
		{
			PathGuideMakerData result = default(PathGuideMakerData);
			if (mRolePathTable == null || mRolePathTable.PathData == null || mRolePathTable.PathData.Count == 0)
			{
				return result;
			}
			if (mStartIndex < 0 || mStartIndex >= mEndIndex)
			{
				return result;
			}
			if (mRolePathTable.PathData.Count < mEndIndex)
			{
				return result;
			}
			List<Vector3> list = new List<Vector3>();
			for (int i = mStartIndex; i <= mEndIndex; i++)
			{
				if (mIgnoreRotate)
				{
					list.Add(mRolePathTable.PathData[i].Position + mOffset);
				}
				else
				{
					list.Add(mRolePathTable.PathData[i].Position + mRolePathTable.PathData[i].PartRotation * mOffset);
				}
			}
			List<Vector3> list2 = new List<Vector3>();
			int num = list.Count / mSmooth * mSmooth;
			if (list.Count > 0)
			{
				for (int j = 0; j < num; j += mSmooth)
				{
					list2.Add(list[j]);
				}
				list2.Add(list[list.Count - 1]);
			}
			Debug.Log("get count =" + list.Count + " , copy count =" + list2.Count);
			result.mPos = list2.ToArray();
			return result;
		}

		private void OnDrawGizmos()
		{
			if (mRolePathTable == null || mRolePathTable.PathData == null || mRolePathTable.PathData.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < mRolePathTable.PathData.Count - 1; i++)
			{
				Gizmos.color = Color.yellow;
				if (mStartIndex >= 0 && mStartIndex < mEndIndex && mStartIndex <= i && mEndIndex > i)
				{
					Gizmos.color = Color.red;
				}
				Gizmos.DrawLine(mRolePathTable.PathData[i].Position, mRolePathTable.PathData[i + 1].Position);
			}
		}
	}
}
