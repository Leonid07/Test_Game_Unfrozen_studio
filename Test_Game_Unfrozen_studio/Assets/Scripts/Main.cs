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
    string[] kit = new string[3] { "attack", "DoubleShift", "PickaxeCharge" };// ������ ��� ��� �����
    string currentState;
    string currentAnimation;
    public int damage = 15;//����
    public int health = 100;//��������
    public bool move = true;//���������� ��� �������� ����
    public Slider sliderHp;//�������� ��
    void Start()
    {
        //������� ��������� (����� ����� �� �����)
        currentState = "Idle";
        SetCharapter(currentState);
    }
    void Update()
    {
        //�������� �� ������� ��
        if (health <= 0)
        {
            Destroy(gameObject, 2.2f);
        }
        //�������� �� �� ��� ����� ������ ����
        if (!move)
        {
            StartCoroutine(delayAttack(1.3f));
        }
        sliderHp.value = health;// ������� ��������
    }
    /* ����� ��� ����� ��������
     * ������ �������� ��� ����� ��������
     * ������ �������� �������� �� ����������� ��������
     * ������ �������� �������� �� �������� ������������ ��������
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
    //��������� ��������� ��������� � �������������� ������������� ��������
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
    // ����� � ������� �������� ��������� �����
    public void _Damage()
    {
        StartCoroutine(_damage(0.6f));
    }
    //��������� ��� ���� ��� ������
    public void Attack()
    {
        if (move)
        {
            int kit_count = Random.Range(0,kit.Length);
            SetCharapter(kit[kit_count]);
            move = !move;
        }
    }
    //������� ���������� ���������
    public IEnumerator delayAttack(float time)
    {
        yield return new WaitForSeconds(time);
        SetCharapter("Idle");
    }
    //������� �������� ��������� �����
    IEnumerator _damage(float time)
    {
        yield return new WaitForSeconds(time);
        SetCharapter("Damage");
        yield return new WaitForSeconds(time);
        SetCharapter("Idle");
    }
}