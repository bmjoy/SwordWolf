using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Asakuma
{
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoLoader : MonoBehaviour
    {
        [SerializeField]
        private string relativePath;
        // Start is called before the first frame update
        void Start()
        {
            VideoPlayer player = GetComponent<VideoPlayer>();
            player.source = VideoSource.Url;
            player.url = Application.streamingAssetsPath + "/" + relativePath;
            player.prepareCompleted += PrepareCompleted;
            player.Prepare();
        }
        void PrepareCompleted(VideoPlayer vp)
        {
            vp.prepareCompleted -= PrepareCompleted;
            vp.Play();
        }
    }
}