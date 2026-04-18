using UnityEngine;

public class PlayEnemyParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;

    public void PlayParticles(int index){
        particles[index].Play();
    }
}
