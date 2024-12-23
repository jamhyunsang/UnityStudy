using Cysharp.Threading.Tasks;
using UnityEngine;

public class TitleScene : MonoBehaviour 
{
    #region Cashed Object
    [SerializeField] private Root root;
    #endregion

    #region Unity Method
    private void Start()
    {
        StartStep();
    }
    #endregion

    #region Member Property
    private int step = 0;
    #endregion

    #region Member Method
    private async void StartStep()
    {
        switch(step)
        {
            case 0: CreateManagers(); break;
            case 1: SetRoot(); break;
            case 2: await CreateUI(); break;
        }
    }

    private void NextStep()
    {
        step++;
        StartStep();
    }

    private void CreateManagers()
    {
        _ = GameManager.Instance;
        NextStep();
    }

    private void SetRoot()
    {
        UIManager.Instance.SetRoot(root);
        NextStep();
    }

    private async UniTask CreateUI()
    {
        await UIManager.Instance.Open<TitleWindow>("Prefab/UI/Windows/TitleWindow", UI.Main, null, false);
    }
    #endregion
}
