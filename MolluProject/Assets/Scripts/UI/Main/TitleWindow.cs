using Cysharp.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleWindow : UIElement
{
    #region Member Property
    private Button startButton = null;
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

        // 시작 위치를 화면 오른쪽 바깥으로 설정
        root.style.position = Position.Absolute;
        root.style.left = new StyleLength(Screen.width);

        // 애니메이션: 오른쪽에서 왼쪽으로 등장
        await SlideUIFromRightToLeft(root, duration: 10.0f);

        startButton.SetEnabled(true);
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
        startButton = document.rootVisualElement.Q<Button>("GameStartButton");
    }

    private void UISetting()
    {
        startButton.clicked += GameStart;

        startButton.SetEnabled(false);
    }

    private async UniTask SlideUIFromRightToLeft(VisualElement uiElement, float duration)
    {
        float elapsedTime = 0f;
        float startX = Screen.width;  // 시작 위치 (화면 오른쪽 바깥)
        float endX = 0f;              // 목표 위치 (왼쪽 정렬)

        // 프레임마다 위치 보간
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Lerp를 사용해 스타일 위치 보간
            float currentX = Mathf.Lerp(startX, endX, t);
            uiElement.transform.position = Vector3.right * currentX;

            await UniTask.Yield(); // 다음 프레임까지 대기
        }

        // 최종 위치 보정
        uiElement.style.left = new StyleLength(endX);
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
