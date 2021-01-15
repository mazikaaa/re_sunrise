using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Audio : MonoBehaviour
{
    protected List<string> scenename;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        scenename = new List<string>()
        {
            "startScene",
            "selectscene",
        };
    }

    // Update is called once per frame
    void Update()
    {
        foreach (string name in scenename)
        {
            if (SceneManager.GetActiveScene().name == name)
                return;
        }
        Destroy(this.gameObject);
    }
}
