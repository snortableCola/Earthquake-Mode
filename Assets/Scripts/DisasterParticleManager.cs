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

    //tornado particle 


   
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
