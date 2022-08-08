using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ELGame
{
    // 地图数据生成器
    public class BattleCreator 
    {
        private static BattleCreator instance;
        public static BattleCreator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BattleCreator();
                    instance.Init();
                }
                return instance;
            }
        }

        private bool inited = false;

        private void Init()
        {
            if (inited)
                return;

            inited = true;
            EUtilityHelperL.Log("Battle creator inited.");
        }

        //创建一场战斗
        public Battle CreateBattle()
        {
            Battle battle = new Battle();
            battle.Generate(16, 8, 10, 2);
            return battle;
        }
    }
}