using Cysharp.Threading.Tasks;
using UnityEngine;

public class LobbyScene : MonoBehaviour
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
        switch (step)
        {
            case 0: SetRoot(); break;
            case 1: await CreateUI(); break;
        }
    }

    private void NextStep()
    {
        step++;
        StartStep();
    }

    private void SetRoot()
    {
        UIManager.Instance.SetRoot(root);
        NextStep();
    }

    private async UniTask CreateUI()
    {
        await UIManager.Instance.Open<LobbyWindow>("Prefab/UI/Windows/LobbyWindow", UI.Main);
    }
    #endregion
}
