using Cysharp.Threading.Tasks;

public class LobbyWindow : UIElement
{
    #region Override Method
    public override async UniTask Init(object args)
    {
        await UniTask.Yield();
    }

    public override async UniTask OpenAction()
    {
        await UniTask.Yield();
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
}
