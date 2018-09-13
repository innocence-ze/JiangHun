using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonSoundEffect : MonoBehaviour {

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Music\\button"), Camera.main.transform.position, 0.2f);
        });
    }
}
