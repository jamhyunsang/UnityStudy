using UnityEngine;

public class Root : MonoBehaviour
{
    #region Cashed Object
    [SerializeField] private GameObject backGround = null;
    [SerializeField] private GameObject main = null;
    [SerializeField] private GameObject popup = null;
    [SerializeField] private GameObject fade = null;
    #endregion

    #region Member Property
    public GameObject BackGround { get { return backGround; } }
    public GameObject Main { get { return main; } }
    public GameObject Popup { get { return popup; } }
    public GameObject Fade { get {return fade; } }
    #endregion
}
