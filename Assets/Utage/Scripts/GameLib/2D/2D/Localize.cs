//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// 表示言語切り替え用のクラス
	/// </summary>
	[ExecuteInEditMode]
	[AddComponentMenu("Utage/Lib/2D/Localize")]
	public class Localize : MonoBehaviour
	{
		public string Key
		{
			set { key = value; ForceRefresh(); }
			get { return key; }
		}
		[SerializeField]
		string key;

		string currentLanguage;

		/// <summary>
		/// スプライトコンポーネント(アタッチされてない場合はnull)
		/// </summary>
		TextArea2D CachedTextArea { get { if (null == cachedTextArea) cachedTextArea = this.GetComponent<TextArea2D>(); return cachedTextArea; } }
		TextArea2D cachedTextArea;


		public void OnLocalize() { Refresh(); }
		void OnEnable() { Refresh(); }
		void Start(){ Refresh(); }
		void OnValidate()
		{
			ForceRefresh();
		}

		void ForceRefresh()
		{
			currentLanguage = "";
			Refresh();
		}

		void Refresh()
		{
			if (string.IsNullOrEmpty(Key)) return;
			if (CustomProjectSetting.Instance == null) return;

			LanguageManagerBase langManager = LanguageManagerBase.Instance;
			if (langManager==null) return;
			if (!langManager.IsInit) return;
			if (currentLanguage == langManager.CurrentLanguage) return;

			TextArea2D text = CachedTextArea;
			if (text!=null) text.text = langManager.LocalizeText(key);
			currentLanguage = langManager.CurrentLanguage;
		}
	}
}

