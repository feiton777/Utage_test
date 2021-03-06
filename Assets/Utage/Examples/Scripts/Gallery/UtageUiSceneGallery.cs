//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

/// <summary>
/// CGギャラリー画面のサンプル
/// </summary>
[AddComponentMenu("Utage/Examples/SceneGallery")]
public class UtageUiSceneGallery : UtageUiView
{
	/// <summary>
	/// メインゲーム画面
	/// </summary>
	public UtageUiMainGame mainGame;

	/// <summary>
	/// リストビュー
	/// </summary>
	public ListView listView;

	/// <summary>
	/// リストビューアイテムのリスト
	/// </summary>
	List<AdvSceneGallerySettingData> itemDataList = new List<AdvSceneGallerySettingData>();

	/// <summary>ADVエンジン</summary>
	public AdvEngine Engine { get { return this.engine ?? (this.engine = FindObjectOfType<AdvEngine>() as AdvEngine); } }
	[SerializeField]
	AdvEngine engine;

	bool isInit = false;


	/// <summary>
	/// オープンしたときに呼ばれる
	/// </summary>
	void OnOpen()
	{
		isInit = false;
		this.listView.Close();	///いったん閉じる
		StartCoroutine(CoWaitOpen());
	}

	/// <summary>
	/// クローズしたときに呼ばれる
	/// </summary>
	void OnClose()
	{
		this.listView.Close();
	}

	//起動待ちしてから開く
	IEnumerator CoWaitOpen()
	{
		while (Engine.IsWaitBootLoading)
		{
			yield return 0;
		}

		itemDataList = Engine.DataManager.SettingDataManager.SceneGallerySetting.List;
		listView.Open(itemDataList.Count, CallBackCreateItem);
		isInit = true;
	}


	/// <summary>
	/// リストビューのアイテムが作成されるときに呼ばれるコールバック
	/// </summary>
	/// <param name="go">作成されたアイテムのGameObject</param>
	/// <param name="index">作成されたアイテムのインデックス</param>
	void CallBackCreateItem(GameObject go, int index)
	{
		UtageUiSceneGalleryItem item = go.GetComponent<UtageUiSceneGalleryItem>();
		AdvSceneGallerySettingData data = itemDataList[index];
		item.Init(data, index, Engine.SystemSaveData );
		Button button = go.GetComponent<Button>();
		button.Target = this.gameObject;
	}

	void Update()
	{
		//右クリックで戻る
		if (isInit && InputUtil.IsMousceRightButtonDown())
		{
			Back();
		}
	}

	/// <summary>
	/// 各アイテムが押された
	/// </summary>
	/// <param name="button">押されたアイテム</param>
	void OnTap(Button button)
	{
		Close();
		mainGame.OpenSceneGallery(itemDataList[button.Index].ScenarioLabel);
	}
}
