using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace BGMSwitchHelper
{
    class BMGSwitchWindow : EditorWindow
    {


        private void OnGUI()
        {
            GUILayout.BeginVertical();
            //
            GUILayout.BeginHorizontal();
            //区域B 展示区域
            int x = 0;
            int y = 0;
            int size = 60;
            int padding = 10;
            int maxCount = 600 / (size + padding);
            int count = 0;
            Rect rect = Rect.zero;
            Rect labelRect = Rect.zero;
            GUILayout.BeginArea(new Rect(padding, padding, 600, 600));
            GUILayout.BeginScrollView(new Vector2(300, 0),
                GUILayout.Width(600), GUILayout.ExpandHeight(true));

            var nodeInfos = BGMSwitchHelper.config.topics;
            if (nodeInfos != null)
            {
                foreach (var nodeInfo in nodeInfos)
                {
                    if (nodeInfo == null)
                    {
                        return;
                    }
                    rect = new Rect(x, y, size, size);
                    string topicname = nodeInfo.topicname;
                    if (GUI.Button(rect, topicname))
                    {
                        System.Random random = new System.Random();
                        int rat = random.Next(nodeInfo.items.Count);
                        Debug.Log("rat: " + rat);
                        string id = nodeInfo.items[rat].id;
                        AudioClip audioclip = Resources.Load<AudioClip>(id);

                        if (audioclip == null)
                        {
                            Debug.LogError("audioclip is null. path: " + id);
                        }
                        else
                        {
                            TimelineAsset timelineAsset = BGMSwitchHelper.bgmPlayableDirector.playableAsset as TimelineAsset;
                            List<TrackAsset> lstTrackAsset = timelineAsset.GetRootTracks().ToList<TrackAsset>();
                            for (int i = 0; i < lstTrackAsset.Count; i++)
                            {
                                TrackAsset trackAsset = timelineAsset.GetRootTrack(i);
                                timelineAsset.DeleteTrack(trackAsset);
                            }
                            var track = timelineAsset.CreateTrack<AudioTrack>(null, "bgmTrack");
                            BGMSwitchHelper.bgmPlayableDirector.SetGenericBinding(track, BGMSwitchHelper.BMGMgrGo);


                            var clip = track.CreateClip(audioclip);
                            clip.displayName = audioclip.name;
                            AudioPlayableAsset clipasset = clip.asset as AudioPlayableAsset;
                            clipasset.loop = true;
                            AssetDatabase.SaveAssets();
                        }  

                        Close();
                    }

                    x += size + padding;
                    count++;
                    if ((count + 1) % maxCount == 0)
                    {
                        x = 0;
                        y += size + padding;
                    }
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
            GUILayout.EndHorizontal();

//             if (GUILayout.Button("Generate"))
//             {
// 
//             }


            GUILayout.EndVertical();
        }

    }
}
