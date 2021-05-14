using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Akari
{
    public class AtlasHelperComponent
    {
        private ResourceComponent resourceComponent;
        //缓存图集资源
        private readonly Dictionary<string, SpriteAtlas> atlasMap = new Dictionary<string, SpriteAtlas>();

        private const string str_Common = "Common";

        public void Initialize()
        {
            resourceComponent = GameEntry.Resource;

            //AddSpriteAtlas(str_Common);
        }

        private void AddSpriteAtlas(string atlasName)
        {
            resourceComponent.LoadAsset(AssetUtility.GetSpriteAtlasAsset(atlasName), typeof(SpriteAtlas), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    atlasMap.Add(atlasName, asset as SpriteAtlas);
                }));
        }

        /// <summary>
        /// 获取散图资源，无图集
        /// </summary>
        /// <param name="spriteName"></param>
        /// <param name="image"></param>
        public void SetOtherIcon(string spriteName, Image image)
        {

            Debug.Log(image.name);
            resourceComponent.LoadAsset(AssetUtility.GetOtherSpriteAsset(spriteName), typeof(Sprite), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    image.sprite = asset as Sprite;
                }));
        }

        /// <summary>
        /// 获取Common图集里的图片资源
        /// </summary>
        /// <param name="spriteName"></param>
        /// <returns></returns>
        public Sprite GetCommonIcon(string spriteName)
        {
            return atlasMap[str_Common].GetSprite(spriteName);
        }


    }
}
