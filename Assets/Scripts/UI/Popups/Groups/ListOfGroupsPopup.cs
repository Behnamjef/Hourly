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
        private ToDoGroupPlusCell PlusCell => GetCachedComponentInChildren<ToDoGroupPlusCell>();

        private ScrollRect Scroll => GetCachedComponentInChildren<ScrollRect>();
        private Transform Contents => Scroll.content;
        private List<ToDoGroupCell> _groupCells;

        private Data _data;

        public override async Task Init(IPopupData data = null)
        {
            await base.Init(data);
            _data = data as Data;

            PlusCell.Init(new ToDoGroupPlusCell.Data() {OnGroupCreated = AddNewGroupToList});

            ClearList();
            _groupCells = new List<ToDoGroupCell>();

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
            await RebuildAllRects();
        }

        private async Task CreateCell(ToDoGroup doGroup)
        {
            var group = Instantiate(_groupCellPrefab, Contents);
            await group.Init(new ToDoGroupCell.Data() {ToDoGroup = doGroup,OnGroupClicked = _data.OnGroupCellClicked});
            _groupCells.Add(group);
        }

        public async void AddNewGroupToList(string name)
        {
            var group = GroupManager.CreateNewGroup(name);
            await CreateCell(group);
            MovePlusSectionToTheEnd();
            await RebuildAllRects();
        }

        private void MovePlusSectionToTheEnd()
        {
            PlusCell.transform.SetSiblingIndex(_groupCells.Count);
        }

        public void ClearList()
        {
            if (_groupCells.IsNullOrEmpty()) return;

            for (int i = _groupCells.Count - 1; i >= 0; i--)
            {
                Destroy(_groupCells[i].gameObject);
            }

            _groupCells.Clear();
        }


        public class Data : IPopupData
        {
            public List<ToDoGroup> AllGroups;
            public Action<ToDoGroup> OnGroupCellClicked;
        }
    }
}