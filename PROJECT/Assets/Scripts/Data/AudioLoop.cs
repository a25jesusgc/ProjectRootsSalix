using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLoop", menuName = "Custom/Crear loop de audio")]
public class AudioLoop : ScriptableObject
{
    [SerializeField] private AudioClip clip; // Audio clip
    [SerializeField] private int startLoopSample; // Sample where the loop starts
    [SerializeField] private int endLoopSample; // Sample where the loop should end
    [SerializeField] private float pitch = 1f; // Pitch for the audio

    public AudioClip GetAudioClip => clip;
    public int GetStartLoopSample => startLoopSample;
    public int GetEndLoopSample => endLoopSample;
    public float GetPitch => pitch;
}
