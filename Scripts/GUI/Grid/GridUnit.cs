using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ELGame
{
	public class GridUnit
		: ELBehaviour
	{
		public Grid gridData;
		[SerializeField] protected SpriteRenderer tileRenderer;


		public void Refresh()
		{
			tileRenderer.transform.localScale = Vector3.one * EGameConstL.GridWidth;
			//根据格子类型切换颜色
			switch (gridData.gridType)
			{
				case GridType.Normal:
					tileRenderer.color = Color.white;
					break;

				case GridType.Obstacle:
					tileRenderer.color = Color.gray;
					break;

				default:
					tileRenderer.color = Color.white;
					break;
			}
		}
	}
}