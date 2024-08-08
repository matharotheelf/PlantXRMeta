using System.Collections;
using UnityEngine;

public class SeedDisappera : MonoBehaviour
{
    public MonoBehaviour script1;
    public MonoBehaviour script2;

    public Material materialA;
    public Material materialB;
    private Renderer objectRenderer;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material = materialA;

        objectRenderer.material.SetFloat("_Dissolve", 0);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Edge"))
        {

            script1.enabled = true;
            script2.enabled = true;


            StartCoroutine(TransitionMaterialAndDissolve());
        }
    }

    private IEnumerator TransitionMaterialAndDissolve()
    {
        objectRenderer.material = materialB;

        float duration = 1.0f;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float dissolveValue = Mathf.Lerp(0, 1, elapsedTime / duration);
            objectRenderer.material.SetFloat("_Dissolve", dissolveValue);
            yield return null;
        }

        objectRenderer.material.SetFloat("_Dissolve", 1);
    }
}
