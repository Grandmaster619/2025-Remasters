using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector cutsceneDirector;
    private AudioSource src;

    void Start()
    {
        src = GetComponent<AudioSource>();
        //if player loads scene for first time
        if (PlayerPrefs.GetInt("SceneLoaded", 0) == 0)
        {

            //play cutscene
            cutsceneDirector.Play();
            PlayerPrefs.SetInt("SceneLoaded", 1);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cutsceneDirector.time = 1287f;
            if (src != null) { src.Stop(); }
        }
    }
}
