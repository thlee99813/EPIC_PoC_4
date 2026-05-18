using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] private HandDropZone _playerLeftHand;
    [SerializeField] private HandDropZone _playerRightHand;
    [SerializeField] private EnemyDraftPicker _enemyDraftPicker;
    [SerializeField] private BattleStatusView _statusView;
    [SerializeField] private SystemLogView _systemLogView;
    [SerializeField] private UIController _uiController;




    private const float MaxHp = 1000f;

    private float _playerHp = MaxHp;
    private float _enemyHp = MaxHp;
    private int _caution;

    private void Start()
    {
        _statusView.SetHp(_playerHp, _enemyHp);
        _statusView.SetCaution(_caution);
    }
    public void ResolvePlayerAttack(AttackType playerAttack)
    {
        AttackType enemyAttack = ChooseEnemyAttack();

        float playerLeft = _playerLeftHand.CurrentValue;
        float playerRight = _playerRightHand.CurrentValue;
        float enemyLeft = _enemyDraftPicker.LowValue;
        float enemyRight = _enemyDraftPicker.HighValue;

        ResolveCombat(playerAttack, enemyAttack, playerLeft, playerRight, enemyLeft, enemyRight);
    }

    private AttackType ChooseEnemyAttack()
    {
        float enemyLeft = _enemyDraftPicker.LowValue;
        float enemyRight = _enemyDraftPicker.HighValue;

        float leftWeight = 50f;
        float rightWeight = 35f;
        float bothWeight = 15f;

        if (enemyLeft <= -40f)
        {
            leftWeight += 25f;
        }

        if (enemyRight >= 60f)
        {
            rightWeight += 25f;
        }

        if (enemyLeft <= -40f && enemyRight >= 60f)
        {
            bothWeight += 25f;
        }
        else
        {
            bothWeight -= 10f;
        }


        bothWeight = Mathf.Max(0f, bothWeight);

        if (Random.value < 0.15f)
        {
            return PickRandomSingleAttack();
        }

        return PickWeightedAttack(leftWeight, rightWeight, bothWeight);
    }

    private AttackType PickRandomSingleAttack()
    {
        return Random.value < 0.65f ? AttackType.Left : AttackType.Right;
    }

    private AttackType PickWeightedAttack(float leftWeight, float rightWeight, float bothWeight)
    {
        float totalWeight = leftWeight + rightWeight + bothWeight;
        float randomValue = Random.Range(0f, totalWeight);

        if (randomValue < leftWeight)
        {
            return AttackType.Left;
        }

        randomValue -= leftWeight;

        if (randomValue < rightWeight)
        {
            return AttackType.Right;
        }

        return AttackType.Both;
    }

    private void ResolveCombat(
        AttackType playerAttack,
        AttackType enemyAttack,
        float playerLeft,
        float playerRight,
        float enemyLeft,
        float enemyRight)
    {
        string log;

        if (playerAttack != AttackType.Both && enemyAttack != AttackType.Both && playerAttack != enemyAttack)
        {
            _caution++;
            _statusView.SetCaution(_caution);

            log = $"플레이어는 {GetAttackName(playerAttack)}을 선택했습니다."    +
                  $"적은 {GetAttackName(enemyAttack)}을 선택했습니다.\n" +
                  $"서로 다른 손을 내 전투가 발생하지 않았습니다. 긴장도 +1";

            ShowCombatLog(log);
            return;
        }

        float damageMultiplier = 1f + (_caution * 0.2f);
        float playerDamage = 0f;
        float enemyDamage = 0f;

        if (playerAttack == AttackType.Both)
        {
            ResolveBothAttack(true, playerRight, playerLeft, enemyRight, enemyLeft, ref playerDamage, ref enemyDamage);
        }
        else if (enemyAttack == AttackType.Both)
        {
            ResolveBothAttack(false, enemyRight, enemyLeft, playerRight, playerLeft, ref playerDamage, ref enemyDamage);
        }
        else if (playerAttack == AttackType.Right)
        {
            float diff = Mathf.Abs(playerRight - enemyRight);

            if (playerRight > enemyRight)
            {
                enemyDamage = diff;
            }
            else if (enemyRight > playerRight)
            {
                playerDamage = diff;
            }
        }
        else if (playerAttack == AttackType.Left)
        {
            float diff = Mathf.Abs(playerLeft - enemyLeft);

            if (playerLeft < enemyLeft)
            {
                enemyDamage = diff;
            }
            else if (enemyLeft < playerLeft)
            {
                playerDamage = diff;
            }
        }

        playerDamage *= damageMultiplier;
        enemyDamage *= damageMultiplier;

        _playerHp = Mathf.Max(0f, _playerHp - playerDamage);
        _enemyHp = Mathf.Max(0f, _enemyHp - enemyDamage);

        _caution = 0;

        _statusView.SetHp(_playerHp, _enemyHp);
        _statusView.SetCaution(_caution);

        log = $"플레이어는 {GetAttackName(playerAttack)}을 선택했습니다. 좌:{playerLeft:0.#} 우:{playerRight:0.#}   " +
              $"적은 {GetAttackName(enemyAttack)}을 선택했습니다. 좌:{enemyLeft:0.#} 우:{enemyRight:0.#}\n" +
              $"플레이어 피해: {playerDamage:0.#}, 적 피해: {enemyDamage:0.#}";

        ShowCombatLog(log);    
    }

    private void ResolveBothAttack(
        bool isPlayerAttacker,
        float attackerRight,
        float attackerLeft,
        float defenderRight,
        float defenderLeft,
        ref float playerDamage,
        ref float enemyDamage)
    {
        bool winsRight = attackerRight > defenderRight;
        bool winsLeft = attackerLeft < defenderLeft;

        if (winsRight && winsLeft)
        {
            float damage = (Mathf.Abs(attackerRight - defenderRight) + Mathf.Abs(attackerLeft - defenderLeft)) * 2f;

            if (isPlayerAttacker)
            {
                enemyDamage = damage;
            }
            else
            {
                playerDamage = damage;
            }

            return;
        }

        float failDamage = 0f;

        if (!winsRight)
        {
            failDamage += Mathf.Abs(attackerRight - defenderRight) * 2f;
        }

        if (!winsLeft)
        {
            failDamage += Mathf.Abs(attackerLeft - defenderLeft) * 2f;
        }

        if (isPlayerAttacker)
        {
            playerDamage = failDamage;
        }
        else
        {
            enemyDamage = failDamage;
        }
    }

    

    private string GetAttackName(AttackType attackType)
    {
        if (attackType == AttackType.Left)
        {
            return "좌공격";
        }

        if (attackType == AttackType.Right)
        {
            return "우공격";
        }

        return "양손공격";
    }
    private void ShowCombatLog(string log)
    {
        _systemLogView.ShowRaw(log);

        if (_playerHp <= 0f)
        {
            _uiController.ShowGameOver();
            return;
        }

        if (_enemyHp <= 0f)
        {
            _uiController.ShowGameClear();
            return;
        }

        _uiController.ShowNextRoundButton();
    }
}