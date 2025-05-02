using UnityEngine;
using MilkShake;
using System.Collections.Generic;
using System.Collections;

public class DisasterParticleManager : MonoBehaviour
{
    public static DisasterParticleManager Instance;
    //gameobjects for the camera shaker 
    public Shaker CameraShaker;
    public ShakePreset ShakerPreset;
    private ShakeInstance shakeInstance;
    private Dictionary<Disaster, ParticleSystem> _particleEffects; 
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
        // Ensure particle effects are stopped at the start
        if (RainEffect != null)
        {
            RainEffect.Stop();
            RainEffect.gameObject.SetActive(false); // Set inactive
        }

        if (tornadoEffect != null)
        {
            tornadoEffect.Stop();
            tornadoEffect.gameObject.SetActive(false); // Set inactive
        }
        _particleEffects = new Dictionary<Disaster, ParticleSystem>();
        shakeInstance = Shaker.ShakeAll(ShakerPreset); 
    }
    public static DisasterParticleManager GetInstance()
    {
        return Instance;
    }
    public void RegisterEffect(Disaster disaster, ParticleSystem effect)
    {
        if (!_particleEffects.ContainsKey(disaster))
        {
            _particleEffects.Add(disaster, effect);
        }
    }
    public void HandleEffect(string disasterName, bool isStarting, float duration = 0f)
    {
        if (disasterName == "Tsunami" && RainEffect != null)
        {
            if (isStarting)
            {
                RainEffect.Play();
                Debug.Log("Playing rain effect for Tsunami.");
                if (duration > 0)
                {
                    StartCoroutine(StopEffectAfterDelay(RainEffect, duration));
                }
            }
            else
            {
                RainEffect.Stop();
                Debug.Log("Stopping rain effect for Tsunami.");
            }
        }
        if (disasterName == "Tornado" && tornadoEffect != null)
        {
            if (isStarting)
            {
                tornadoEffect.Play();
                Debug.Log("Playing tornado effect");
                if (duration > 0)
                {
                    StartCoroutine(StopEffectAfterDelay(RainEffect, duration));
                }

            }
            else
            {
                tornadoEffect.Stop();
            
            }
        
        
        }
    }
    private IEnumerator StopEffectAfterDelay(ParticleSystem effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        effect.Stop();
        Debug.Log("Effect stopped automatically after delay.");
    }
    public void EarthquakeShake()
    {
        shakeInstance.Start(2f);
        shakeInstance.Stop(2f,false);

    }
}
