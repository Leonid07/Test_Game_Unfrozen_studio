using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject[] minersEnemy;// ������ �����������
    public GameObject[] minersPlayer;// ������ �������
    public GameObject hitButton;// ������ �����
    public GameObject skipButton;// ������ �������� ����
    public bool step;// ���

    public GameObject playerMiners;// ����� ������� ����� ������� ��� ������ RaycastHit
    public GameObject enemyMiners;// ��������� ������� ����� ������� ��� ������ RaycastHit
    public GameObject BattleCamera;// ������ ���

    bool hit;
    Vector3 ArenaBattleL = new Vector3(-1.7f, -1.4f, 8.52f);// ��������� ����� ��� (��� ������)
    Vector3 ArenaBattleR = new Vector3(1.5f, -1.4f, 8.52f);// ��������� ����� ��� (��� ����������)
    Vector3 L;// ��������� ������� ������
    Vector3 R;// ��������� ������� ����������
    private void Start()
    {
        step = (Random.value > 0.5f);// ��������� ������ ��� (��� ������ �����)
        // ���� ����� ����� �� ���������� ������� ������� �������� ����� ������
        if (step)
        {
            for (int i = 0; i < minersEnemy.Length; i++)
            {
                minersEnemy[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        // ���������� ��� � � ������ ������� ������ ��� ����������
        else if (!step)
        {
            for (int i = 0; i < minersPlayer.Length; i++)
            {
                minersPlayer[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
    private void Update()
    {
        // � ���� ������� ���������� ���������� ������ ������ � ���������� ��� ������� ����� ������� ����
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "MinersPlayer")
            {
                playerMiners = hit.collider.gameObject;
                for (int i = 0; i < minersPlayer.Length; i++)
                {
                    //�������� �� ������ ��������
                    if (minersPlayer[i] == null)
                    {
                        continue;
                    }
                    // ������� ������� ����� ������ � ������ ���������
                    if (minersPlayer[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color == Color.red)
                    {
                        minersPlayer[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
                if (playerMiners.transform.GetChild(0).GetComponent<SpriteRenderer>().color == Color.white)
                {
                    playerMiners.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            else if (hit.collider != null && hit.collider.tag == "enemyMiners")
            {
                enemyMiners = hit.collider.gameObject;
                for (int i = 0; i < minersEnemy.Length; i++)
                {
                    if (minersEnemy[i] == null)
                    {
                        continue;
                    }
                    if (minersEnemy[i].transform.GetChild(0).gameObject.activeInHierarchy)
                    {
                        minersEnemy[i].transform.GetChild(0).gameObject.SetActive(false);
                    }
                    enemyMiners.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        // �������� �� ���������� ������ � ������ ������ ��������� �� ������ �����
        if (step)
        {
            StartCoroutine(enemyIsAttack(1.9f));
            step = !step;
        }
        if (playerMiners && hit)
        {
            StartCoroutine(delayAttack(1.3f));
            hit = !hit;
        }
    }
    // ����� ���������� ���������� ����
    public void Skip()
    {
        for (int i = 0; i < minersPlayer.Length; i++)
        {
            if (minersPlayer[i] == null)
            {
                continue;
            }
            minersPlayer[i].GetComponent<Main>().move = true;
        }
        if (!step)
        {
            for (int i = 0; i < minersEnemy.Length; i++)
            {
                if (minersEnemy[i] == null)
                {
                    continue;
                }
                minersEnemy[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < minersPlayer.Length; i++)
        {
            if (minersPlayer[i] == null)
            {
                continue;
            }
            minersPlayer[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        step = !step;
    }
    // ����� ���������� ������ ������
    public void Hit()
    {
        if (playerMiners && enemyMiners && playerMiners.GetComponent<Main>().move)
        {
            L = playerMiners.transform.position;
            R = enemyMiners.transform.position;
            IsAttacking(playerMiners, enemyMiners);
            enemyMiners.GetComponent<enemy>()._Damage();
            enemyMiners.GetComponent<enemy>().health -= playerMiners.GetComponent<Main>().damage;
            playerMiners.GetComponent<Main>().Attack();
            playerMiners.transform.GetChild(0).gameObject.SetActive(false);
            hit = !hit;
        }
    }
    // � ���� ������ ��� ���������� ����������� � ���������� � �������, ����������� � ������ ����� (����������� � ������������ ��� ������ � �����)
    public void IsAttacking(GameObject goP, GameObject goE)
    {
        goP.transform.position = Vector3.MoveTowards(goP.transform.position, ArenaBattleL, 6);
        goP.transform.localScale = new Vector2(0.4f, 0.4f);

        goE.transform.position = Vector3.MoveTowards(goE.transform.position, ArenaBattleR, 6);
        goE.transform.localScale = new Vector2(-0.4f, 0.4f);
    }
    // � ���� ������ ��� ���������� ����������� �� ��������� �������
    public void Retreat(GameObject goP, GameObject goE, Vector3 trL, Vector3 trR)
    {
        goP.transform.position = Vector3.MoveTowards(goP.transform.position, trL, 6);
        goP.transform.localScale = new Vector2(0.3f, 0.3f);

        goE.transform.position = Vector3.MoveTowards(goE.transform.position, trR, 6);
        goE.transform.localScale = new Vector2(-0.3f, 0.3f);
    }
    // ������� ��� ����� ������� ���������
    IEnumerator delayAttack(float time)
    {
        hitButton.SetActive(false);
        skipButton.SetActive(false);
        BattleCamera.SetActive(true);
        yield return new WaitForSeconds(time);
        Retreat(playerMiners, enemyMiners, L, R);
        BattleCamera.SetActive(false);
        hitButton.SetActive(true);
        skipButton.SetActive(true);
    }
    //������� ��� ����� ���� �����������
    IEnumerator enemyIsAttack(float time)
    {
        for (int i = 0; i < minersEnemy.Length; i++)
        {
            int r = Random.Range(0, minersPlayer.Length);
            if (minersEnemy[i] == null)
            {
                continue;
            }
            hitButton.SetActive(false);
            skipButton.SetActive(false);
            if (minersPlayer[r] == null)
            {
                for (; ; )
                {
                    r = Random.Range(0, minersPlayer.Length);
                    if (minersPlayer[r] != null)
                    {
                        break;
                    }
                }
            }
            BattleCamera.SetActive(true);
            L = minersPlayer[r].transform.position;
            R = minersEnemy[i].transform.position;

            IsAttacking(minersPlayer[r], minersEnemy[i]);
            minersPlayer[r].GetComponent<Main>()._Damage();
            minersPlayer[r].GetComponent<Main>().health -= minersEnemy[i].GetComponent<enemy>().damage;
            minersEnemy[i].GetComponent<enemy>().IsAttack();
            minersEnemy[i].transform.GetChild(0).gameObject.SetActive(false);

            yield return new WaitForSeconds(time);
            Retreat(minersPlayer[r], minersEnemy[i], L, R);

            BattleCamera.SetActive(false);
            yield return new WaitForSeconds(1f);
            hitButton.SetActive(true);
            skipButton.SetActive(true);
        }
        if (!step)
        {
            for (int i = 0; i < minersPlayer.Length; i++)
            {
                if (minersEnemy[i] == null)
                {
                    continue;
                }
                minersEnemy[i].GetComponent<enemy>()._move = true;
            }
            for (int i = 0; i < minersPlayer.Length; i++)
            {
                if (minersPlayer[i] == null)
                {
                    continue;
                }
                minersPlayer[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}