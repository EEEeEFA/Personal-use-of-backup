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
        [SerializeField] private Image parryImage;
        [SerializeField] private Image crystalImage;
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
            currentSouls.text = PlayerManager.instance.CurrentCurrencyAmount().ToString("#,#");


            if (Input.GetKeyDown(KeyCode.LeftShift)) //&& skills.dash.dashUnlocked解锁技能检索
                SetCooldownOf(dashImage);

            if (Input.GetKeyDown(KeyCode.R))
                SetCooldownOf(blackholeImage);

            if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipedEquipment(EquipmentType.Flask) != null)//必须获取药水
                SetCooldownOf(flaskImage);



            CheckCoolDownOf(dashImage, skills.dash.cooldownTime);


            CheckCoolDownOf(flaskImage, Inventory.instance.GetEquipedEquipment(EquipmentType.Flask).CoolDownTime);
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
