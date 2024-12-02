using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;



public class PlayerHealth : MonoBehaviour
{
   public float health;
   public float maxHealth;
   public Image healthBar;


    void Start()
    {
        maxHealth = health;
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
    }

}
