using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoToCutscene : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public string nextSceneName;    // Name of the scene to load after the video

    private void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component is not assigned.");
            return;
        }

        // Subscribe to the VideoPlayer's loopPointReached event
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video ended. Loading next scene...");

        // Load the next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not specified.");
        }
    }

    private void OnDestroy()
    {
        // Clean up event subscription to prevent memory leaks
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}
