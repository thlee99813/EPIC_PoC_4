using UnityEngine;

public class PickHandPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panelRoot;
    [SerializeField] private HandDropZone _leftHand;
    [SerializeField] private HandDropZone _rightHand;
    [SerializeField] private CombatController _combatController;


    public void Show()
    {
        _panelRoot.SetActive(true);
    }

    public void Hide()
    {
        _panelRoot.SetActive(false);
    }

    public void PickLeftAttack()
    {
        Hide();
        _combatController.ResolvePlayerAttack(AttackType.Left);

    }

    public void PickRightAttack()
    {
        Hide();
        _combatController.ResolvePlayerAttack(AttackType.Right);
    }

    public void PickBothAttack()
    {
        Hide();
        _combatController.ResolvePlayerAttack(AttackType.Both);
    }
}