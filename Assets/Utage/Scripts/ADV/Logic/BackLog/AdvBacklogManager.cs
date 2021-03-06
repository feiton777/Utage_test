//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// バックログ管理
	/// </summary>
	[AddComponentMenu("Utage/ADV/Internal/BacklogManager")]
	public class AdvBacklogManager : MonoBehaviour
	{

		/// <summary>
		/// ログの最大数
		/// </summary>
		public int MaxLog { get { return maxLog; } }
		[SerializeField]
		int maxLog = 10;

		/// <summary>
		/// バックログデータのリスト
		/// </summary>
		/// <returns></returns>
		public List<AdvBacklog> Backlogs{ get { return backlogs; }}
		List<AdvBacklog> backlogs = new List<AdvBacklog>();

		/// <summary>
		/// クリア処理
		/// </summary>
		public void Clear()
		{
			backlogs.Clear();
		}

		/// <summary>
		/// ボイスの再生
		/// </summary>
		/// <param name="index">バックログのインデックス</param>
		/// <returns>成否</returns>

		public bool TryPlayVoice(AdvEngine engine, int index)
		{
			AdvBacklog backlog = Backlogs[index];
			if (backlog.IsVoice)
			{
				StartCoroutine(CoPlayVoice(engine,backlog.VoiceFile));
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// バックログ追加
		/// </summary>
		/// <param name="text">テキスト</param>
		/// <param name="characteName">キャラ名</param>
		/// <param name="voiceFile">ボイスファイル(nullでも良い)</param>

		public void Add(string text, string characteName, AssetFile voiceFile)
		{
			backlogs.Add(new AdvBacklog(text, characteName, voiceFile));
			if (backlogs.Count > MaxLog)
			{
				backlogs.RemoveAt(0);
			}
		}
	
		/// <summary>
		/// バックログ追加
		/// </summary>
		/// <param name="text">テキスト</param>
		public void Add(string text)
		{
			Add(text, "", null);
		}


		//ボイスの再生
		IEnumerator CoPlayVoice( AdvEngine engine, AssetFile voiceFile)
		{
			if (voiceFile == null)
			{
				Debug.LogError("Backlog voiceFile is NULL");
			}
			AssetFileManager.Load(voiceFile, this);
			while (!voiceFile.IsReadyStreaming)
			{
				yield return 0;
			}
			engine.SoundManager.Play(SoundManager.StreamType.Voice, voiceFile, false, true);
			voiceFile.Unuse(this);
		}
	}
}
