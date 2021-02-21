using System.Collections.Generic;

namespace Akari
{
    public class GameData
    {
        //游戏唯一id
        public int gameId;
        public GameMode gameMode;

        public int playerCount;//玩家 数量
        public string[] nameArray;//名字信息
        public int[] posArray;//位置信息
        public int[] typeIdArray;

        public GameData(GameMode gameMode,int playerCount)
        {
            this.gameMode = gameMode;
            this.playerCount = playerCount;

            nameArray = new string[playerCount];
            posArray = new int[playerCount];
            typeIdArray = new int[playerCount];
        }

        public void SavePlayerData(PlayerData playerData)
        {
            nameArray[playerData.playerId] = playerData.playerName;
            posArray[playerData.playerId] = playerData.playerPos;
            typeIdArray[playerData.playerId] = playerData.TypeId;
        }
    }
}
