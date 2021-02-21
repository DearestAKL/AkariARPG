using GameFramework;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace Akari
{
    public static class SettingExtension
    {
        #region 玩家定制卡牌
        public static void SaveCard(this SettingComponent setting, UserCard userCard)
        {
            var userCardList = setting.GetObject<List<UserCard>>(Constant.Setting.UserCard);

            bool HasCard = false;
            for (int i = 0; i < userCardList.Count; i++)
            {
                if(userCardList[i].id == userCard.id)
                {
                    //覆盖
                    userCardList[i].name = userCard.name;
                    userCardList[i].content = userCard.content;
                    userCardList[i].type = userCard.type;
                    HasCard = true;
                }
            }

            //保存
            if (!HasCard)
                userCardList.Add(userCard);

            setting.SetObject(Constant.Setting.UserCard, userCardList);
        }

        public static void RemoveCard(this SettingComponent setting, int cardId)
        {
            var userCardList = setting.GetObject<List<UserCard>>(Constant.Setting.UserCard);

            for (int i = 0; i < userCardList.Count; i++)
            {
                if (userCardList[i].id == cardId)
                {
                    userCardList.RemoveAt(i);
                    setting.SetObject(Constant.Setting.UserCard, userCardList);
                    break;
                }
            }
        }

        public static List<UserCard> GetUserCards(this SettingComponent setting)
        {
            var userCardList = setting.GetObject<List<UserCard>>(Constant.Setting.UserCard);

            return userCardList;
        }
        #endregion

        #region 游戏存档

        public static void SaveGame(this SettingComponent setting,GameData gameData)
        {
            var gameDataList = setting.GetObject<List<GameData>>(Constant.Setting.GameData);

            bool HasGame = false;
            for (int i = 0; i < gameDataList.Count; i++)
            {
                if (gameDataList[i].gameId == gameData.gameId)
                {
                    //覆盖
                    gameDataList[i] = gameData;
                    HasGame = true;
                }
            }

            //保存
            if (!HasGame)
                gameDataList.Add(gameData);

            setting.SetObject(Constant.Setting.GameData, gameDataList);
        }

        public static void RemoveGameData(this SettingComponent setting, int gameId)
        {
            var gameDataList = setting.GetObject<List<GameData>>(Constant.Setting.GameData);

            for (int i = 0; i < gameDataList.Count; i++)
            {
                if (gameDataList[i].gameId == gameId)
                {
                    gameDataList.RemoveAt(i);
                    setting.SetObject(Constant.Setting.GameData, gameDataList);
                    break;
                }
            }
        }

        public static List<GameData> GetGameDatas(this SettingComponent setting)
        {
            var gameDataList = setting.GetObject<List<GameData>>(Constant.Setting.GameData);

            return gameDataList;
        }
        #endregion
    }
}
