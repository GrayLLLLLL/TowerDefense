using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{

    public TurretData laserTurretData;
    public TurretData missileTurretData;
    public TurretData standardTurretData;

    //表示当前选择的炮台（将要建造的炮台）
    private TurretData selectedTurretData;

    //表示当前选择的炮台（场景中的游戏物体）
    private MapCube selectedMapCube;

    private int money = 1000;
    public Text moneyText;
    public Animator moneyAnimator;

    public GameObject upgradeCanvas;
    public Button upgradeButton;
    private Animator upgradeCanvasAnimator;

    //更改金钱
    void ChangeMoney(int change = 0)
    {
        money += change;
        moneyText.text = "$ - " + money;
    }

    void Start()
    {
        upgradeCanvasAnimator = upgradeCanvas.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false) //判断指针是否点在了UI上
            {
                //判断指针和mapCube做碰撞并且检测是否为空
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube"));
                if (isCollider)
                {
                    MapCube mapCube = hit.collider.GetComponent<MapCube>(); //得到点击的mapCube
                    if (selectedTurretData != null && mapCube.turretGo == null)
                    {
                        //可以创建
                        if (money > selectedTurretData.cost)
                        {
                            //金钱足够，扣钱，交给MapCube创建炮台
                            ChangeMoney(-selectedTurretData.cost);
                            mapCube.BuildTurret(selectedTurretData);
                        }
                        else
                        {
                            //提示金钱不足
                            moneyAnimator.SetTrigger("Flicker");
                        }
                    }
                    else if (mapCube.turretGo != null)
                    {
                        //升级面板
                        if (mapCube == selectedMapCube && upgradeCanvas.activeInHierarchy)
                        {
                            StartCoroutine(HideUpgradeUI());
                        }
                        else
                        {
                            ShowUpgradeUI(mapCube.transform.position, mapCube.isUpgraded);
                        }
                        selectedMapCube = mapCube;
                    }
                }
            }
        }
    }

    //选择炮塔
    public void OnLaserSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = laserTurretData;
        }
    }
    public void OnMissileSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = missileTurretData;
        }
    }
    public void OnStandardSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = standardTurretData;
        }
    }

    //展示和隐藏升级UI
    void ShowUpgradeUI(Vector3 pos, bool isDisableUpgrade = false)
    {
        StopCoroutine("HideUpgradeUI");
        upgradeCanvas.SetActive(false);
        upgradeCanvas.SetActive(true);
        upgradeCanvas.transform.position = pos;
        upgradeButton.interactable = !isDisableUpgrade;
    }
    IEnumerator HideUpgradeUI()
    {
        upgradeCanvasAnimator.SetTrigger("Hide");
        yield return new WaitForSeconds(0.8f);
        upgradeCanvas.SetActive(false);
    }

    //点击两个按钮的事件
    public void OnUpgradeButtonDown()
    {
        if (money >= selectedMapCube.turretData.costUpgraded)
        {
            ChangeMoney(-selectedMapCube.turretData.costUpgraded);
            selectedMapCube.UpgradeTurret();
        }
        else
        {
            moneyAnimator.SetTrigger("Flicker");
        }
        StartCoroutine(HideUpgradeUI());
    }
    public void OnDestroyButtonDown()
    {
        selectedMapCube.DestroyTurret();
        StartCoroutine(HideUpgradeUI());
    }
}
