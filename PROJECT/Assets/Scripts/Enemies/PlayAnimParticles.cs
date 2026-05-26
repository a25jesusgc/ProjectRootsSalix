using UnityEngine;

public class PlayAnimParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;

    public void PlayParticles(int index){
        particles[index].Play();
    }
}
