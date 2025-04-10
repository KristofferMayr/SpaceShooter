/*
Not yet used -> usage later in bossfight development
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    [SerializeField] private Slider _healthBar;
 
    private int _healthMax;
 
    private void Start()
    {
        SetHealth(_health);
    }
 
    public void TakeDamage(int damage)
    {
        if (_health > 0)
        {
            _health -= damage;
            _healthBar.value -= (float)10 / Mathf.Pow(10, _healthMax.ToString().Length - 1);
        }
 
        if (_health <= 0)
        {
            Death();
        }
 
        ChangeHealthBarColor();
    }
 
    public void SetHealth(int health)
    {
        _healthBar.value = health;
         
        if(_health > _healthMax)
        {
            _health = _healthMax;
        }
 
        ChangeHealthBarColor();
    }
 
    private void Death()
    {
        Destroy(this.gameObject);
        Debug.Log("Dead");
    }
 
    private void ChangeHealthBarColor()
    {
        Color color;
 
        if (_healthBar.value > 0.5)
        {
            color = Color.green;
        }
        else if ((_healthBar.value > 0.25f) && (_healthBar.value <= 0.5f))
        {
            color = Color.yellow;
        }
        else
        {
            color = Color.red;
        }
 
        _healthBar.fillRect.GetComponent<Image>().color = color;
    }
}