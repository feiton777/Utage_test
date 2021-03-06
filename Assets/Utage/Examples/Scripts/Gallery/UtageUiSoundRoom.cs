//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

/// <summary>
/// サウンドルーム画面のサンプル
/// </summary>
[AddComponentMenu("Utage/Examples/SoundRoom")]
public class UtageUiSoundRoom : UtageUiView
{
	/// <summary>
	/// リストビュー
	/// </summary>
	public ListView listView;

	/// <summary>
	/// リストビューアイテムのリスト
	/// </summary>
	List<AdvSoundSettingData> itemDataList = new List<AdvSoundSettingData>();

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
		isInit = false;
		this.listView.Close();
		Engine.SoundManager.StopAll(0.2f);
	}

	//起動待ちしてから開く
	IEnumerator CoWaitOpen()
	{
		while (Engine.IsWaitBootLoading)
		{
			yield return 0;
		}

		itemDataList = Engine.DataManager.SettingDataManager.SoundSetting.GetSoundRoomList();
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
		UtageUiSoundRoomItem item = go.GetComponent<UtageUiSoundRoomItem>();
		AdvSoundSettingData data = itemDataList[index];
		item.Init(data, index );
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
		AdvSoundSettingData data = itemDataList[button.Index];
		string path = Engine.DataManager.SettingDataManager.SoundSetting.LabelToFilePath(data.Key, SoundType.Bgm);

		StartCoroutine( CoPlaySound(path) );
	}

	//サウンドをロードして鳴らす
	IEnumerator CoPlaySound(string path)
	{
		AssetFile file = AssetFileManager.Load(path,this);
		while (!file.IsLoadEnd) yield return 0;
		Engine.SoundManager.Play( SoundManager.StreamType.Bgm, file, true, false );
		file.Unuse(this);
	}
}
