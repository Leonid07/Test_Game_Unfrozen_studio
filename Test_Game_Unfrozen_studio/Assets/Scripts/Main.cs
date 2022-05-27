using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;
public class Main : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset Idle, attack, Damage, DoubleShift, PickaxeCharge;
    string[] kit = new string[3] { "attack", "DoubleShift", "PickaxeCharge" };// массив для мув сетов
    string currentState;
    string currentAnimation;
    public int damage = 15;//урон
    public int health = 100;//здоровье
    public bool move = true;//переменная для проверки хода
    public Slider sliderHp;//ползунок хп
    void Start()
    {
        //обычное состояние (когда стоит на месте)
        currentState = "Idle";
        SetCharapter(currentState);
    }
    void Update()
    {
        //проверка на наличие хп
        if (health <= 0)
        {
            Destroy(gameObject, 2.2f);
        }
        //проверка на то что игрок сделал удар
        if (!move)
        {
            StartCoroutine(delayAttack(1.3f));
        }
        sliderHp.value = health;// полоска здоровья
    }
    /* метод для смены анимации
     * первый параметр для новой анимации
     * второй параметр отвечает за цикличность анимации
     * третий параметр отвечает за скорость проигрования анимации
     * */
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        currentAnimation = animation.name;
    }
    //проверяет состояние персонажа и соответственно устанавливает анимацию
    public void SetCharapter(string state)
    {
        if (state.Equals("Idle"))
        {
            SetAnimation(Idle, true, 1f);
        }
        if (state.Equals("attack"))
        {
            SetAnimation(attack, false, 1f);
        }
        if (state.Equals("Damage"))
        {
            SetAnimation(Damage, false, 1f);
        }
        if (state.Equals("DoubleShift"))
        {
            SetAnimation(DoubleShift, false, 2f);
        }
        if (state.Equals("PickaxeCharge"))
        {
            SetAnimation(PickaxeCharge, false, 1f);
        }
    }
    // медот в котором анимация получения урона
    public void _Damage()
    {
        StartCoroutine(_damage(0.6f));
    }
    //рандомные мув сеты для игрока
    public void Attack()
    {
        if (move)
        {
            int kit_count = Random.Range(0,kit.Length);
            SetCharapter(kit[kit_count]);
            move = !move;
        }
    }
    //корутин спокойного состояния
    public IEnumerator delayAttack(float time)
    {
        yield return new WaitForSeconds(time);
        SetCharapter("Idle");
    }
    //корутин анимаций получения урона
    IEnumerator _damage(float time)
    {
        yield return new WaitForSeconds(time);
        SetCharapter("Damage");
        yield return new WaitForSeconds(time);
        SetCharapter("Idle");
    }
}