//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// Web 请求成功事件。
    /// </summary>
    public sealed class WebRequestSuccessEventArgs : GameEventArgs
    {
        private byte[] m_WebResponseBytes = null;

        /// <summary>
        /// Web 请求成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(WebRequestSuccessEventArgs).GetHashCode();

        /// <summary>
        /// 初始化 Web 请求成功事件的新实例。
        /// </summary>
        public WebRequestSuccessEventArgs()
        {
            SerialId = 0;
            WebRequestUri = null;
            m_WebResponseBytes = null;
            UserData = null;
        }

        /// <summary>
        /// 获取 Web 请求成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取 Web 请求任务的序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取 Web 请求地址。
        /// </summary>
        public string WebRequestUri
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取 Web 响应的数据流。
        /// </summary>
        /// <returns>Web 响应的数据流。</returns>
        public byte[] GetWebResponseBytes()
        {
            return m_WebResponseBytes;
        }

        /// <summary>
        /// 创建 Web 请求成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的 Web 请求成功事件。</returns>
        public static WebRequestSuccessEventArgs Create(GameFramework.WebRequest.WebRequestSuccessEventArgs e)
        {
            WWWFormInfo wwwFormInfo = (WWWFormInfo)e.UserData;
            WebRequestSuccessEventArgs webRequestSuccessEventArgs = ReferencePool.Acquire<WebRequestSuccessEventArgs>();
            webRequestSuccessEventArgs.SerialId = e.SerialId;
            webRequestSuccessEventArgs.WebRequestUri = e.WebRequestUri;
            webRequestSuccessEventArgs.m_WebResponseBytes = e.GetWebResponseBytes();
            webRequestSuccessEventArgs.UserData = wwwFormInfo.UserData;
            ReferencePool.Release(wwwFormInfo);
            return webRequestSuccessEventArgs;
        }

        /// <summary>
        /// 清理 Web 请求成功事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            WebRequestUri = null;
            m_WebResponseBytes = null;
            UserData = null;
        }

        //public void GetReturn()
        //{
        //    var bytes = GetWebResponseBytes();
        //    var head = bytes.ReadFrom(0, 2);
        //    var sign = bytes.ReadFrom(2, 2);

        //    int result = (bytes[4] & 0xff);

        //    if (result > 127)
        //    {
        //        result -= 256;
        //    }

        //    string errorMsg = "";
        //    if (result < 0)
        //    {
        //        if (result != -128)
        //        {
        //            errorMsg = System.Text.Encoding.UTF8.GetString(bytes, 5, bytes.Length - 5);
        //            Log.Error($"请求Http错误，错误信息：{errorMsg}");
        //        }
        //    }
        //    else
        //    {
        //        var opcodeTypeComponent = ETHotfix.Game.Scene.GetComponent<ETHotfix.OpcodeTypeComponent>();
        //        object instance = opcodeTypeComponent.GetInstanceBack(sign);
        //        var ob = GameEntry.ETNetwork.MsgTest(bytes, 5, bytes.Length - 5, instance);
        //        var data = ob as AccountLoginDown;
        //    }
        //}
    }
}
