using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;

public class MonsterEditor : EditorWindow
{
    private List<MonsterData> monsterDataList = new List<MonsterData>();
    private ListView monsterListView;
    private MonsterData selectedMonster;

    private VisualElement editorPanel;

    private TextField nameField;
    private FloatField healthField;
    private FloatField damageField;
    private ObjectField iconField;
    private Image iconPreview;

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
        iconField = root.Q<ObjectField>("icon-field");
        iconPreview = root.Q<Image>("icon-preview");

        iconField.objectType = typeof(Sprite);
        iconPreview.scaleMode = ScaleMode.ScaleToFit;

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
        iconField.RegisterValueChangedCallback(evt => { if (selectedMonster != null) { selectedMonster._icon = (Sprite)evt.newValue; MarkDirty(); } });
    }


    private void SetEditorEnabled(bool enabled)
    {
        nameField.SetEnabled(enabled);
        healthField.SetEnabled(enabled);
        damageField.SetEnabled(enabled);
        iconField.SetEnabled(enabled);
        iconPreview.SetEnabled(enabled);
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
            iconField.UnregisterValueChangedCallback(OnIconChanged);

            nameField.SetValueWithoutNotify(selectedMonster._name);
            healthField.SetValueWithoutNotify(selectedMonster._health);
            damageField.SetValueWithoutNotify(selectedMonster._damage);
            iconField.SetValueWithoutNotify(selectedMonster._icon);
            UpdateSpritePreview();

            nameField.RegisterValueChangedCallback(OnNameChanged);
            healthField.RegisterValueChangedCallback(OnHealthChanged);
            damageField.RegisterValueChangedCallback(OnDamageChanged);
            iconField.RegisterValueChangedCallback(OnIconChanged);

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

    private void OnIconChanged(ChangeEvent<Object> evt)
    {
        selectedMonster._icon = (Sprite)evt.newValue;
        MarkDirty();
        UpdateSpritePreview();
    }

    private void UpdateSpritePreview()
    {
        if (selectedMonster._icon != null && selectedMonster._icon.texture != null)
        {
            Texture2D cropped = GetCroppedTextureFromSprite(selectedMonster._icon);
            iconPreview.style.backgroundImage = new StyleBackground(cropped); // selectedMonster._icon.texture);
        }
        else
        {
            iconPreview.style.backgroundImage = null;
        }
    }

    private Texture2D GetCroppedTextureFromSprite(Sprite sprite)
    {
        if (sprite == null) return null;

        Rect spriteRect = sprite.rect;
        Texture2D sourceTexture = sprite.texture;

        // Create a new texture with the size of the sprite
        Texture2D croppedTexture = new Texture2D((int)spriteRect.width, (int)spriteRect.height);
        croppedTexture.filterMode = sourceTexture.filterMode;
        croppedTexture.wrapMode = TextureWrapMode.Clamp;

        Color[] pixels = sourceTexture.GetPixels(
            (int)spriteRect.x,
            (int)spriteRect.y,
            (int)spriteRect.width,
            (int)spriteRect.height
        );

        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        return croppedTexture;
    }

}
