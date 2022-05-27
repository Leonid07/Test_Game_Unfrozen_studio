using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;

public class enemy : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset Idle, attack, Damage, DoubleShift, PickaxeCharge;
    string[] kit = new string[3] { "attack", "DoubleShift", "PickaxeCharge" };// массив для мув сетов
    string currentState;
    string currentAnimation;
    public int damage = 10;//урон
    public int health = 100;//здоровье
    public bool _move = true;//переменная для проверки хода
    public Slider sliderHp;//ползунок хп
    private void Start()
    {
        //обычное состояние (когда стоит на месте)
        currentState = "Idle";
        SetCharapter(currentState);
    }
    private void Update()
    {
        //проверка на то что игрок сделал удар
        if (!_move)
        {
            StartCoroutine(delayAttack(1.3f));
        }
        sliderHp.value = health;
        //проверка на наличие хп
        if (health <= 0)
        {
            Destroy(gameObject, 2.2f);
        }
    }
    public void IsAttack()
    {
        if (_move)
        {
            int kit_count = Random.Range(0, kit.Length);
            SetCharapter(kit[kit_count]);
            _move = !_move;
        }
    }
    // медот в котором анимация получения урона
    public void _Damage()
    {
        StartCoroutine(_damage(0.6f));
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
        else if (state.Equals("attack"))
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
    IEnumerator _damage(float time)
    {
        yield return new WaitForSeconds(time);
        SetCharapter("Damage");
        yield return new WaitForSeconds(time);
        SetCharapter("Idle");
    }
    IEnumerator delayAttack(float time)
    {
        yield return new WaitForSeconds(time);
        SetCharapter("Idle");
    }
}