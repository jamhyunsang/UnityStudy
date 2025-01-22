#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEngine.UIElements;

public class Settings : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Custom/Settings")]
    public static void Show()
    {
        Settings wnd = GetWindow<Settings>();
        wnd.titleContent = new GUIContent("Settings");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement UXML = m_VisualTreeAsset.Instantiate();
        root.Add(UXML);

        UXML.Q<Button>("Btn_Addressable").clicked += OnClick_SetAddressablePath;
    }

    private void OnClick_SetAddressablePath()
    {
        var Settings = AddressableAssetSettingsDefaultObject.GetSettings(false);
        if (Settings == null)
        {
            Debug.LogError("Addressable Asset Settings�� �������� �ʾҽ��ϴ�. Addressable �ý����� Ȱ��ȭ�ϼ���.");
            return;
        }

        string TargetFolder = "Assets/ResourceAddressable";

        if (!Directory.Exists(TargetFolder))
        {
            Debug.LogError($"������ ������ �������� �ʽ��ϴ�: {TargetFolder}");
            return;
        }

        string[] Files = Directory.GetFiles(TargetFolder, "*.*", SearchOption.AllDirectories);

        foreach (var File in Files)
        {
            if (File.EndsWith(".meta")) continue;

            string AssetPath = File.Replace("\\", "/");
            string Guid = AssetDatabase.AssetPathToGUID(AssetPath);
            if (string.IsNullOrEmpty(Guid)) continue;

            var Entry = Settings.FindAssetEntry(Guid);
            if (Entry == null)
            {
                var Group = Settings.DefaultGroup;
                Entry = Settings.CreateOrMoveEntry(Guid, Group);

                // Ȯ���ڸ� �����ϰ�, ��Ʈ ���� ��� ��η� Address ����
                string RelativePath = AssetPath.Replace(TargetFolder + "/", ""); // ��Ʈ ���� ����
                RelativePath = Path.ChangeExtension(RelativePath, null); // Ȯ���� ����
                Entry.address = RelativePath;

                Debug.Log($"Addressable ���: {Entry.address}");
            }
            else
            {
                Debug.LogWarning($"�̹� Addressable�� ��ϵ� �����Դϴ�: {AssetPath}");
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Addressable �ڵ� ���� �Ϸ�!");
    }
}
#endif