using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Monster/MonsterData")]
public class MonsterData : ScriptableObject
{
    [Header("General")]
    public string _name;
    public MonsterType _monsterType = MonsterType.None;
    [Range(0, 100)]
    public float _changeToDropItems;
    [Tooltip("Radius size where the monster can see the player")]
    [Range(0, 100)]
    public float _rangeOfAwareness;
    public Sprite _icon;


    [Header("Combat Stats")]
    [SerializeField]
    public float _damage;
    [SerializeField]
    public float _health;
    [SerializeField]
    public float _speed;


    [Header("Dialog")]
    [TextArea()]
    public string _battleCry;
}
