﻿using System;
using SwinGameSDK;
namespace MyGame
{
	public class MainMenuController:Page
	{
		const int ButtonX = 50;
		const int ButtonY = 150;
		const int Spacing = 5;
		const int ButtonWidth = 179;
		const int ButtonHeight = 45;
		string [] menu = {
			"Play",
			"High Score",
			"Setting",
			"Instruction",
			"Vehicle"
		};

		public override void DrawPage ()
		{
			SwinGame.DrawBitmap ("bg2.jpg", 0, 0);

			for(int i = 0; i < menu.Length ; i++) {
				SwinGame.FillRectangle (Color.Transparent, ButtonX, ButtonY + (Spacing + ButtonHeight) * i, ButtonWidth, ButtonHeight);
				SwinGame.DrawBitmap ("play_button.png", ButtonX, ButtonY);
				SwinGame.DrawBitmap ("high.png", ButtonX, ButtonY + 51);
				SwinGame.DrawBitmap ("setting.png", ButtonX, ButtonY + 101);
				SwinGame.DrawBitmap ("instruction.png", ButtonX, ButtonY + 151);
				SwinGame.DrawBitmap ("vehicle.png", ButtonX, ButtonY + 201);
				if (UtilityFunction.IsMouseInRectangle (ButtonX, ButtonY + (Spacing + ButtonHeight) * i, ButtonWidth, ButtonHeight, SwinGame.MousePosition ())) {
					if (SwinGame.MouseDown (MouseButton.LeftButton))
						SwinGame.FillRectangle(Color.LimeGreen, ButtonX, ButtonY + (Spacing + ButtonHeight) * i, ButtonWidth, ButtonHeight);
					else
						SwinGame.DrawRectangle (Color.Azure, ButtonX, ButtonY + (Spacing + ButtonHeight) * i, ButtonWidth, ButtonHeight);
				}
				//SwinGame.DrawText (menu [i], Color.Black, ButtonX + 10 * Spacing, ButtonY + ButtonHeight / 2 + (Spacing + ButtonHeight) * i);
			}
		}

		public override void Execute ()
		{
			HandleInput ();
			DrawPage ();
		}

		public override void HandleInput ()
		{
			Point2D clicked = SwinGame.MousePosition ();
			if(GameState.ViewingMainPage == UtilityFunction.gameStateStack.Peek())
				HandleMenuInput ();
		}

		private void HandleMenuInput ()
		{
			for (int i = 0; i < menu.Length; i++) {
				if (SwinGame.MouseClicked (MouseButton.LeftButton)
				   && UtilityFunction.IsMouseInRectangle (ButtonX, ButtonY + (Spacing + ButtonHeight) * i,
														   ButtonWidth, ButtonHeight, SwinGame.MousePosition ()))
					PerformMenuAction (i);
			}
		}

		void PerformMenuAction (int button)
		{
			switch (button) {
			case 2:
				UtilityFunction.gameStateStack.Push (GameState.ViewingSettingPage);
				break;
			case 3:
				UtilityFunction.gameStateStack.Push (GameState.Instruction);
				break;
			case 5:
				UtilityFunction.gameStateStack.Push (GameState.ViewingCarSelection);
				break;
			default:
				UtilityFunction.gameStateStack.Push (GameState.ViewingGamePage);
				break;
			}
		}
}
}
