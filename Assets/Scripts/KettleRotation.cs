using UnityEngine;

public class KettleRotation : MonoBehaviour
{
    public GameObject objectB; 
    public Transform kettleTransform; 
    
    private ParticleSystem particleSystemB; 

    public Material Wobble;
    public Material WobbleTube;

    public string materialPropertyName = "_FillAmount";
    public string materialPropertyNameTube = "_FillAmounttube";

    void Start()
    {

        particleSystemB = objectB.GetComponent<ParticleSystem>();

        objectB.SetActive(false);
    }

    void Update()
    {
        var startlifetime = particleSystemB.main;

        if (kettleTransform.transform.position.y > 1)
        {
            startlifetime.startLifetime = 0.2f + (kettleTransform.transform.position.y - 1) * 1F;
        }

        if (particleSystemB == null) return;
        {

        }

        float rotationZ = kettleTransform.rotation.eulerAngles.z;


        if (rotationZ > 180)
        {
            rotationZ -= 360;
        }


        bool shouldActivate = rotationZ > -200 && rotationZ <= -60;

        if (shouldActivate)
        {
            if (!objectB.activeSelf)
            {
                objectB.SetActive(true);

                Debug.Log("active");
            }

            if (!particleSystemB.isPlaying)
            {
                particleSystemB.Play();
            }

            float t = Mathf.InverseLerp(-200, -60, rotationZ);
            float tt = Mathf.InverseLerp(-200, -60, rotationZ);

            float newValue = Mathf.Lerp(0.022f, 0f, t);
            float newTubeValue = Mathf.Lerp(-0.066f, 0.066f, tt);

            Wobble.SetFloat(materialPropertyName, newValue);
            WobbleTube.SetFloat(materialPropertyNameTube, newTubeValue);
        }

        else
        {
            if (objectB.activeSelf)
            {
                if (particleSystemB.isPlaying)
                {
                    particleSystemB.Stop(); 
                }

                objectB.SetActive(false);
            }

            Wobble.SetFloat(materialPropertyName, 0.022f);
            WobbleTube.SetFloat(materialPropertyNameTube, -0.066f);
        }
    }
}
