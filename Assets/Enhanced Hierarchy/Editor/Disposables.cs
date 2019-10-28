﻿using System;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace EnhancedHierarchy {

    internal struct GUIBackgroundColor : IDisposable {
        private readonly Color before;

        public GUIBackgroundColor(Color color) {
            before = GUI.backgroundColor;
            GUI.backgroundColor = color;
        }

        public void Dispose() {
            GUI.backgroundColor = before;
        }
    }

    internal struct GUIContentColor : IDisposable {
        private readonly Color before;

        public GUIContentColor(Color color) {
            before = GUI.contentColor;
            GUI.contentColor = color;
        }

        public void Dispose() {
            GUI.contentColor = before;
        }
    }

    internal struct GUIColor : IDisposable {
        private readonly Color before;

        public GUIColor(Color color) {
            before = GUI.color;
            GUI.color = color;
        }

        public void Dispose() {
            GUI.color = before;
        }
    }

    internal sealed class GUIIndent : IDisposable {
        public GUIIndent() {
            EditorGUI.indentLevel++;
        }

        public GUIIndent(string label) {
            EditorGUILayout.LabelField(label);
            EditorGUI.indentLevel++;
        }

        public void Dispose() {
            EditorGUI.indentLevel--;
            EditorGUILayout.Separator();
        }
    }

    internal struct GUIEnabled : IDisposable {
        private readonly bool before;

        public GUIEnabled(bool enabled) {
            before = GUI.enabled;
            GUI.enabled = before && enabled;
        }

        public void Dispose() {
            GUI.enabled = before;
        }
    }

    internal sealed class GUIFade : IDisposable {
        private AnimBool anim;

        public bool Visible { get; private set; }

        public GUIFade() {
            Visible = true;
        }

        public void SetTarget(bool target) {
            if(anim == null) {
                anim = new AnimBool(target);
                anim.valueChanged.AddListener(() => {
                    if(EditorWindow.focusedWindow)
                        EditorWindow.focusedWindow.Repaint();
                });
            }

            anim.target = target;
            Visible = EditorGUILayout.BeginFadeGroup(anim.faded);
        }

        public void Dispose() {
            EditorGUILayout.EndFadeGroup();
        }
    }

}