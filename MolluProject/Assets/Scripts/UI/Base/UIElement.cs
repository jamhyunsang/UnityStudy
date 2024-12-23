using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIElement : MonoBehaviour
{
    protected UIDocument document;
    public void SetUIDocument(UIDocument document)
    {
        this.document = document;
    }

    public abstract UniTask Init(object args);
    public abstract UniTask OpenAction();
    public abstract UniTask CloseAction();
    public abstract UniTask Refresh();

    protected bool isButtonActive = false;
    private float buttonActiveDuration = 0f;
    private void Update()
    {
        if(isButtonActive)
        {
            buttonActiveDuration += Time.deltaTime;

            if(buttonActiveDuration > 0.3f )
            {
                isButtonActive = false;
            }
        }
    }
}
