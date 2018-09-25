using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.YuriSouza.Utility
{
    public static class Variables
    {
        public static int EnemiesPerLine = 7;
        public static int EnemiesPerCollumn = 4;
        public static int EnemyPositionStartLeft = 78;
        public static int EnemySpeed = 1;
        public static int EnemyShotSpeed = 3;
        public static int EnemyWidth = 24;
        public static int EnemyHeight = 24;
        public static string EnemyName = "invader";

        public static int AirShipSpeed = 5;
        public static int AirShipShotSpeed = 6;

        public static int TotalWalls = 4;
        public static int TotalLinesInWall = 4;
        public static int TotalCollumnInWall = 10;

        public static string ShieldName = "shield";
        public static int ShieldWidth = 6;
        public static int ShieldHeight = 8;

        public static string ShootNameAirShip = "ShotAirShip";
        public static string ShootNameEnemy = "ShotEnemy";
        public static int ShootWidth = 2;
        public static int ShootHeight = 13;
        public static int ShotsEnemiesInScreen = 4;

        public static int MinPositionTop = 150;
    }
}
