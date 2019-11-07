﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Walls : MonoBehaviour
{
    public int hp = 4;
    public Sprite damagedSprite;
    //lo usaremos para cambiar el sprite cuando el muro esté dañado
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int damage)
    {
        //tiene que cambiar el sprite por el sprite dañado
        //tiene que restar la vida la muro
        //tiene que destruir si la vida llega a 0

        spriteRenderer.sprite = damagedSprite;
        hp -= damage;
        if (hp < 1)
        {
            Destroy(gameObject);
        }
    }
}
