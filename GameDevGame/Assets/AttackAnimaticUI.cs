using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttackAnimaticUI : MonoBehaviour
{
    public Image attackerImage;
    public Image victimImage;

    private void Start()
    {
        //gameObject.SetActive(false); 
    }

    public void PlayAnimatic(Sprite attacker, Sprite victim)
    {
        Debug.Log("PlayAnimatic called");

        attackerImage.sprite = attacker;
        victimImage.sprite = victim;

        gameObject.SetActive(true); 

        StartCoroutine(HideAfterDelay(1.5f)); 
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Hiding animatic panel after delay");
        gameObject.SetActive(false);
    }
}