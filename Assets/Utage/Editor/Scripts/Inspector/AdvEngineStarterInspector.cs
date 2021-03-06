//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Utage
{
	[CustomEditor(typeof(AdvEngineStarter))]
	public class AdvEngineStarterInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			DrawProperties();
			serializedObject.ApplyModifiedProperties();
		}

		void DrawProperties()
		{
			AdvEngineStarter obj = target as AdvEngineStarter;

			UtageEditorToolKit.PropertyField(serializedObject, "engine", "Engine");
			UtageEditorToolKit.PropertyField(serializedObject, "isAutomaticPlay", "Is Automatic Play");
			UtageEditorToolKit.PropertyField(serializedObject, "startScenario", "Start Scenario Label");

			//シナリオデータ
			UtageEditorToolKit.BeginGroup("Scenario Data");
			UtageEditorToolKit.PropertyField(serializedObject, "scenarioDataLoadType", "LoadType");
			switch( obj.ScenarioDataLoadType )
			{
				case AdvEngineStarter.LoadType.Local:
					UtageEditorToolKit.PropertyField(serializedObject, "settingDataManager", "Setting Data Manager");
					UtageEditorToolKit.PropertyFieldArray(serializedObject, "exportedScenarioDataTbl", "Exported Scenario Data Tbl");
					break;
				case AdvEngineStarter.LoadType.Server:
					UtageEditorToolKit.PropertyField(serializedObject, "urlScenarioData", "URL Scenario Data");
					UtageEditorToolKit.PropertyField(serializedObject, "scenarioVersion", "Boot File Version");
					break;
			}
			UtageEditorToolKit.EndGroup();


			//リソースデータ
			UtageEditorToolKit.BeginGroup("Resource Data");
			UtageEditorToolKit.PropertyField(serializedObject, "resourceLoadType", "LoadType");
			switch (obj.ResourceLoadType)
			{
				case AdvEngineStarter.LoadType.Local:
					UtageEditorToolKit.PropertyField(serializedObject, "rootResourceDir", "Root Dir");
					break;
				case AdvEngineStarter.LoadType.Server:
					UtageEditorToolKit.PropertyField(serializedObject, "urlResourceDir", "URL Resource Dir");
					break;
			}
			UtageEditorToolKit.EndGroup();
		}
	}
}

 