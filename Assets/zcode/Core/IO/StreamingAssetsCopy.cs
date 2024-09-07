/***************************************************************
* Author: Zhang Minglin
* Note  : Application.streamingAssetsPath目录下拷贝
***************************************************************/
using System.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

namespace zcode
{
    /// <summary>
    /// Application.streamingAssetsPath目录下拷贝
    /// </summary>
    public class StreamingAssetsCopy
    {
        /// <summary>
        /// 是否结束
        /// </summary>
        public bool isDone { get; private set; }

        /// <summary>
        /// 拷贝结果
        /// </summary>
        public emIOOperateCode resultCode { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string error { get; private set; }

        /// <summary>
        ///   从Application.streamingAssetsPath目录下拷贝
        /// </summary>
        public IEnumerator Copy(string src, string dest)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_IPHONE
            src = "file:///" + src;
#endif
            SetResult(false, emIOOperateCode.Succeed, null);
            do
            {
                using (UnityWebRequest request = UnityWebRequest.Get(src))
                {
                    yield return request.SendWebRequest();
                    if (!string.IsNullOrEmpty(request.error))
                    {
                        SetResult(true, emIOOperateCode.Fail, request.error);
                    }
                    else
                    {
                        if (request.isDone && request.downloadHandler.data.Length > 0)
                        {
                            var ret = zcode.FileHelper.WriteBytesToFile(dest, request.downloadHandler.data, request.downloadHandler.data.Length);
                            SetResult(true, ret, null);
                        }
                    }
                }
            } while (!isDone);
        }

        /// <summary>
        /// 
        /// </summary>
        void SetResult(bool isDone, emIOOperateCode result, string error)
        {
            this.isDone = isDone;
            this.resultCode = result;
            this.error = error;
        }
    }
}