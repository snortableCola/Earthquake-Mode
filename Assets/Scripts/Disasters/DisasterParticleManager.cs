using UnityEngine;
using MilkShake;

public class DisasterParticleManager : MonoBehaviour
{
    private static DisasterParticleManager Instance;
    //gameobjects for the camera shaker 
    public Shaker CameraShaker;
    public ShakePreset ShakerPreset;
    private ShakeInstance shakeInstance;

    //rainfall particle 
  [SerializeField]  private ParticleSystem RainEffect;

    //tornado particle 
   [SerializeField] private ParticleSystem tornadoEffect; 

   
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

        }
        else
        { 
         
            Instance = this;    
        }
        shakeInstance = Shaker.ShakeAll(ShakerPreset); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDisasterParticles()
    { 
    
    
    }
}
