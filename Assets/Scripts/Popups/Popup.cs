using System;

namespace Hourly.UI
{
    public abstract class Popup : CommonBehaviour
    {
        public void Show()
        {
            SetActive(true);
        }
        
        public virtual void Init(IPopupData data = null)
        {
        }
        
        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }

        private void OnEnable()
        {
            OnShow();
        }

        private void OnDisable()
        {
            OnHide();
        }

        public void Close()
        {
            SetActive(false);
        }
    }

    public interface IPopupData
    {}
}