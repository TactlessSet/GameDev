using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttackAnimaticUI : MonoBehaviour
{
    public Image attackerImage;
    public Image victimImage;
    public GameObject attackPanel;
    public static AttackAnimaticUI Instance { get; private set; }
    

    private void Awake()
    {
        Debug.Log("AttackAnimaticUI Awake called");
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        attackPanel.SetActive(false); //kind of redundant but whatevs
    }


    private void Start()
    {
 
    }


    public void PlayAnimatic(Sprite attacker, Sprite victim)
    {
        Debug.Log("PlayAnimatic called");

       
        attackerImage.sprite = attacker;
        victimImage.sprite = victim;

        // Debug log to check if sprites are valid
        if (attacker == null || victim == null)
        {
            Debug.LogError("Attacker or victim sprite is null!");
        }

        attackPanel.SetActive(true);

        StartCoroutine(HideAfterDelay(1.5f)); 
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        attackPanel.SetActive(false);
    }
}
