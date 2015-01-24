using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{
	[CommandInfo("Dialog", 
	             "Say", 
	             "Writes a line of story text to a Say Dialog. " +
	             "Select [Game Object > Fungus > Dialog > Say Dialog] to create a new Say Dialog in your scene. " + 
	             "Select [Game Object > Fungus > Dialog > Character] to create a new selectable speaking character.")]
	public class Say : Command 
	{
		[Tooltip("Story text to display to the player")]
		[TextArea(5,10)]
		public string storyText;

		[Tooltip("Speaking character to use when writing the story text")]
		public Character character;

		[Tooltip("Say Dialog to use when writing the story text.")]
		public SayDialog sayDialog;

		[Tooltip("Voiceover audio to play when writing the story text")]
		public AudioClip voiceOverClip;

		[Tooltip("Only show this text once, even if the command is executed again")]
		public bool showOnce;

		protected int executionCount;

		protected bool showBasicGUI;

		public override void OnEnter()
		{
			if (showOnce && executionCount > 0)
			{
				Continue();
				return;
			}
			
			executionCount++;

			showBasicGUI = false;
			if (sayDialog == null)
			{
				// Try to get any SayDialog in the scene
				sayDialog = GameObject.FindObjectOfType<SayDialog>();
				if (sayDialog == null)
				{
					showBasicGUI = true;
					return;
				}
			}
	
			sayDialog.SetCharacter(character);

			sayDialog.ShowDialog(true);

			if (voiceOverClip != null)
			{
				MusicController.GetInstance().PlaySound(voiceOverClip, 1f);
			}

			sayDialog.Say(storyText, delegate {
				sayDialog.ShowDialog(false);
				Continue();
			});
		}

		public override string GetSummary()
		{
			return "\"" + storyText + "\"";
		}

		protected virtual void OnGUI()
		{
			if (!showBasicGUI)
			{
				return;
			}

			// Draw a basic GUI to use when no uGUI dialog has been set
			// Does not support drawing character images

			GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
			GUILayout.FlexibleSpace();

			GUILayout.BeginVertical(GUILayout.Height(Screen.height));
			GUILayout.FlexibleSpace();

			GUILayout.BeginVertical(new GUIStyle(GUI.skin.box));

			if (character != null)
			{
				GUILayout.Label(character.nameText);
				GUILayout.Space(10);
			}

			GUILayout.Label(storyText);
			if (GUILayout.Button("Continue"))
			{
				showBasicGUI = false;
				Continue();
			}

			GUILayout.EndVertical();

			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		public override Color GetButtonColor()
		{
			return new Color32(184, 210, 235, 255);
		}

		public override void OnCommandAdded(Sequence parentSequence)
		{
			// Find last Say command in the sequence, then copy the Say dialog it's using.
			// This saves a step when adding a new Say command
			for (int i = parentSequence.commandList.Count - 1; i >= 0; --i) 
			{
				Say sayCommand = parentSequence.commandList[i] as Say;
				if (sayCommand != null)
				{
					if (sayCommand.sayDialog != null)
					{
						sayDialog = sayCommand.sayDialog;
						break;
					}
				}
			}
		}

		public override void OnReset()
		{
			executionCount = 0;
		}
	}

}