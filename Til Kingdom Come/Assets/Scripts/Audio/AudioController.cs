using UnityEngine;

public class AudioController : MonoBehaviour
{
    public void Play(string name) {
        AudioManager.instance.Play(name);
    }
}
