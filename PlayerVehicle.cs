using System;
using SwinGameSDK;
using System.Diagnostics;

namespace MyGame
{
	public class PlayerVehicle
	{
		double _x, _y, _acc, _spdX, _spdY;

		public DateTime _prevTime;
		public DateTime _curTime;
		public bool transparent;
		public static Stopwatch sw = new Stopwatch ();
		public bool speed;
		public bool isInvisible;

		public PlayerVehicle (double x, double y)
		{
			_x = x;
			_y = y;
			_spdX = 500;
			_spdY = 500;
			_acc = 1000;
			_prevTime = DateTime.Now;
			_curTime = DateTime.Now;
		}

		public void UpdateTime ()
		{
			_prevTime = _curTime;
			_curTime = DateTime.Now;
		}

		/// <summary>
		/// Execute update time method before using this.
		/// </summary>
		public void NavigateUp ()
		{
			if (Y > 90) {
				_spdY += _curTime.Subtract (_prevTime).TotalMilliseconds / 1000 * _acc;
				Y -= _curTime.Subtract (_prevTime).TotalMilliseconds / 1000 * SpeedY;
			}
		}

		/// <summary>
		/// Execute update time method before using this.
		/// </summary>
		public void NavigateDown ()
		{
			if (Y < 530) {
				_spdY += _curTime.Subtract (_prevTime).TotalMilliseconds / 1000 * _acc;
				Y += _curTime.Subtract (_prevTime).TotalMilliseconds / 1000 * SpeedY;
			}
		}
		/// <summary>
		/// Execute update time method before using this.
		/// </summary>
		public void NavigateLeft ()
		{
			_spdX += _curTime.Subtract (_prevTime).TotalMilliseconds / 1000 * _acc;
			X -= _curTime.Subtract (_prevTime).TotalMilliseconds / 1000 * SpeedX;
			if (X < GameController.startLane1X - 20) {
				X = GameController.startLane1X - 20;
			}
		}

		/// <summary>
		/// Execute update time method before using this.
		/// </summary>
		public void NavigateRight ()
		{
			_spdX += _curTime.Subtract (_prevTime).TotalMilliseconds / 1000 * _acc;
			X += _curTime.Subtract (_prevTime).TotalMilliseconds / 1000 * SpeedX;
			if (X > GameController.startLane3X + 20) {
				X = GameController.startLane3X + 20;
			}
		}

		public void NavigateSpeed ()
		{
			if (speed == true) {
				if (sw.ElapsedMilliseconds <= 3000) {
					_spdX = 125;
					_spdY = 125;
					_acc = 50;
				} else {
					sw.Stop ();
					speed = false;
				}
			} else {
				_spdX = 100;
				_spdY = 100;
				_acc = 0;
			}
		}

		public void Draw ()
		{
			SwinGame.DrawRectangle (Color.Transparent, (float)X, (float)Y, 80, 80);
			if (transparent == true) {
				if (sw.ElapsedMilliseconds <= 3000) {
					SwinGame.DrawBitmap ("player2.png", (float)X, (float)Y);
				} else {
					sw.Stop ();
					isInvisible = false;
					transparent = false;
				}
			} else {
				//if (MouseButton.LeftButton.Equals.case0)
				//if (CarSelectionController.CarVariety.Equals (CarType.Car)) {
				//	SwinGame.DrawBitmap ("player.png", (float)X, (float)Y);
				//} else if (CarSelectionController.CarVariety.Equals (CarType.SportCar)){
				//	SwinGame.DrawBitmap ("SportCar.png", (float)X, (float)Y);
				//}
				bool b = CarSelectionController.CarVariety.Equals (CarType.Car);
				bool a = CarSelectionController.CarVariety.Equals (CarType.SportCar);
				if (b == true) {
					SwinGame.DrawBitmap ("Player.png", (float)X, (float)Y);
				} else if (a == true) {
					SwinGame.DrawBitmap ("SportCar.png", (float)X, (float)Y);
				}

			}
		}

		public bool Speed {
			get { return speed; }
			set { speed = value; }

		}

		public bool Transparent {
			get { return transparent; }
			set { transparent = value; }
		}


		public double X
		{
			get{ return _x; }
			set{ _x = value; }

		}

		public double Y
		{
			get{ return _y; }
			set{ _y = value; }
		}

		public double SpeedX {
			get { return _spdX; }
			set { _spdX = value; }
		}

		public double SpeedY {
			get { return _spdY; }
			set { _spdY = value; }
		}

		public double Acceleration {
			get { return _acc; }
			set { _acc = value; }
		}
	}
}

