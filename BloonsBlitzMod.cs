using MelonLoader;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Il2CppSystem.Linq;
using Il2Cpp;
using Il2CppSystem.Collections.Generic;
using Il2CppNinjaKiwi.Localization;
using Il2CppSystem.IO;



namespace BloonsBlitzMod
{
    public class Main : MelonMod
    {
        private static int monkeySlots = 3;


        [HarmonyPatch(typeof(App), nameof(App.Update))]
        public class App_Awake_Patch
        {
            public static void Postfix(App __instance)
            {

                if (__instance == null || __instance.charactersData == null)
                {
                    return;
                }
                var monkeys = __instance.charactersData.monkeys;
                var heroes = __instance.charactersData.heroes;

                if (monkeys == null || monkeys.Length == 0 || heroes == null || heroes.Length == 0)
                {
                    return;
                }
                foreach (var monkey in monkeys)
                {
                    foreach (var upgrade in monkey.upgrades)
                    {
                        if (upgrade.unlockWeaponStats != null)
                        {
                            upgrade.unlockWeaponStats.attackSpeed = 100f;
                        }
                    }
                    foreach (var weapon in monkey.weaponStats)
                    {
                        weapon.attackSpeed = 100f;
                    }
                }
                foreach (var hero in heroes)
                {
                    foreach (var upgrade in hero.upgrades)
                    {
                        if (upgrade.unlockWeaponStats != null)
                        {
                            upgrade.unlockWeaponStats.attackSpeed = 100f;
                        }
                    }
                    foreach (var weapon in hero.weaponStats)
                    {
                        weapon.attackSpeed = 100f;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(MonkeysMenu), nameof(MonkeysMenu.Awake))]
        public class MonkeysMenu_SliderPatch
        {
            private static GameObject sliderGO = null;

            static void Postfix(MonkeysMenu __instance)
            {
                if (__instance == null) return;

                if (sliderGO != null) return;

                Transform menuParent = __instance.transform;

                sliderGO = new GameObject("MonkeySlotsSlider");
                sliderGO.transform.SetParent(menuParent, false);

                var mainRect = sliderGO.AddComponent<RectTransform>();
                mainRect.sizeDelta = new Vector2(750, 50);

                mainRect.anchoredPosition = new Vector2(0, 500);

                GameObject bgTrack = new GameObject("BackgroundBar");
                bgTrack.transform.SetParent(sliderGO.transform, false);

                var bgImage = bgTrack.AddComponent<Image>();
                bgImage.color = new Color(0.12f, 0.12f, 0.12f, 0.9f);

                var bgRect = bgTrack.GetComponent<RectTransform>();
                bgRect.anchorMin = new Vector2(0, 0);
                bgRect.anchorMax = new Vector2(1, 1);
                bgRect.sizeDelta = Vector2.zero;

                GameObject handleKnob = new GameObject("HandleIndicator");
                handleKnob.transform.SetParent(sliderGO.transform, false);

                var handleImage = handleKnob.AddComponent<Image>();
                handleImage.color = new Color(0.1f, 0.85f, 0.3f, 1f);

                var handleRect = handleKnob.GetComponent<RectTransform>();
                handleRect.sizeDelta = new Vector2(26, 26);

                var visualSlider = sliderGO.AddComponent<Slider>();
                visualSlider.minValue = 0;
                visualSlider.maxValue = 15;
                visualSlider.value = 3;
                visualSlider.wholeNumbers = true;

                visualSlider.targetGraphic = handleImage;
                visualSlider.handleRect = handleRect;
                visualSlider.direction = Slider.Direction.LeftToRight;

                visualSlider.onValueChanged.AddListener(new System.Action<float>((targetValue) => OnSliderValueChanged(targetValue)));

                sliderGO.SetActive(true);
            }
        }

        private static void OnSliderValueChanged(float val)
        {
            monkeySlots = (int)val;
        }

        [HarmonyPatch(typeof(StatsManager), nameof(StatsManager.GameTick))]
        public class StatsManager_Instantiate_Patch
        {
            public static void Postfix(StatsManager __instance)
            {
                __instance.numberOfMonkeySlots = monkeySlots;
            }
        }

        /* [HarmonyPatch(typeof(PlayerProfile), nameof(PlayerProfile.TrySpendMonkeyMoney))]
         public class PlayerProfile_GameTick_Patch
         {
             public static void Postfix(PlayerProfile __instance)
             {
                 foreach (var monkey in __instance.monkeys)
                 {
                     if (monkey.id == "monkey.boomerang")
                     {
                         monkey.powerLevel = 20;
                         monkey.level = 20;
                         monkey.powerLevel = 20;
                     }

                     monkey.currentXP = 0;
                 }

                 foreach (var toy in __instance.toys)
                 {
                     toy.count = 1;
                 }
                 __instance.currencies.bloonStones = 999999;
                 __instance.currencies.lives = 99;
                 __instance.currencies.monkeyMoney = 999999;
             }
         }*/
    }
}