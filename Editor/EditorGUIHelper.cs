using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor { 
    public static class EditorHelper {
        static string nullString = null;

	static public int DrawToolbar(int index, GUIContent[] buttons) {
		EditorGUILayout.BeginVertical();
		GUILayout.Space(3f);

		EditorGUILayout.BeginHorizontal("Toolbar", GUILayout.ExpandWidth(true));

		for (int i = 0; i < buttons.Length; i++)
		{
			GUILayout.Button(buttons[i], "toolbarButton");
		}

		EditorGUILayout.EndHorizontal();

		GUILayout.Space(18f);

		EditorGUILayout.EndVertical();
		return 0;
	}

	static public bool DrawHeader(string text, string key, bool forceOn, ref string filter)
	{
		bool state = EditorPrefs.GetBool(key, true);


		GUILayout.Space(1f);
		if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
		GUILayout.BeginHorizontal();
		GUILayout.Space(-8f);
		GUILayout.BeginHorizontal("pretoolbar", GUILayout.ExpandHeight(true));
		GUILayout.Space(-5f);

		GUI.changed = false;
		if (state) text = "\u25B2 " + text;
		else text = "\u25BC " + text;
		if (!GUILayout.Toggle(true, text, "pretoolbar", GUILayout.MinWidth(20f))) state = !state;
		if (GUI.changed) EditorPrefs.SetBool(key, state);

		GUILayout.FlexibleSpace ();

		if (filter == null) {
			filter = EditorPrefs.GetString (key + "-filter", "");
			filter = EditorGUILayout.TextField ("", filter, "ToolbarSeachTextField", GUILayout.MaxWidth (128));
			if (GUILayout.Button ("", "ToolbarSeachCancelButton")) {
				filter = "";
			}
			EditorPrefs.SetString (key + "-filter", filter);
		}

		GUILayout.EndHorizontal();
		GUILayout.EndHorizontal();
		GUI.backgroundColor = Color.white;
		if (!forceOn && !state) GUILayout.Space(2f);
		return state;
	}

	static public bool DrawHeader(string title) { return DrawHeader(title, title, true, ref nullString); }

	static public bool DrawHeaderFilter(string title, ref string filter) { return DrawHeader(title, title, true, ref filter); }

	static public void BeginContents()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(-9f);
		EditorGUILayout.BeginHorizontal("flow overlay box", GUILayout.MinHeight(10f));
		GUI.contentColor = Color.white;
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
	}

	static public void BeginExpandContents()
	{
		GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
		GUILayout.Space(4f);
		EditorGUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
	}

	static public void EndContents()
	{
		GUILayout.Space(3f);
		GUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(-1f);
		GUILayout.EndHorizontal();
		GUILayout.Space(3f);
	}

	static public void DrawSeparator()
	{
		GUILayout.Space(12f);

		if (Event.current.type == EventType.Repaint)
		{
			Texture2D tex = EditorGUIUtility.whiteTexture;
			Rect rect = GUILayoutUtility.GetLastRect();
			GUI.color = new Color(0f, 0f, 0f, 0.25f);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 4f), tex);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 1f), tex);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 9f, Screen.width, 1f), tex);
			GUI.color = Color.white;
		}
	}

	static public void DrawSeparatorBlank()
	{
		GUILayout.Space(4f);

		if (Event.current.type == EventType.Repaint)
		{
			Texture2D tex = EditorGUIUtility.whiteTexture;
			Rect rect = GUILayoutUtility.GetLastRect();
			GUI.color = new Color(0f, 0f, 0f, 0.25f);
			GUI.DrawTexture(new Rect(0f, rect.yMin, Screen.width, 4f), tex);
			GUI.DrawTexture(new Rect(0f, rect.yMin, Screen.width, 1f), tex);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 3f, Screen.width, 1f), tex);
			GUI.color = Color.white;
		}
	}

	static public bool DrawToggleHeader(bool enable, string title, Color color, Color content, int paddindTop, int paddingBottom, int paddingLeft, int paddingRight)
	{
		Color oldColor = GUI.backgroundColor;
		Color oldContent = GUI.contentColor;
		GUI.backgroundColor = color;
		GUI.contentColor = content;
		EditorGUILayout.BeginVertical();
		GUILayout.Space(paddindTop);
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(paddingLeft);
		EditorGUILayout.BeginHorizontal("AS TextArea");
		string text = "<b><size=11>" + title + "</size></b>";
		GUILayout.Label(text, "dragtab");
		bool result = EditorGUILayout.Toggle(enable, GUILayout.Width(13));
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(paddingRight);
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(paddingBottom);
		EditorGUILayout.EndVertical();
		GUI.backgroundColor = oldColor;
		GUI.contentColor = oldContent;
		return result;
	}

	static public void BeginContents(bool hasScroll, ref Vector2 scrollPos, Color color, int paddindTop, int paddingLeft)
	{
		GUI.backgroundColor = color;
		EditorGUILayout.BeginVertical();
		GUILayout.Space(paddindTop);
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(paddingLeft);
		EditorGUILayout.BeginVertical("AS TextArea");
		scrollPos = hasScroll ? Vector2.zero : EditorGUILayout.BeginScrollView(scrollPos);
	}

	static public void EndContents(bool wrapContents, int paddingBottom, int paddingRight)
	{
		if (!wrapContents) GUILayout.FlexibleSpace();
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		GUILayout.Space(paddingRight);
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(paddingBottom);
		EditorGUILayout.EndVertical();
	}

	static public bool DrawToggleHeader(bool enable, string title, Color color, Color content)
	{
		return DrawToggleHeader(enable, title, color, content, 2, 0, 2, 2);
	}

	public static T[] GetAssetsOfType<T>() where T : UnityEngine.Object
	{
		List<T> tempObjects = new List<T>();

		var paths = AssetDatabase.GetAllAssetPaths();

		T tempGO;
		foreach (string fileName in paths)
		{
			tempGO = AssetDatabase.LoadAssetAtPath(fileName, typeof(T)) as T;
			if (tempGO == null)
			{
				continue;
			}
			else if (!(tempGO is T))
			{
				continue;
			}

			tempObjects.Add(tempGO);
		}

		return tempObjects.ToArray();
	}

	public static T[] GetSelectedOfType<T>() where T : UnityEngine.Object
	{
		List<T> listTmp = new List<T>();
		foreach (Object obj in Selection.objects)
		{
			if (obj is T) listTmp.Add(obj as T);
		}
		return listTmp.ToArray();
	}

	public static void DrawTexture(Texture2D tex, int size)
	{
		if (Event.current.type == EventType.Repaint)
		{
			Rect r = GUILayoutUtility.GetLastRect();
			Rect drawRect = new Rect(r.x + 5, r.y + r.height, size, size);

			EditorGUI.DrawRect(new Rect(drawRect.x - 1, drawRect.y - 1, drawRect.width + 2, drawRect.height + 2), Color.black);

			if (tex == null)
			{
				EditorGUI.DrawPreviewTexture(drawRect, EditorGUIUtility.whiteTexture);
			}
			else {
				EditorGUI.DrawPreviewTexture(drawRect, tex);
			}
		}
		GUILayout.Space(size + 5);
	}

	public static void DrawTexture(Texture2D tex, Rect rect, Rect uv, Color color, Material mat)
	{
		int w = Mathf.RoundToInt(tex.width * uv.width);
		int h = Mathf.RoundToInt(tex.height * uv.height);

		// Create the texture rectangle that is centered inside rect.
		Rect outerRect = rect;
		outerRect.width = w;
		outerRect.height = h;

		if (outerRect.width > 0f)
		{
			float f = rect.width / outerRect.width;
			outerRect.width *= f;
			outerRect.height *= f;
		}

		if (rect.height > outerRect.height)
		{
			outerRect.y += (rect.height - outerRect.height) * 0.5f;
		}
		else if (outerRect.height > rect.height)
		{
			float f = rect.height / outerRect.height;
			outerRect.width *= f;
			outerRect.height *= f;
		}

		if (rect.width > outerRect.width) outerRect.x += (rect.width - outerRect.width) * 0.5f;

		// Draw the background
		//NGUIEditorTools.DrawTiledTexture(outerRect, NGUIEditorTools.backdropTexture);

		// Draw the sprite
		GUI.color = color;

		if (mat == null)
		{
			GUI.DrawTextureWithTexCoords(outerRect, tex, uv, true);
		}
		else
		{
			// NOTE: There is an issue in Unity that prevents it from clipping the drawn preview
			// using BeginGroup/EndGroup, and there is no way to specify a UV rect... le'suq.
			UnityEditor.EditorGUI.DrawPreviewTexture(outerRect, tex, mat);
		}
		GUI.color = Color.white;

		// Draw the lines around the sprite
		Handles.color = Color.black;
		Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMin), new Vector3(outerRect.xMin, outerRect.yMax));
		Handles.DrawLine(new Vector3(outerRect.xMax, outerRect.yMin), new Vector3(outerRect.xMax, outerRect.yMax));
		Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMin), new Vector3(outerRect.xMax, outerRect.yMin));
		Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMax), new Vector3(outerRect.xMax, outerRect.yMax));

		// Sprite size label
		string text = string.Format("Texture Size: {0}x{1}", w, h);
		EditorGUI.DropShadowLabel(GUILayoutUtility.GetRect(Screen.width, 18f), text);
	}

    }
}