using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour
{

    [HideInInspector]
    public GameObject turretGo; //保存当前cube身上的炮台
    [HideInInspector]
    public TurretData turretData;

    public GameObject buildEffect;

    [HideInInspector]
    public bool isUpgraded = false;

    //修建炮塔
    public void BuildTurret(TurretData turretData)
    {
        this.turretData = turretData;
        isUpgraded = false;
        turretGo = GameObject.Instantiate(turretData.turretPrefab, transform.position, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }

    //升级和拆卸
    public void UpgradeTurret()
    {
        if (isUpgraded == true)
        {
            return;
        }
        Destroy(turretGo);
        isUpgraded = true;
        turretGo = GameObject.Instantiate(turretData.turretUpgradedPrefab, transform.position, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
    public void DestroyTurret()
    {
        Destroy(turretGo);
        isUpgraded = false;
        turretGo = null;
        turretData = null;
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }

    //鼠标移入和移出
    void OnMouseEnter()
    {
        if (turretGo == null && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.color = Color.green;
        }
    }
    void OnMouseExit()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = Color.white;
    }
}
