//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// 選択肢管理
	/// </summary>
	[AddComponentMenu("Utage/ADV/Internal/SelectionManager")]
	public class AdvSelectionManager : MonoBehaviour
	{
		/// <summary>
		/// 選択肢データ
		/// </summary>
		public List<AdvSelection> Selections { get { return selections; } }
		List<AdvSelection> selections = new List<AdvSelection>();			//表示スタイル

		/// <summary>
		/// 選択待ち状態か
		/// </summary>
		public bool IsWaitSelect { get { return this.isWaitSelect; } }
		bool isWaitSelect = false;

		/// <summary>
		/// 選択されたデータ
		/// </summary>
		public AdvSelection Selected
		{
			get { return selected; }
		}
		AdvSelection selected = null;

		/// <summary>
		/// 選択
		/// </summary>
		/// <param name="index">選択肢のインデックス</param>
		public void Select(int index)
		{
			selected = selections[index];
			isWaitSelect = false;
		}

		/// <summary>
		/// 選択肢追加
		/// </summary>
		/// <param name="label">ジャンプ先のラベル</param>
		/// <param name="text">表示テキスト</param>
		/// <param name="exp">選択時に実行する演算式</param>
		public void AddSelection(string label, string text, ExpressionParser exp)
		{
			selections.Add(new AdvSelection(label, text, exp));
		}

		/// <summary>
		/// 選択の入力待ち開始
		/// </summary>
		public void StartWaiting()
		{
			isWaitSelect = true;
			selected = null;
		}

		/// <summary>
		/// 選択肢データをクリア
		/// </summary>
		public void Clear()
		{
			isWaitSelect = false;
			selected = null;
			selections.Clear();
		}

		/// <summary>
		/// セーブデータ用のバイナリを取得
		/// </summary>
		/// <returns>セーブデータのバイナリ</returns>
		public byte[] ToSaveDataBuffer()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				//バイナリ化
				using (BinaryWriter writer = new BinaryWriter(stream))
				{
					Write(writer);
				}
				return stream.ToArray();
			}
		}

		/// <summary>
		/// セーブデータを読みこみ
		/// </summary>
		/// <param name="buffer">セーブデータのバイナリ</param>
		public void ReadSaveDataBuffer(byte[] buffer)
		{
			using (MemoryStream stream = new MemoryStream(buffer))
			{
				using (BinaryReader reader = new BinaryReader(stream))
				{
					Read(reader);
				}
			}
		}

		const int VERSION = 0;
		//バイナリ書き込み
		void Write(BinaryWriter writer)
		{
			writer.Write(VERSION);
			writer.Write(Selections.Count);
			foreach (var selection in Selections)
			{
				selection.Write(writer);
			}
		}
		//バイナリ読み込み
		void Read(BinaryReader reader)
		{
			int version = reader.ReadInt32();
			if (version == VERSION)
			{
				this.Clear();
				int count = reader.ReadInt32();
				for (int i = 0; i < count; i++)
				{
					AdvSelection selection = new AdvSelection(reader);
					selections.Add(selection );
				}
			}
			else
			{
				Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, version));
			}
		}
	}
}