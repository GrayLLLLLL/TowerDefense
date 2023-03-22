using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    private List<GameObject> enemys = new List<GameObject>();

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            enemys.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy")
        {
            enemys.Remove(col.gameObject);
        }
    }

    public float attackRateTime = 1; //多少秒攻击一次
    private float timer = 0;

    public GameObject bulletPrefab; //子弹
    public Transform firePosition;
    public Transform head;

    //判断是否是激光炮塔
    public bool useLaser = false;
    public float damageRate = 30; //激光伤害速率
    public LineRenderer laserRenderer;
    public GameObject laserEffect;

    void Start()
    {
        timer = attackRateTime;
    }

    void Update()
    {
        //控制炮口朝向
        if (enemys.Count > 0 && enemys[0] != null)
        {
            Vector3 targetPosition = enemys[0].transform.position;
            targetPosition.y = head.position.y;
            head.LookAt(targetPosition);
        }
        //子弹类所用计时器
        if (useLaser == false)
        {
            timer += Time.deltaTime;
            if (enemys.Count > 0 && timer >= attackRateTime)
            {
                timer = 0;
                Attack();
            }
        }
        //否则为激光攻击
        else if (enemys.Count > 0)
        {
            if (laserRenderer.enabled == false)
            {
                laserRenderer.enabled = true;
            }
            laserEffect.SetActive(true);
            if (enemys[0] == null)
            {
                UpdateEnemys();
            }
            if (enemys.Count > 0)
            {
                laserRenderer.SetPositions(new Vector3[]{firePosition.position, enemys[0].transform.position});
                enemys[0].GetComponent<Enemy>().TakeDamage(damageRate * Time.deltaTime);
                laserEffect.transform.position = enemys[0].transform.position;
            }
        }
        //激光不攻击
        else
        {
            laserEffect.SetActive(false);
            laserRenderer.enabled = false;
        }
    }

    //子弹类攻击方法
    void Attack()
    {
        if (enemys[0] == null)
        {
            UpdateEnemys();
        }

        if (enemys.Count > 0)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            bullet.GetComponent<Bullet>().SetTarget(enemys[0].transform);
        }
        else
        {
            timer = attackRateTime;
        }
    }

    //更新敌人
    void UpdateEnemys()
    {
        List<int> emptyIndex = new List<int>();
        for (int index = 0; index < enemys.Count; index++)
        {
            if (enemys[index] == null)
            {
                emptyIndex.Add(index);
            }
        }

        for (int i = 0; i < emptyIndex.Count; i++)
        {
            enemys.RemoveAt(emptyIndex[i] - i);
        }
    }

}
