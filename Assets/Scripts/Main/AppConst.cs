using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppConst : MonoBehaviour
{
    public static string enemyConfigXMLUrl = Application.dataPath + "/XML/EnemyInfo.xml";
    public static string towerConfigXMLUrl = Application.dataPath + "/XML/TowerInfo.xml";
    //Prefab
    public static string Archer_Arrow_Prefab = "Prefab/Tower/Bullets/Archer_Arrow";
    public static string Canon_Lvl1_Ball_Prefab = "Prefab/Tower/Bullets/Canon_Lvl1_Ball";
    public static string Canon_Lvl2_Ball_Prefab = "Prefab/Tower/Bullets/Canon_Lvl2_Ball";
    public static string Canon_Lvl3_Rocket_Prefab = "Prefab/Tower/Bullets/Canon_Lvl3_Rocket";
    public static string Canon_Lvl4A_Rocket_Prefab = "Prefab/Tower/Bullets/Canon_Lvl4A_Rocket";
    public static string Canon_Lvl4B_Rocket_Prefab = "Prefab/Tower/Bullets/Canon_Lvl4B_Rocket";
    public static string SimpleLightning_Prefab = "Prefab/Tower/Bullets/SimpleLightning";
    public static string IntensiveLightning_Prefab = "Prefab/Tower/Bullets/IntensiveLightning";
    public static string CannonArrow_Prefab = "Prefab/Tower/Bullets/CannonArrow";
    public static string MagicArrowBall_Prefab = "Prefab/Tower/Bullets/MagicArrowBall";
    public static string MagicCannonBall_Prefab = "Prefab/Tower/Bullets/MagicCannonBall";

    public static string United_Bow = "Prefab/Tower/Bow";
    public static string United_Canon = "Prefab/Tower/Canon";
    public static string United_Magic = "Prefab/Tower/Magic";

    public static string Record_Text = "Prefab/Tower/RecordText";
    //UI_Gadget
    public static string ChooseBtn_Prefab = "Prefab/UIGadget/ChooseBtn";
    public static string TipListContentBtn_Prefab = "Prefab/UIGadget/TipListContentBtn";
    public static string TipListContentDesp_Prefab = "Prefab/UIGadget/TipListContentDesp";
    public static string EnemyImg_Prefab = "Prefab/UIGadget/EnemyImg";
    //Audio
    public static string MagicEject = "Audio/MagicEject";
    public static string CannonArrowEject = "Audio/CannonArrowEject";
    public static string MagicArrowEject = "Audio/MagicArrowEject";
    //UI Sprite
    public static string Archer_lv1 = "Prefab/UI/Archer_lv1";
    public static string Archer_lv2 = "Prefab/UI/Archer_lv2";
    public static string Archer_lv3 = "Prefab/UI/Archer_lv3";
    public static string Archer_lv4A = "Prefab/UI/Archer_lv4A";
    public static string Archer_lv4B = "Prefab/UI/Archer_lv4B";
    public static string Canon_lv1 = "Prefab/UI/Canon_lv1";
    public static string Canon_lv2 = "Prefab/UI/Canon_lv2";
    public static string Canon_lv3 = "Prefab/UI/Canon_lv3";
    public static string Canon_lv4A = "Prefab/UI/Canon_lv4A";
    public static string Canon_lv4B = "Prefab/UI/Canon_lv4B";
    public static string Magic_lv1 = "Prefab/UI/Magic_lv1";
    public static string Magic_lv2 = "Prefab/UI/Magic_lv2";
    public static string Magic_lv3 = "Prefab/UI/Magic_lv3";
    public static string Magic_lv4A = "Prefab/UI/Magic_lv4A";
    public static string Magic_lv4B = "Prefab/UI/Magic_lv4B";

    public static string Coin = "Prefab/UI/Coin";
    public static string Heart = "Prefab/UI/Heart";
    public static string Bow = "Prefab/UI/Bow";
    public static string Lightning = "Prefab/UI/Lightning";
    public static string Bomb = "Prefab/UI/Bomb";

    public static string button_outlined = "Prefab/UI/button_outlined";
    public static string gloves = "Prefab/UI/gloves";
    public static string helmet = "Prefab/UI/helmet";
    public static string home = "Prefab/UI/home";
    public static string dots = "Prefab/UI/dots";

    public static string FireDragonSmall = "Prefab/UI/Enemy/FireDragonSmall";
    public static string FireDragonMid = "Prefab/UI/Enemy/FireDragonMid";
    public static string FireDragonBig = "Prefab/UI/Enemy/FireDragonBig";
    //Effect
    public static string Explo_Large_Blue_Prefab = "Prefab/Tower/Explosions/Blue_DemonFire/large_demonFire";
    public static string Explo_Medium_Blue_Prefab = "Prefab/Tower/Explosions/Blue_DemonFire/medium_demonFire";
    public static string Explo_Small_Blue_Prefab = "Prefab/Tower/Explosions/Blue_DemonFire/small_demonFire";
    public static string Explo_Large_Green_Prefab = "Prefab/Tower/Explosions/Green_WildFire/large_wildFire";
    public static string Explo_Medium_Green_Prefab = "Prefab/Tower/Explosions/Green_WildFire/medium_wildFire";
    public static string Explo_Small_Green_Prefab = "Prefab/Tower/Explosions/Green_WildFire/small_wildFire";
    public static string Explo_Large_Joker_Prefab = "Prefab/Tower/Explosions/JokerFire/large_jokerFire";
    public static string Explo_Medium_Joker_Prefab = "Prefab/Tower/Explosions/JokerFire/medium_jokerFire";
    public static string Explo_Small_Joker_Prefab = "Prefab/Tower/Explosions/JokerFire/small_jokerFire";
    public static string Explo_Large_Purple_Prefab = "Prefab/Tower/Explosions/Purple_DarkFire/large_darkFire";
    public static string Explo_Medium_Purple_Prefab = "Prefab/Tower/Explosions/Purple_DarkFire/medium_darkFire";
    public static string Explo_Small_Purple_Prefab = "Prefab/Tower/Explosions/Purple_DarkFire/small_darkFire";
    public static string Explo_Large_Red_Prefab = "Prefab/Tower/Explosions/Red_OriginalFire/large_originalFire";
    public static string Explo_Medium_Red_Prefab = "Prefab/Tower/Explosions/Red_OriginalFire/medium_originalFire";
    public static string Explo_Small_Red_Prefab = "Prefab/Tower/Explosions/Red_OriginalFire/small_originalFire";
    public static string Explo_Large_Red_NoSmoke_Prefab = "Prefab/Tower/Explosions/Red_OriginalFire_NoSmoke/large_originalFire_noSmoke";
    public static string Explo_Medium_Red_NoSmoke_Prefab = "Prefab/Tower/Explosions/Red_OriginalFire_NoSmoke/medium_originalFire_noSmoke";
    public static string Explo_Small_Red_NoSmoke_Prefab = "Prefab/Tower/Explosions/Red_OriginalFire_NoSmoke/small_originalFire_noSmoke";

    public static string Thunderbolt_Prefab = "Prefab/Effect/Thunderbolt";
    public static string ElectricGround_Prefab = "Prefab/Effect/ElectricGround";
    public static string MagicCannonExplosion_Prefab = "Prefab/Tower/Explosions/MagicCannonExplosion";
    //string
    public static string Tower_General_Tip1 = "1.damage increases by 2% for every enemy tower killed. Extra effect kills are not count";
    public static string Tower_General_Tip2 = "2.Each type of tower can continue to upgrade and interact with other types of towers after reaching the toppest level";

    public static string Archer_General_Tip = "Lv1: atk=20, range=24, fire speed=1.0 \n Lv2: atk=25, range=26, fire speed=0.9 \n Lv3: atk=30, range=28, fire speed=0.8 \n Lv4A: atk=50, range=31, fire speed=0.7 \n Lv4B: atk=40, range=31, fire speed=0.5";
    public static string Archer_Cannon_Tip = "The arrow explode into small explosives after hitting the enemy";
    public static string Archer_Magic_Tip = "The arrow generates electric chains between up to 5 monsters after hitting the enemy";
    public static string Cannon_General_Tip = "Lv1: atk=50, range=24, fire speed=1.5 \n Lv2: atk=55, range=28, fire speed=1.6 \n Lv3: atk=60, range=32, fire speed=1.7 \n Lv4A: atk=70, range=40, fire speed=1.8 \n Lv4B: atk=30, range=40, fire speed=2.0";
    public static string Cannon_Archer_Tip = "Bullet turnes into a repelling shell";
    public static string Cannon_Magic_Tip = "When a shell explodes, it produces a lightningbolt and a thunderstorm area on the ground.(Thunderstorm damage is based on the number of lightning towers planted";
    public static string Magic_General_Tip = "Lv1: atk=30, range=24, fire speed=0.7 \n Lv2: atk=34, range=27, fire speed=0.6 \n Lv3: atk=38, range=30, fire speed=0.5 \n Lv4A: atk=46, range=36, fire speed=0.45 \n Lv4B: atk=42, range=36, fire speed=0.4";
    public static string Magic_Archer_Tip = "Bullet turns into lightning balls, make enemy speed -1";
    public static string Magic_Cannon_Tip = "Bullet turns into a thunder ball to produce a violent shock. Reduce the enemy's speed to a fixed value. Attack speed is reduced";

    public static string SmallWyvern_General_Tip = "speed=3 maxSpeed=10 acc=1 def=20 maxHp=400 reward=40$";
    public static string MidWyvern_General_Tip = "speed=3 maxSpeed=11 acc=1 def=20 maxHp=600 reward=60$";
    public static string BigWyvern_General_Tip = "speed=2 maxSpeed=12 acc=1 def=20 maxHp=1000 reward=100$";

    public static string Left_Btn_Tip = "Press left button of your mouse to move the camera";
    public static string Right_Btn_Tip = "Press right button of your mouse to rotate the camera";
    public static string Wheel_Tip = "Roll the Wheel of your mouse to zoom in/out the camera";
    public static string Keypad_Move_Tip = "Use W, A, S, D or up, left, right, down to move the camera";
    public static string Keypad_Rotate_Tip = "Use Q or E to rotate the camera";
}
