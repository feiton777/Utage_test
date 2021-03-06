//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using System.IO;
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// セーブデータ
	/// </summary>
	[System.Serializable]
	public class AdvSaveData
	{
		public enum SaveDataType
		{
			Default,
			Quick,
			Auto,
		};

		public SaveDataType Type{ get{ return type;}}
		SaveDataType type;

		/// <summary>
		/// 現在のシナリオラベル
		/// </summary>
		public string CurrentSenarioLabel { get { return this.currentSenarioLabel; } }
		string currentSenarioLabel;

		/// <summary>
		/// 現在のページ
		/// </summary>
		public int CurrentPage { get { return this.currentPage; } }
		int currentPage;

		/// <summary>
		/// 現在の、シーン回想用のシーンラベル
		/// </summary>
		public string CurrentGallerySceneLabel { get { return this.currentGallerySceneLabel; } }
		string currentGallerySceneLabel = "";


		/// <summary>
		/// スプライトの取得
		/// </summary>
		public Sprite GetSprite(float pixelsToUnits)
		{
			if (sprite == null )
			{
				if (Texture == null)
				{
					return null;
				}

				sprite = UtageToolKit.CreateSprite(Texture, pixelsToUnits);
			}
			return sprite;
		}
		Sprite sprite;

		/// <summary>
		/// テクスチャ
		/// </summary>
		public Texture2D Texture{get{	return texture;}}
		Texture2D texture;

		///パラメーターデータ
		byte[] paramBuf;
		//レイヤーデータ
		byte[] layerManagerBuf;
		//サウンドデータ
		byte[] soundManagerBuf;
		//選択肢データ
		byte[] selectionManagerBuf;

		/// <summary>
		/// 日付
		/// </summary>
		public System.DateTime Date { get { return this.date; } }
		System.DateTime date;

		/// <summary>
		/// タイトル
		/// </summary>
		public string Title { get { return this.title; } }
		string title = "";

		/// <summary>
		/// セーブデータのファイルパス
		/// </summary>
		public string Path { get { return this.path; } }
		string path;

		/// <summary>
		/// セーブされているか
		/// </summary>
		public bool IsSaved{get { return !string.IsNullOrEmpty(currentSenarioLabel); }	}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="path">セーブデータのファイルパス</param>
		public AdvSaveData(SaveDataType type, string path)
		{
			this.type = type;
			this.path = path;
		}

		/// <summary>
		/// クリア
		/// </summary>
		public void Clear()
		{
			currentSenarioLabel = "";

			if (texture != null) UnityEngine.Object.Destroy(texture);
			texture = null;

			if (sprite != null) UnityEngine.Object.Destroy(sprite);
			sprite = null;

			paramBuf = null;
			layerManagerBuf = null;
			soundManagerBuf = null;
		}

		/// <summary>
		/// ゲームのデータをセーブ
		/// </summary>
		/// <param name="engine">ADVエンジン</param>
		/// <param name="tex">セーブアイコン</param>
		public void SaveGameData(AdvEngine engine, Texture2D tex)
		{
			Clear();
			title = engine.Page.TextData.ToUnityRitchText();
			currentSenarioLabel = engine.ScenarioPlayer.CurrentScenarioLabel;
			currentPage = engine.ScenarioPlayer.CurrentPage;
			currentGallerySceneLabel = engine.ScenarioPlayer.CurrentGallerySceneLabel;
			paramBuf = engine.Param.ToSaveDataBuffer();
			layerManagerBuf = engine.LayerManager.ToSaveDataBuffer();
			soundManagerBuf = SoundManager.GetInstance().ToSaveDataBuffer();
			selectionManagerBuf = engine.SelectionManager.ToSaveDataBuffer();
			texture = tex;
			date = System.DateTime.Now;
		}

		/// <summary>
		/// ゲームのデータをロード
		/// </summary>
		/// <param name="engine">ADVエンジン</param>
		public void LoadGameData(AdvEngine engine)
		{
			engine.ScenarioPlayer.CurrentGallerySceneLabel = currentGallerySceneLabel;
			engine.Param.ReadSaveDataBuffer(paramBuf);
			engine.LayerManager.ReadSaveDataBuffer(layerManagerBuf);
			engine.SelectionManager.ReadSaveDataBuffer(selectionManagerBuf);
			SoundManager.GetInstance().ReadSaveDataBuffer(soundManagerBuf);
		}

		static readonly int MagicID = FileIOManager.ToMagicID('S', 'a', 'v', 'e');	//識別ID
		const int Version = 2;	//ファイルバージョン
		const int Version1 = 1;

		/// <summary>
		/// バイナリ読み込み
		/// </summary>
		/// <param name="reader"></param>
		public void Read(BinaryReader reader)
		{
			Clear();
			int magicID = reader.ReadInt32();
			if (magicID != MagicID)
			{
				throw new System.Exception("Read File Id Error");
			}

			int fileVersion = reader.ReadInt32();
			if (fileVersion >= Version1)
			{
				title = reader.ReadString();
				date = new System.DateTime(reader.ReadInt64());
				currentSenarioLabel = reader.ReadString();
				currentPage = reader.ReadInt32();
				if (fileVersion > Version1)
				{
					currentGallerySceneLabel = reader.ReadString();
				}

				int captureMemLen = reader.ReadInt32();
				if (captureMemLen > 0)
				{
					byte[] captureMem = reader.ReadBytes(captureMemLen);
					texture = new Texture2D(1, 1, TextureFormat.RGB24, false);
					texture.LoadImage(captureMem);
				}
				else
				{
					texture = null;
				}

				paramBuf = reader.ReadBytes(reader.ReadInt32());
				layerManagerBuf = reader.ReadBytes(reader.ReadInt32());
				soundManagerBuf = reader.ReadBytes(reader.ReadInt32());
				selectionManagerBuf = reader.ReadBytes(reader.ReadInt32());
			}
			else
			{
				throw new System.Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, fileVersion));
			}
		}

		/// <summary>
		/// バイナリ書き込み
		/// </summary>
		/// <param name="writer">バイナリライター</param>
		public void Write(BinaryWriter writer)
		{
			date = System.DateTime.Now;

			writer.Write(MagicID);
			writer.Write(Version);
			writer.Write(title);
			writer.Write(date.Ticks);
			writer.Write(currentSenarioLabel);
			writer.Write(currentPage);
			writer.Write(currentGallerySceneLabel);

			if (texture != null)
			{
				byte[] captureMem = texture.EncodeToPNG();
				writer.Write(captureMem.Length);
				writer.Write(captureMem);
			}
			else
			{
				writer.Write(0);
			}
			writer.Write(paramBuf.Length);
			writer.Write(paramBuf);
			writer.Write(layerManagerBuf.Length);
			writer.Write(layerManagerBuf);
			writer.Write(soundManagerBuf.Length);
			writer.Write(soundManagerBuf);
			writer.Write(selectionManagerBuf.Length);
			writer.Write(selectionManagerBuf);
		}
	}
}