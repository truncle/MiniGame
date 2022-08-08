using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ELGame
{
	public class BattleField
		: ELBehaviour
	{
		private static BattleField instance;
		public static BattleField Instance
		{
			get
			{
				return instance;
			}
		}

		//当前显示的战斗信息
		private Battle currentData;

		//格子的模型
		[SerializeField] private List<GridUnit> gridUnitModels = new List<GridUnit>();

		//挂载格子的对象
		[SerializeField] private Transform gridUnitsRoot;

		//当前地图上挂的格子
		GridUnit[,] gridUnits;

		//用来管理创建出来的格子
		List<GridUnit> gridPool;

		//加载战斗信息
		public void LoadBattleData(Battle battleData)
		{
			if (currentData != null)
				UnloadBattleData();

			currentData = battleData;

			PrepareBattleMap();
		}

		//准备加载战场
		private void PrepareBattleMap()
		{
			if (currentData == null)
			{
				EUtilityHelperL.LogError("Prepare battle map failed. No battle data.");
				return;
			}
			gridUnits = new GridUnit[currentData.mapWidth, currentData.mapHeight];

			for (int row = 0; row < currentData.mapHeight; ++row)
			{
				for (int column = 0; column < currentData.mapWidth; ++column)
				{
					Grid grid = currentData.mapGrids[column, row];
					if (grid != null)
					{
						//创建一个用于显示的格子对象
						GridUnit gu = CreateGrid(grid);
						if (gu != null)
						{
							gridUnits[column, row] = gu;
							gu.name = string.Format("Grid_{0}_{1}", row, column);
							gu.Refresh();
							gu.gameObject.SetActive(true);
						}
					}
				}
			}
		}

		//卸载战场
		private void UnloadBattleData()
		{
			RecycleAllGrids();
			currentData = null;
		}

		//创建格子
		private GridUnit CreateGrid(Grid grid)
		{
			if (gridPool == null)
				gridPool = new List<GridUnit>();

			for (int i = 0; i < gridPool.Count; ++i)
			{
				if (!gridPool[i].gameObject.activeSelf)
					return gridPool[i];
			}

			var gu = Instantiate<GridUnit>(gridUnitModels[0]);
			gu.transform.SetParent(gridUnitsRoot);
			gu.transform.localPosition = grid.localPosition;
			gu.transform.localScale = Vector3.one;
			gu.transform.localRotation = Quaternion.identity;
			gu.gridData = grid;

			gridPool.Add(gu);

			return gu;
		}

		//回收所有格子
		private void RecycleAllGrids()
		{
			if (gridPool == null)
				return;

			for (int i = 0; i < gridPool.Count; ++i)
			{
				gridPool[i].transform.localPosition = Vector3.zero;
				gridPool[i].name = "UNUSED";
				gridPool[i].gameObject.SetActive(false);
			}

			gridUnits = null;
		}

		private void Awake()
		{
			instance = this;
		}
	}
}