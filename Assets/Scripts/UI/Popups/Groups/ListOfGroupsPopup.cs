using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hourly.Group;
using Hourly.Profile;
using Hourly.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Hourly.UI
{
    public class ListOfGroupsPopup : Popup
    {
        [SerializeField] private ToDoGroupCell _groupCellPrefab;
        private ToDoGroupPlusCell PlusCell => GetCachedComponentInChildren<ToDoGroupPlusCell>(true);

        private ScrollRect Scroll => GetCachedComponentInChildren<ScrollRect>();
        private Transform Contents => Scroll.content;

        private List<ToDoGroupCell> GroupCells
        {
            set => _groupCells = value;
            get => _groupCells ??= new List<ToDoGroupCell>();
        }
        private List<ToDoGroupCell> _groupCells;

        private Data _data;

        public override async Task Init(IPopupData data = null)
        {
            await base.Init(data);
            _data = data as Data;

            PlusCell.Init(new ToDoGroupPlusCell.Data() {OnGroupCreated = AddNewGroupToList});

            ClearList();

            var groups = _data.AllGroups;
            foreach (var doGroup in groups)
            {
                await CreateCell(doGroup);
            }
            
            MovePlusSectionToTheEnd();
        }

        protected override async void OnShow()
        {
            base.OnShow();
            
            MovePlusSectionToTheEnd();
        }

        private async Task CreateCell(ToDoGroup doGroup)
        {
            var group = Instantiate(_groupCellPrefab, Contents);
            await group.Init(new ToDoGroupCell.Data() {ToDoGroup = doGroup,OnGroupClicked = _data.OnGroupCellClicked});
            GroupCells.Add(group);
        }

        public async void AddNewGroupToList(string name)
        {
            var group = GroupManager.CreateNewGroup(name);
            await CreateCell(group);
            MovePlusSectionToTheEnd();
        }

        private async void MovePlusSectionToTheEnd()
        {
            await Task.Delay(1);
            PlusCell.transform.SetSiblingIndex(GroupCells.Count);
            await RebuildAllRects();
        }

        public void ClearList()
        {
            if (GroupCells.IsNullOrEmpty()) return;

            for (int i = GroupCells.Count - 1; i >= 0; i--)
            {
                Destroy(GroupCells[i].gameObject);
            }

            GroupCells.Clear();
        }


        public class Data : IPopupData
        {
            public List<ToDoGroup> AllGroups;
            public Action<ToDoGroup> OnGroupCellClicked;
        }
    }
}