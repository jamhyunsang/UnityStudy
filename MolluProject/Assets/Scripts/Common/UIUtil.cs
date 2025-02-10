using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEngine;

public class UIUtil
{
    public async static UniTask SlideUI(VisualElement UIElement, float Duration, Direction Direction)
    {
        float ElapsedTime = 0f;
        float StartPos = 0;
        float EndPos = 0f;
              
        Vector3 Vector = Vector3.zero;

        switch (Direction)
        {
            case Direction.Up:
                {
                    StartPos = -Screen.height;
                    Vector = Vector3.up;
                }
                break;
            case Direction.Down:
                {
                    StartPos = Screen.height;
                    Vector = Vector3.down;
                }
                break;
            case Direction.Left:
                {
                    StartPos = -Screen.width;
                    Vector = Vector3.left;
                }
                break;
            case Direction.Right:
                {
                    StartPos = Screen.width;
                    Vector = Vector3.right;
                }
                break;
        }

        while (ElapsedTime < Duration)
        {
            ElapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(ElapsedTime / Duration);
            
            float CurrentPos = Mathf.Lerp(StartPos, EndPos, t);
            UIElement.transform.position = Vector * CurrentPos;

            await UniTask.Yield();
        }
    }
}
