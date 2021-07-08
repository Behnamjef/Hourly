﻿using UnityEngine;
using UnityEditor;

namespace HierarchyDecorator
    {
    internal class LayerInfo : HierarchyInfo
        {
        public LayerInfo(Settings settings) : base(settings)
            {
          
            }

        protected override void DrawInternal(Rect rect, GameObject instance)
            {
            EditorGUI.LabelField (rect, LayerMask.LayerToName(instance.layer), Style.dropdownSmallStyle);

            if (settings.globalSettings.editableLayers)
                {
                Event e = Event.current;
                bool hasClicked = rect.Contains (e.mousePosition) && e.type == EventType.MouseDown;

                if (!hasClicked)
                    return;

                var selection = Selection.gameObjects;

                if (selection.Length < 2)
                    Selection.SetActiveObjectWithContext (instance, null);

                GenericMenu menu = new GenericMenu ();
                bool setChildLayers = settings.globalSettings.applyChildLayers;

                foreach (var layer in Constants.LayerMasks)
                    {
                    int index = LayerMask.NameToLayer (layer);

                    menu.AddItem (new GUIContent (layer), false, () =>
                        {
                        Undo.RecordObjects (Selection.gameObjects, "Layer Updated");
                            
                        foreach (var go in Selection.gameObjects)
                            {
                            go.layer = index;

                            if (setChildLayers)
                                {
                                Undo.RecordObjects (Selection.gameObjects, "Layer Updated");

                                foreach (Transform child in go.transform)
                                    child.gameObject.layer = index;
                                }

                            if (Selection.gameObjects.Length == 1)
                                Selection.SetActiveObjectWithContext (null, null);
                            }
                        });
                    }

                menu.ShowAsContext ();
                e.Use ();
                }
            }

        public override int GetRowSize()
            {
            return 3;
            }

        public override bool CanDisplayInfo()
            {
            return settings.globalSettings.showLayers;
            }

        }
    }
