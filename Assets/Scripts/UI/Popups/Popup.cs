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
        
        protected virtual async void OnShow()
        {
        }

        protected virtual async void OnHide()
        {
        }

        private async void OnEnable()
        {
            OnShow();
        }

        private async void OnDisable()
        {
            OnHide();
        }

        public async void Close()
        {
            SetActive(false);
        }
    }

    public interface IPopupData
    {}
}