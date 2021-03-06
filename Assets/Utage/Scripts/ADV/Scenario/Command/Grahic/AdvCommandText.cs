//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// コマンド：テキスト表示（地の文）
	/// </summary>
	internal class AdvCommandText : AdvCommand
	{

		public AdvCommandText(StringGridRow row)
		{
			this.text = AdvParser.ParseCell<string>(row, AdvColumnName.Text);
			if (AdvCommand.IsEditorErrorCheck)
			{
				TextData textData = new TextData(text);
				if (!string.IsNullOrEmpty(textData.ErrorMsg))
				{
					Debug.LogError(row.ToErrorString(textData.ErrorMsg));
				}
			}
		}

		public override void DoCommand(AdvEngine engine)
		{
			engine.Page.SetText(text);
			if (text.Length > 0) engine.BacklogManager.Add(text);
		}

		public override bool Wait(AdvEngine engine)
		{
			return engine.Page.IsShowingText;
		}

		//ページ区切りのコマンドか
		public override bool IsTypePageEnd() { return true; }

		string text;
	}
}