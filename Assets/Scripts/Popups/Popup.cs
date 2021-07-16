using System;
using System.Threading.Tasks;

namespace Hourly.UI
{
    public abstract class Popup : CommonUiBehaviour
    {
        public void Show()
        {
            SetActive(true);
        }
        
        public virtual async Task Init(IPopupData data = null)
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