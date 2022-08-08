using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ELGame
{
	public class Main
		: ELBehaviour
	{
		private Battle battle;

		private void Awake()
		{
			Random.InitState((int)System.DateTime.Now.Ticks);
		}

		private void Start()
		{
			// 创建一个战场
			battle = BattleCreator.Instance.CreateBattle();
			BattleField.Instance.LoadBattleData(battle);
		}
	}
}