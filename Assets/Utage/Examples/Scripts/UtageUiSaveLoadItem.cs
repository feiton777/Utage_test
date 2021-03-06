//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;
using Utage;



/// <summary>
/// セーブロード用のUIのサンプル
/// </summary>
[AddComponentMenu("Utage/Examples/SaveLoadItem")]
public class UtageUiSaveLoadItem : MonoBehaviour
{
	/// <summary>本文</summary>
	public TextArea2D text;

	/// <summary>セーブ番号</summary>
	public TextArea2D no;

	/// <summary>日付</summary>
	public TextArea2D date;

	/// <summary>スクショ</summary>
	public Sprite2D texture;

	/// <summary>未セーブだった場合に表示するテキスト</summary>
	public string textEmpty = "Empty";

	[SerializeField]
	float pixcelsToUnits = 100;

	/// <summary>
	/// 初期化
	/// </summary>
	/// <param name="data">セーブデータ</param>
	/// <param name="index">インデックス</param>
	/// <param name="isSave">セーブ画面用ならtrue、ロード画面用ならfalse</param>
	public void Init(AdvSaveData data, int index, bool isSave)
	{

		ListViewItem listViewItem = this.GetComponent<ListViewItem>();

		no.text = string.Format("No.{0,3}", index);
		if (data.IsSaved)
		{
			if (data.Type != AdvSaveData.SaveDataType.Auto)
			{	//オートセーブにはテクスチャがない
				texture.Sprite = data.GetSprite(pixcelsToUnits);
			}
			text.text = data.Title;
			date.text = UtageToolKit.DateToStringJp(data.Date);
			listViewItem.IsEnableButton = true;
		}
		else
		{
			text.text = textEmpty;
			date.text = "";
			listViewItem.IsEnableButton = isSave;
		}

		//オートセーブデータ
		if (data.Type == AdvSaveData.SaveDataType.Auto )
		{
			no.text = "Auto";
			//セーブはできない
			if (isSave)
			{
				listViewItem.IsEnableButton = false;
			}
		}
	}
}
