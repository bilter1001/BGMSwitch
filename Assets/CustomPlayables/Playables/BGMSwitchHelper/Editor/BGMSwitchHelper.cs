using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace BGMSwitchHelper
{
    class BGMSwitchHelper : EditorWindow
    {

        [Serializable]
        public class BGMConfig
        {
            [Serializable]
            public class Item
            {               
                public string id;
                public string name;
            }

            public class Topic
            {
                public string topicname;
                public List<Item> items;
            }
            public List<Topic> topics = new List<Topic>();
        }

        protected static string configFile = "Assets/StreamingAssets/Config/BGMConfig.json";
        public static BGMConfig config = new BGMConfig();

        public static PlayableDirector bgmPlayableDirector;
        public static GameObject BMGMgrGo;
        static string playableDir = "Assets/BGMPlayable/";
        static string playableName = "BgmSetting.playable";

        [MenuItem("自动配乐/设置")]
        static void CreateVLGameObject()
        {
            BMGMgrGo = GameObject.Find("BMGMgrGo");
            if (BMGMgrGo == null)
            {
                BMGMgrGo = new GameObject("BMGMgrGo");
                bgmPlayableDirector = BMGMgrGo.AddComponent<PlayableDirector>();
                BMGMgrGo.AddComponent<AudioSource>();
            }
            else
            {
                bgmPlayableDirector = BMGMgrGo.GetComponent<PlayableDirector>();
            }
            if (!Directory.Exists(playableDir))
            {
                Directory.CreateDirectory(playableDir);
            }
            string playablePath = playableDir + playableName;

            if (!File.Exists(playablePath))
            {
                var asset = TimelineAsset.CreateInstance<TimelineAsset>();
                AssetDatabase.CreateAsset(asset, playablePath);
                var track = asset.CreateTrack<AudioTrack>(null, "BgmTrack");
                AssetDatabase.SaveAssets();
                bgmPlayableDirector.playableAsset = asset;
            }
            else
            {
                TimelineAsset asset = AssetDatabase.LoadAssetAtPath<TimelineAsset>(playablePath);
                bgmPlayableDirector.playableAsset = asset;
            }



            string json = File.ReadAllText(configFile);
            config = JsonConvert.DeserializeObject<BGMConfig>(json);

            BMGSwitchWindow _window = EditorWindow.GetWindow<BMGSwitchWindow>(); // 创建自定义窗体
            _window.titleContent = new GUIContent("自动配乐"); // 窗口的标题
            _window.minSize = new Vector2(250, 400);
            _window.Show();
        }




    }
}
