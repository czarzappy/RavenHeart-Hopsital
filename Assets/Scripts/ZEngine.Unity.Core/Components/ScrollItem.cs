namespace ZEngine.Unity.Core.Components
{
    public abstract class ScrollItem<TInitData> : UIMonoBehaviour
    {
        public abstract void Init(TInitData initData);
    }
}