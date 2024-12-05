using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private Slider slider;

        [SerializeField] private Image dashImage;
        [SerializeField] private Image cloneImage;
        //[SerializeField] private Image crystalImage;
        [SerializeField] private Image swordImage;
        [SerializeField] private Image blackholeImage;
        [SerializeField] private Image flaskImage;

        [SerializeField] private TextMeshProUGUI currentSouls;
        private PlayerSkillManager skills;


        void Start()
        {
            if (playerStats != null)
                playerStats.onHealthChanged += UpdateHealthUI;

            skills = PlayerSkillManager.instance;
        }

    void Update()
    {
        //currentSouls.text = PlayerManager.instance.GetCurrency().ToString("#,#");

        //释放技能 更新技能CD UI
        if (Input.GetKeyDown(KeyCode.LeftShift) )//&& skills.dash.dashUnlocked)
            SetCooldownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.LeftShift) && cloneImage.fillAmount <= 0)// && skills.parry.parryUnlocked)
            SetCooldownOf(cloneImage);

        //if (Input.GetKeyDown(KeyCode.F))// && skills.crystal.crystalUnlocked)
        //    SetCooldownOf(crystalImage);

        if (Input.GetKeyDown(KeyCode.A))// && skills.sword.swordUnlocked)
            SetCooldownOf(swordImage);

        if (Input.GetKeyDown(KeyCode.H))// && skills.blackhole.blackHoleUnlocked)
            SetCooldownOf(blackholeImage);

        //更新技能CD UI
        CheckCoolDownOf(dashImage, skills.dash.cooldownTime);
        CheckCoolDownOf(cloneImage, skills.clone.cooldownTime);
        //CheckCoolDownOf(crystalImage, skills.clone.cooldownTime);
        CheckCoolDownOf(swordImage, skills.TS.cooldownTime);
        CheckCoolDownOf(blackholeImage, skills.BH.cooldownTime);



        if(Inventory.instance.GetEquipedEquipment(EquipmentType.Flask) != null)//检测是否装备了药水
        {
            ItemData_Equipment flask = Inventory.instance.GetEquipedEquipment(EquipmentType.Flask);

            if (Input.GetKeyDown(KeyCode.R))
                SetCooldownOf(flaskImage);

            CheckCoolDownOf(flaskImage, flask.CoolDownTime);
        }
        
    }


        private void UpdateHealthUI()//更新血条值
        {
            slider.maxValue = playerStats.GetMaxHealthValue();
            slider.value = playerStats.currentHP;
        }


        private void SetCooldownOf(Image _image)//设置技能冷却时间动画
        {
            if (_image.fillAmount <= 0)
                _image.fillAmount = 1;
        }

        private void CheckCoolDownOf(Image _image, float _coolDown)
        {
            if (_image.fillAmount > 0)
                _image.fillAmount -= 1 / _coolDown * Time.deltaTime;
        }

   
}
