using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public float speed = 10;

    public float hp = 150;
    public Slider hpSlider;
    private float totalHp;

    public GameObject explosionEffect;

    private Transform[] positions;

    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        positions = WayPoints.positions;
        totalHp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //敌人移动
    void Move()
    {
        if (index > positions.Length - 1)
        {
            return;
        }

        transform.Translate((positions[index].position - transform.position).normalized * Time.deltaTime * speed);

        if (Vector3.Distance(positions[index].position, transform.position) < 0.2f)
        {
            index++;
        }

        if (index > positions.Length - 1)
        {
            ReachDestination();
        }
    }

    //抵达终点
    void ReachDestination()
    {
        GameManager.Instance.Failed();
        GameObject.Destroy(this.gameObject);
    }

    //敌人销毁
    void OnDestroy()
    {
        EnemySpawner.countEnemyAlive--;
    }

    //敌人受到伤害
    public void TakeDamage(float damage)
    {
        if (hp <= 0)
        {
            return;
        }
        hp -= damage;
        hpSlider.value = (float)hp / totalHp;
        if (hp <= 0)
        {
            Die();
        }
    }
    //敌人销毁
    void Die()
    {
        GameObject effect = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effect, 1.5f);
        Destroy(this.gameObject);
    }
}
 