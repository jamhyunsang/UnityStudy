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
            Debug.LogError("Addressable Asset Settings이 설정되지 않았습니다. Addressable 시스템을 활성화하세요.");
            return;
        }

        string TargetFolder = "Assets/ResourceAddressable";

        if (!Directory.Exists(TargetFolder))
        {
            Debug.LogError($"지정된 폴더가 존재하지 않습니다: {TargetFolder}");
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

                // 확장자를 제거하고, 루트 폴더 상대 경로로 Address 설정
                string RelativePath = AssetPath.Replace(TargetFolder + "/", ""); // 루트 폴더 제거
                RelativePath = Path.ChangeExtension(RelativePath, null); // 확장자 제거
                Entry.address = RelativePath;

                Debug.Log($"Addressable 등록: {Entry.address}");
            }
            else
            {
                Debug.LogWarning($"이미 Addressable에 등록된 파일입니다: {AssetPath}");
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Addressable 자동 설정 완료!");
    }
}
#endif