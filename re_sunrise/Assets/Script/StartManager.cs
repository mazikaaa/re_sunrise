using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public GameObject audio;
    AudioSource audiosource;
    public AudioClip button_SE;
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        Instantiate(audio);
        PlayerPrefs.SetInt("Audio", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoSelectButton()
    {
        audiosource.PlayOneShot(button_SE);
        Invoke("Select", 0.3f);
    }

    public void Select()
    {
        SceneManager.LoadScene("selectscene");
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Audio", 0);
    }
}
