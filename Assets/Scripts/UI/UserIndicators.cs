using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UserIndicators : MonoBehaviour
{
    public static UserIndicators S;
    
    private GameObject _player;

    [SerializeField]
    private Text _textHealth;
    [SerializeField]
    private Text _ShotsDone;
    [SerializeField]
    private Text _EnemyKills;
    [SerializeField]
    private Image _BoolletReflection;

    private CharacterHealth _heroHealth;
    
    private void Awake()
    {
        if (S == null)
            S = this;
        else
            Debug.Log("Singleton error!!!");
    }

    public void SubscribeOnPlayer(GameObject player)
    {
        _player = player;
        _heroHealth = _player.GetComponent<CharacterHealth>();
        _heroHealth.OnHealthChange += ShowHealth;
        ShowHealth();
    }
    
    public void UnSubscribeOnPlayer()
    {
        _heroHealth = _player.GetComponent<CharacterHealth>();
        _heroHealth.OnHealthChange -= ShowHealth;
        _textHealth.text = "Health: -- ";
        _player = null;
    }

    private void ShowHealth()
    {
        _textHealth.text = "Health: " + _heroHealth.Health;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (_heroHealth != null)
    //     {
    //         _textHealth.text = "Health: " + _heroHealth.Health;
    //     }
    //     if (GameManager.S.stat != null)
    //     {
    //         _ShotsDone.text = "Shots: " + GameManager.S.stat.shotCount;
    //         _EnemyKills.text = "Enemy kills: " + GameManager.S.stat.enemyKill;
    //         _BoolletReflection.enabled = GameManager.S.stat.hasBooletReflect;
    //     }
    // }

    void OnDestroy()
    {
        _heroHealth.OnHealthChange -= ShowHealth;
    }
}
