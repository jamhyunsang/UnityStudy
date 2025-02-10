using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class TitleWindow : UIElement
{
    #region Cashed Object
    private Button Btn_Start = null;
    #endregion

    #region Member Property
    private int m_Step = 0;
    #endregion

    #region Override Method
    public override async UniTask Init(object args)
    {
        UICashing();
        UISetting();

        await UniTask.Yield();
    }

    public override async UniTask OpenAction()
    {
        var root = document.rootVisualElement;

        await UIUtil.SlideUI(root, 10.0f, Direction.Right);

        StartStep();
    }

    public override async UniTask CloseAction()
    {
        await UniTask.Yield();
    }
    
    public override async UniTask Refresh()
    {
        await UniTask.Yield();
    }
    #endregion

    #region Member Method
    private void UICashing()
    {
        Btn_Start = document.rootVisualElement.Q<Button>("GameStartButton");
    }

    private void UISetting()
    {
        Btn_Start.clicked += GameStart;

        Btn_Start.SetEnabled(false);
    }

    private async void StartStep()
    {
        switch (m_Step)
        {
            case 0: await TableLoad(); break;
            case 1: GameReady(); break;
        }
    }

    private void NextStep()
    {
        m_Step++;
        StartStep();
    }

    private async UniTask TableLoad()
    {
        await DataManager.Instance.Load();
        NextStep();
    }

    private void GameReady()
    {
        Btn_Start.SetEnabled(true);
    }
    #endregion

    #region Button Event
    private async void GameStart()
    {
        isButtonActive = true;

        await GameManager.Instance.LoadScene(Scene.Lobby);
    }
    #endregion
}
