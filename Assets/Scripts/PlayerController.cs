﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : WorldObject
{
    [Serializable]
    private struct PlayerInfo
    {
        public int locx;
        public int locy;
        public float health;
        public float hunger;
    }

    public Animator animator;

    public float speed = 5;

    public float max_health = 100;
    public float max_hunger = 100;

    public UiBar health_bar;
    public UiBar hunger_bar;
    float health;
    float hunger;
    float hunger_speed = 0.1f;

    Rigidbody2D rigidbody2d;
    float horizontal = 0;
    float vertical = 0;
    float look_angle = 0;

    int row = 0;
    int column = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        health = max_health;
        hunger = max_hunger;

        health_bar = GameObject.Find("Hungerbar").GetComponent<UiBar>();
        hunger_bar = GameObject.Find("Healthbar").GetComponent<UiBar>();
    }

    // Update is called once per frame
    void Update()
    {
        CalcGridPosition();

        if (hunger > 0)
        {
            hunger -= hunger_speed * Time.deltaTime;
            hunger_bar.updateValue(hunger);
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        animator.SetFloat("Speed", Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float x1 = mousePosition.x;
        float x2 = rigidbody2d.position.x;
        float y1 = mousePosition.y;
        float y2 = rigidbody2d.position.y;
        float xDiff = x1 - x2;
        float yDiff = y1 - y2;
        look_angle = (float)(Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg) + 90;
    }

    void FixedUpdate()
    {
        Vector2 move = new Vector2(horizontal, vertical);
        Vector2 new_position = rigidbody2d.position + move * speed * Time.fixedDeltaTime;


        rigidbody2d.MovePosition(new_position);
        rigidbody2d.MoveRotation(look_angle);
        //rigidbody2d.transform.center
        //transform.position = new_position;
        //transform.rotation = Quaternion.Euler(0, 0, look_angle);
    }

    void CalcGridPosition()
    {
        // loops through the surrounding grid locations and checks to see which one is closest and updates row and column
        float closest_dist = 1000;
        int new_row=0;
        int new_col=0;
        for(int i=-1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3 cell_loc = GameInfo.Instance.GetCellLocation(row+i, column+j);
                float dist = Vector3.Distance(cell_loc, transform.position);
                if (dist < closest_dist)
                {
                    closest_dist = dist;
                    new_row = row + i;
                    new_col = column + j;
                }
            }
        }
        row = new_row;
        column = new_col;

        //Debug.Log(new_row + ", " + new_col);
    }

    public override WorldObject ObjectFromString(string json)
    {
        PlayerInfo info = JsonUtility.FromJson<PlayerInfo>(json);
        health = info.health;
        hunger = info.hunger;

        return this;
    }

    public override string ObjectToString()
    {
        PlayerInfo info;
        info.locx = GetLocationX();
        info.locy = GetLocationY();
        info.health = health;
        info.hunger = hunger;

        return JsonUtility.ToJson(info);
    }
}
