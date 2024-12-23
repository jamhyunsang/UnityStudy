using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    #region Override Method
    protected override void Init()
    {
        CreateManagers();
    }
    #endregion

    #region Member Method
    #region init
    private void CreateManagers()
    {
        _ = ResourceManager.Instance;
        _ = UIManager.Instance;
    }
    #endregion

    #region Scene
    public async UniTask LoadScene(Scene scene)
    {
        var currentScene = SceneManager.GetActiveScene();

        var loadTask = SceneManager.LoadSceneAsync($"{scene}", LoadSceneMode.Additive);

        while (!loadTask.isDone)
        {
            //loadTask.progress;
            await UniTask.Yield();
        }

        await loadTask.ToUniTask();

        var unloadTask = SceneManager.UnloadSceneAsync(currentScene);

        while (!unloadTask.isDone)
        {
            //unloadTask.progress;
            await UniTask.Yield();
        }

        await unloadTask.ToUniTask();
    }
    #endregion
    #endregion
}
