//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// コマンド：環境音再生
	/// </summary>
	internal class AdvCommandAmbience : AdvCommand
	{
		public AdvCommandAmbience( StringGridRow row, AdvSettingDataManager dataManager)
		{
			string label = AdvParser.ParseCell<string>(row, AdvColumnName.Arg1);
			if (!dataManager.SoundSetting.Contains(label, SoundType.Ambience))
			{
				Debug.LogError(row.ToErrorString(label + " is not contained in file setting"));
			}

			this.file = AddLoadFile(dataManager.SoundSetting.LabelToFilePath(label, SoundType.Ambience));
			this.isLoop = AdvParser.ParseCellOptional<bool>(row, AdvColumnName.Arg2, false);
		}
		public override void DoCommand(AdvEngine engine)
		{
			engine.SoundManager.Play(SoundManager.StreamType.Ambience, file, isLoop, false);
		}
		AssetFile file;
		bool isLoop;
	}
}