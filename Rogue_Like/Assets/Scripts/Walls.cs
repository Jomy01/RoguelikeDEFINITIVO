using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Walls : MonoBehaviour
{
    public int hp = 4;
    public Sprite damagedSprite;
    //lo usaremos para cambiar el sprite cuando el muro esté dañado
    SpriteRenderer spriteRenderer;
    private Audio audioController;
    public AudioClip[] chopClips;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioController = GameObject.Find("AudioController").GetComponent<Audio>();
    }
    //al llamar a esta función le indicaremos cuál es el valor de damage, por si es atacado por distitnos personajes, armas,...
    public void DamageWall(int damage)
    {
        //tiene que cambiar el sprite por el sprite dañado
        //tiene que restar la vida la muro
        //tiene que destruir si la vida llega a 0
        audioController.PlayRandomClip(chopClips);
        spriteRenderer.sprite = damagedSprite;
        hp -= damage;
        if (hp < 1)
        {
            Destroy(gameObject);
        }
    }
}
