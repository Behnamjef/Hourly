using System.Threading.Tasks;
using Hourly.Group;

namespace Hourly.UI
{
    public class ToDoGroupCell : CommonUiBehaviour
    {
        private CustomText TitleText => GetCachedComponentInChildren<CustomText>();
        public Data data { private set; get; }
        public async Task Init(Data data)
        {
            this.data = data;
            TitleText.text = this.data.ToDoGroup.Name;
        }
        
        public class Data
        {
            public ToDoGroup ToDoGroup;
        }
    }
}