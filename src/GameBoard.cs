using System;
using SwinGameSDK;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyGame
{
	public class GameBoard
	{
		//==========FIELD============
		private Color _background;
		private List<Obstacle> _obstacles;
		static List<Pattern> debugPattern = new List<Pattern>();

		private bool _spawned;

		private static Random _random = new Random ();
		private int _spawnpoints;
		private static Stopwatch s1 = Stopwatch.StartNew ();
		public static int numberOfObstacles = 1;
		static int [] positionX = { 320, 415, 510 };
		public GameStage _stage;


		const int xMin = 320;
		const int xFullRange = 190;
		const int yMin = 0;
		const int yFullRange = 600;

		//==========CONSTRUCTOR==============
		public GameBoard (Color background)
		{
			_obstacles = new List<Obstacle> ();
			_background = background;

			s1.Reset ();
			s1.Start ();
		}

		public GameBoard () : this (Color.DimGray)
		{ }

		//==========METHODS===========
		public void Draw ()
		{
			//SwinGame.FillRectangleOnScreen (Color.LightGray, 0, 0, 300, 800);
			//SwinGame.FillRectangleOnScreen (Color.LightGray, 600, 0, 300, 800);
			SwinGame.DrawBitmap ("bg2.jpg", 0, 0);
			SwinGame.DrawText ("" + s1.Elapsed.Seconds, Color.White, 300, 300);
		}
		public void GetScore ()
		{
			//foreach (Obstacle c in _obstacles) {
			//	ScoreBoard.Score += 1;
			//}
		}

		public void RandomSpawnVehicle (PlayerVehicle p, Obstacle o)
		{
			_spawnpoints = _random.Next (0, 3);
			o.X = positionX [_spawnpoints];
			o.Y = UtilityFunction.InitialY;
			DifficultyHandler (p,o);
			Obstacles.Add (o);
			o.Draw ();
        }

		void DifficultyHandler (PlayerVehicle p, Obstacle o)
		{
			//difficulty control
			if (UtilityFunction.currentDifficulty.Equals (GameDifficulty.Easy)) {
				o.SpeedY = 200 + (ScoreBoard.Stage - 1)*100;
				o.Acceleration = 0;
			}

			if (UtilityFunction.currentDifficulty.Equals (GameDifficulty.Medium)) {
				o.SpeedY = 200;
				o.Acceleration = 500 + (ScoreBoard.Stage - 1) * 100;
			}

			if (UtilityFunction.currentDifficulty.Equals (GameDifficulty.Hard)) {
				o.SpeedY = 200 + (ScoreBoard.Stage - 1) * 100;
				o.Acceleration = 500 + (ScoreBoard.Stage - 1) * 100;
			}
			if (UtilityFunction.currentDifficulty.Equals (GameDifficulty.Extreme)) {
				o.SpeedY = 400+ (ScoreBoard.Stage - 1) * 100;
				o.SpeedX = 400;
				o.Acceleration = 0 + (ScoreBoard.Stage - 1) * 100;
				int desiredPositionCount = 2;
				int prevX = (int)o.X;
				//debugPattern.Clear ();
				for (int i = 0; i < desiredPositionCount; i++) {
					Pattern pattern = new Pattern ();
					pattern.Y = _random.Next(0, (int)((double)(yFullRange-desiredPositionCount*100) / desiredPositionCount))
						+ ((double)(yFullRange-desiredPositionCount * 100) / desiredPositionCount + 100)*i;
					int xTemp = prevX;
					while(xTemp == prevX)
						xTemp = positionX [(_random.Next (0, 3))];
					pattern.X = xTemp;
					prevX = (int)pattern.X;
					o.PatternQueue.Enqueue(pattern);
				}
			}
			ScoreBoard.Stage = (int)(s1.Elapsed.TotalSeconds / 5) + 1;
			if (ScoreBoard.Stage % 5 == 0) {
				if (_stage == GameStage.NormalStage) {
					Font font = new Font ("Notification", "arial.ttf", 36);
					font.FontStyle = FontStyle.BoldFont;
					s1.Stop ();
					SwinGame.DrawBitmap ("bg2.jpg", 0, 0);
					SwinGame.DrawText ("BONUS STAGE! READY...3", Color.Magenta, "Notification", new Point2D () { X = 200, Y = 300 });
					SwinGame.RefreshScreen (60);
					SwinGame.Delay (1000);
					SwinGame.DrawBitmap ("bg2.jpg", 0, 0);
					SwinGame.DrawText ("BONUS STAGE! READY...2", Color.Magenta, "Notification", new Point2D () { X = 200, Y = 300 });
					SwinGame.RefreshScreen (60);
					SwinGame.Delay (1000);
					SwinGame.DrawBitmap ("bg2.jpg", 0, 0);
					SwinGame.DrawText ("BONUS STAGE! READY...1", Color.Magenta, "Notification", new Point2D () { X = 200, Y = 300 });
					SwinGame.RefreshScreen (60);
					SwinGame.Delay (1000);
					s1.Start ();
					font.Dispose ();
					p.X = GameController.startLane2X;
					p.Y = 600;
				}
				_stage = GameStage.BonusStage;
			}
			else
				_stage = GameStage.NormalStage;
		}

		public void DisplaySpeed()
		{
			if (s1.Elapsed.TotalSeconds > 20 && s1.Elapsed.TotalSeconds <= 40)
			{
				ScoreBoard.Traffic = "Mid-Day";
			}
			else if (s1.Elapsed.TotalSeconds > 40 && s1.Elapsed.TotalSeconds <= 60)
			{
				ScoreBoard.Traffic = "Night Life";
			}
			else
			{
				ScoreBoard.Traffic = "Peak Hours";
			}
		}

		public void ClearScreen()
		{
			_obstacles.Clear();
		}

		public bool GameOver(){
			if (ScoreBoard.Life == 0) return true;
			else return false;
		}

		public void RestartTimer()
		{
			s1.Reset ();
			s1.Start ();
		}


		internal void Check ()
		{
			if (GameOver () == true) {
				UtilityFunction.gameStateStack.Push (GameState.GameOverPage);
			}
		}

		internal void MoveObstacle (PlayerVehicle p)
		{

			for (int i = 1; i < debugPattern.Count; i++) {
				SwinGame.DrawText (debugPattern [i].Y.ToString () + " " + debugPattern [i].X.ToString (),
								  Color.AliceBlue,
								  0, 12 * i);
			}
            for(int i = 0; i < _obstacles.Count; i++)
			{
				_obstacles[i].Drop (p);
				if (_obstacles [i].Y >= 600 || _obstacles [i].Collision (p) == true) {
					if (_obstacles [i].Collision (p) == true) {
						if (_obstacles [i] is Invisible) {
							p.Transparent = true;
							PlayerVehicle.sw.Restart ();
							p.isInvisible = true;
							p.Draw ();
						}

						ScoreBoard.Life += _obstacles [i].LifeReward;

						if (_obstacles [i] is Score) {
							ScoreBoard.Score += 10;
						}

						if (_obstacles [i] is Bomb) {
							ScoreBoard.Life = 0;
							SwinGame.PlaySoundEffect ("Bomb.wav");
							SwinGame.DrawBitmap ("explosion1.png", (float)xMin, (float)yMin);

						}

						if (_obstacles [i] is Turbo) {
							p.speed = true;
							p.SpeedX += 100;
							p.SpeedY += 100;
						}
					}
					if (_obstacles [i].GetObstacleType == ObstacleType.Bomb ||
					    _obstacles [i].GetObstacleType == ObstacleType.Car ||
					    _obstacles [i].GetObstacleType == ObstacleType.Lorry ||
					    _obstacles [i].GetObstacleType == ObstacleType.Motorcycle)
						ScoreBoard.Score += 1;
					_obstacles.Remove (_obstacles [i]);
						i--;
				}
            }
		}

		/// <summary>
		/// Condition checking for valid obstacles generation
		/// </summary>
		public bool ObstacleCondition ()
		{
			if (_obstacles.Count < UtilityFunction.ObstacleLimit) {
				return true;
			}
			else 
			{
				for (int i = 0; i < Obstacles.Count; i++) {
					if (Obstacles [i].Y < 200) return false;
				}
			}
			return true;
		}

		//===============PROPERTIES======================
		public Color BackgroundColor
		{
			get { return _background; }
			set{_background = value; }
		}

		public List<Obstacle> Obstacles
		{
			get{ return _obstacles;}
		}

		public bool Spawned
		{
			get{ return _spawned;}
			set{ _spawned = value;}
		}

		public int Spawnpoints
		{
			get{return _spawnpoints; }
			set{_spawnpoints = value; }
		}

		public GameStage Stage {
			get { return _stage; }
			set { _stage = value; }
		}


	}
}
