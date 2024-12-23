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

        // ���� ��ġ�� ȭ�� ������ �ٱ����� ����
        root.style.position = Position.Absolute;
        root.style.left = new StyleLength(Screen.width);

        // �ִϸ��̼�: �����ʿ��� �������� ����
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
        float startX = Screen.width;  // ���� ��ġ (ȭ�� ������ �ٱ�)
        float endX = 0f;              // ��ǥ ��ġ (���� ����)

        // �����Ӹ��� ��ġ ����
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Lerp�� ����� ��Ÿ�� ��ġ ����
            float currentX = Mathf.Lerp(startX, endX, t);
            uiElement.transform.position = Vector3.right * currentX;

            await UniTask.Yield(); // ���� �����ӱ��� ���
        }

        // ���� ��ġ ����
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
