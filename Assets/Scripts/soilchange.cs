using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soilchange : MonoBehaviour
{
    public float timer;

    public float timergrowth;

    public Transform GreenButton;
    public MeshRenderer soil1;
    public MeshRenderer grass;

    bool particletrigger;

    void Start()
    {

    }
    void Update()
    {
        Debug.Log(GreenButton.localPosition.z);

        if (GreenButton.localPosition.z == 0.018f)
        {
            timergrowth -= Time.deltaTime; timer -= Time.deltaTime * (1f / 3);
            timergrowth = Mathf.Clamp(timergrowth, 0f, 3f);
            timer = Mathf.Clamp(timer, 0f, 1f);

            if (timergrowth >= 2f)
            {
                grass.material.SetFloat("_WidthRandom", (timergrowth - 2) * 0.02f);

                //need value determination

            }
            else
            {
                timergrowth = 0f;
                grass.material.SetFloat("_WidthRandom", 0);
            }

            soil1.material.SetFloat("_Metallic", timer);

            particletrigger = false;
        }
        
        if (particletrigger)
        {
            timergrowth += Time.deltaTime;
            timergrowth = Mathf.Clamp(timergrowth, 0f, 3f);
            if (timergrowth >= 2f)
            {
                grass.material.SetFloat("_WidthRandom", (timergrowth - 2) * 0.02f);
            }
        }
    }
    void OnParticleCollision(GameObject other)
    {
        timer += Time.deltaTime * (1f / 3);
        timer = Mathf.Clamp(timer, 0f, 1f);

        soil1.material.SetFloat("_Metallic", timer);
        

        particletrigger = true;

        
        Debug.Log("hit");
    }

    
}
