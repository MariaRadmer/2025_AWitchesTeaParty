using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

public class MonsterEditor : EditorWindow
{
    private List<MonsterData> monsterDataList = new List<MonsterData>();
    private ListView monsterListView;
    private MonsterData selectedMonster;

    private VisualElement editorPanel;

    private TextField nameField;
    private FloatField healthField;
    private FloatField damageField;



    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/MonsterEditor")]
    public static void ShowExample()
    {
        MonsterEditor wnd = GetWindow<MonsterEditor>();
        wnd.titleContent = new GUIContent("MonsterEditor");
    }

    public void CreateGUI()
    {

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor scripts/MonsterEditor.uxml");
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor scripts/MonsterEditor.uss");

        VisualElement root = visualTree.Instantiate();

        rootVisualElement.Add(root);
        rootVisualElement.styleSheets.Add(styleSheet);


        monsterListView = root.Q<ListView>("monster-list");
        editorPanel = root.Q<ScrollView>("editor-panel");

        nameField = root.Q<TextField>("name-field");
        healthField = root.Q<FloatField>("health-field");
        damageField = root.Q<FloatField>("damage-field");

        SetEditorEnabled(false);

        LoadMonsters();

        root.Q<Button>("refresh-button").clicked += LoadMonsters;

        monsterListView.makeItem = () => new Label();
        monsterListView.bindItem = (element, index) =>
        {
            var label = element as Label;
            label.text = monsterDataList[index].name;
        };

        monsterListView.selectionType = SelectionType.Single;
        monsterListView.selectionChanged += OnMonsterSelected;

        nameField.RegisterValueChangedCallback(evt => { if (selectedMonster != null) { selectedMonster._name = evt.newValue; MarkDirty(); RefreshMonsterList();}});
        healthField.RegisterValueChangedCallback(evt => { if (selectedMonster != null) { selectedMonster._health = evt.newValue; MarkDirty(); } });
        damageField.RegisterValueChangedCallback(evt => { if (selectedMonster != null) { selectedMonster._damage = evt.newValue; MarkDirty(); } });

    }


    private void SetEditorEnabled(bool enabled)
    {
        nameField.SetEnabled(enabled);
        healthField.SetEnabled(enabled);
        damageField.SetEnabled(enabled);
    }

    private void LoadMonsters()
    {
        string[] guids = AssetDatabase.FindAssets("t:MonsterData");
        monsterDataList = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<MonsterData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(monster => monster != null)
            .ToList();

        monsterListView.itemsSource = monsterDataList;
        monsterListView.Rebuild();

        selectedMonster = null;
        monsterListView.ClearSelection();

        SetEditorEnabled(false);

    }

    private void OnMonsterSelected(IEnumerable<object> selectedItems)
    {
        selectedMonster = selectedItems.Cast<MonsterData>().FirstOrDefault();
        if (selectedMonster != null)
        {
            SetEditorEnabled(true);
            nameField.UnregisterValueChangedCallback(OnNameChanged);
            healthField.UnregisterValueChangedCallback(OnHealthChanged);
            damageField.UnregisterValueChangedCallback(OnDamageChanged);

            nameField.SetValueWithoutNotify(selectedMonster._name);
            healthField.SetValueWithoutNotify(selectedMonster._health);
            damageField.SetValueWithoutNotify(selectedMonster._damage);

            nameField.RegisterValueChangedCallback(OnNameChanged);
            healthField.RegisterValueChangedCallback(OnHealthChanged);
            damageField.RegisterValueChangedCallback(OnDamageChanged);
        } else
        {
            selectedMonster = null;
            SetEditorEnabled(false);

        }
    }


    private void MarkDirty()
    {
        if (selectedMonster != null)
        {
            EditorUtility.SetDirty(selectedMonster);
        }
    }

    private void RefreshMonsterList()
    {
        // To reflect name changes, rebuild the list
        monsterListView.RefreshItems();
    }

    private void OnNameChanged(ChangeEvent<string> evt)
    {
        selectedMonster._name = evt.newValue;
        MarkDirty();
        RefreshMonsterList();
    }

    private void OnHealthChanged(ChangeEvent<float> evt)
    {
        selectedMonster._health = evt.newValue;
        MarkDirty();
    }

    private void OnDamageChanged(ChangeEvent<float> evt)
    {
        selectedMonster._damage = evt.newValue;
        MarkDirty();
    }

}
