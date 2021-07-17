using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    AudioClip SelectLevelAudio;

    public void StartLevel(int sceneID) 
    {
        SceneManager.LoadScene(sceneID);
        AudioSource.PlayClipAtPoint(SelectLevelAudio, Camera.main.transform.position, 0.75f);
    }
}
