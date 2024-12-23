using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : Singleton<UIManager>
{
    #region Override Method
    protected override void Init()
    {
        InitUIManager();
    }
    #endregion

    #region Member Property
    private Root root = null;
    Dictionary<string, UIElement> elements = null;
    private PanelSettings panelSettings = null;
    #endregion

    #region Member Method
    public async void InitUIManager()
    {
        elements = new Dictionary<string, UIElement>();
        panelSettings = await ResourceManager.Instance.LoadResourceAsync<PanelSettings>("Prefab/UI/Base/PanelSettings", false);
    }

    public void SetRoot(Root root)
    {
        this.root = root;
    }

    public void ChangeRoot(Root root)
    {
        SetRoot(root);
    }

    private GameObject GetRootUI(UI ui)
    {
        switch (ui)
        {
            case UI.BackGround:
                return root.BackGround;
            case UI.Main:
                return root.Main;
            case UI.Popup:
                return root.Popup;
            case UI.Fade:
                return root.Fade;
            default:
                return null;
        }
    }

    public async UniTask<T> Open<T>(string resourcePath, UI ui, object args = null, bool isAddressable = true) where T : UIElement    {
        if(root == null)
        {
            Debug.LogError("Root Empty");
            return null;
        }

        if(elements.ContainsKey(typeof(T).Name))
        {
            Debug.LogError("already Opened");
            return null;
        }

        var resource = await ResourceManager.Instance.LoadResourceAsync<VisualTreeAsset>(resourcePath, isAddressable);
        if(resource == null)
        {
            Debug.LogError("resource Empty");
            return null;
        }

        var uiObj = GetRootUI(ui);

        var obj = new GameObject(typeof(T).Name);
        obj.transform.parent = uiObj.transform;

        var uiDocument = obj.AddComponent<UIDocument>();
        uiDocument.panelSettings = panelSettings;
        uiDocument.visualTreeAsset = resource;

        var uiComponent = obj.AddComponent<T>();
        uiComponent.SetUIDocument(uiDocument);

        await uiComponent.Init(args);
        await uiComponent.OpenAction();

        elements.Add(typeof(T).Name, uiComponent);

        ResourceManager.Instance.ReleaseResource(resource, isAddressable);

        return uiComponent;
    }

    public async UniTask Close<T>() where T : UIElement
    {
        if (!elements.ContainsKey(typeof(T).Name))
        {
            Debug.LogError("not Opened");
            return;
        }

        await elements[typeof(T).Name].CloseAction();

        Destroy(elements[typeof(T).Name]);

        elements.Remove(typeof(T).Name);
    }
    #endregion
}
