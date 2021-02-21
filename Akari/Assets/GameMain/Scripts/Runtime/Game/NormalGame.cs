using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akari
{
    public class NormalGame : GameBase
    {
        private GameData gameData;//游戏数据

        private GameMap gameMap;//游戏地图
        private int maxStepCount;//最大步数

        private PlayerData[] playerDatas;
        private Player[] players;
        private int playerCount;

        private int curPlayerIndex;

        public override GameMode GameMode
        {
            get
            {
                return gameData.gameMode;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            playerCount = 2;

            gameData = new GameData(GameMode.Normal, playerCount);
            gameMap = Object.FindObjectOfType<GameMap>();
            playerDatas = new PlayerData[playerCount];
            players = new Player[playerCount];

            playerDatas[0] = new PlayerData(GameEntry.Entity.GenerateSerialId(), 10000, 0, "akl", 0)
            {
                Position = gameMap.posArray[0].localPosition,
            };
            playerDatas[1] = new PlayerData(GameEntry.Entity.GenerateSerialId(), 10000, 1, "sxt", 0)
            {
                Position = gameMap.posArray[0].localPosition,
            };

            for (int i = 0; i < playerCount; i++)
            {
                GameEntry.Entity.ShowPlayer(playerDatas[i]);
                gameData.SavePlayerData(playerDatas[i]);
            }

            maxStepCount = gameMap.posArray.Length;
        }

        public override void Shutdown()
        {
            base.Shutdown();
            gameData = null;
        }

        public override void SaveGame()
        {
            base.SaveGame();
            GameEntry.Setting.SaveGame(gameData);
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //开始 游戏 获取角色
                for (int i = 0; i < playerCount; i++)
                {
                    var player = GameEntry.Entity.GetEntity(playerDatas[i].Id);
                    players[i] = player.GetComponent<Player>();
                }

                curPlayerIndex = 0;
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                PlayerRun();
            }
        }

        private void PlayerRun()
        {
            //随机骰子
            int step = Random.Range(1, 4);
            Debug.Log(step);

            //移动
            var player = players[curPlayerIndex];
            var path = new Vector3[step];
            int curPos = player.PlayerData.playerPos;
            for (int i = 0; i < step; i++)
            {
                path[i] = gameMap.posArray[curPos + i + 1].position;
            }
            player.MoveToTargetPoint(path);

            //弹出信息界面
            //效果
            //选择
        }
    }
}
