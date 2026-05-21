using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace C_进阶俄罗斯方块
{
    /// <summary>
    /// 砖块逻辑类型（包含墙壁 + 7种方块）
    /// </summary>
   public enum BrickType
    {
        Wall,   // 墙壁
        I,
        O,
        T,
        L,
        J,
        S,
        Z
    }

    /// <summary>
    /// 坐标结构体
    /// </summary>
   public struct Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        // 定义坐标相加的运算符重载，方便后续计算方块位置
        public static Position operator +(Position p1, Position p2)
        {
            return new Position(p1.x + p2.x, p1.y + p2.y);
        }

        public static bool operator ==(Position p1, Position p2)
        {
            return p1.x == p2.x && p1.y == p2.y;
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return !(p1 == p2);
        }
    }

    /// <summary>
    /// 单个格子对象
    /// </summary>
    public class GameObject : IDraw
    {
        public Position pos;

        public BrickType Type;

        public GameObject(BrickType type, Position p)
        {
            pos = p;
            Type = type;
        }
        public GameObject(BrickType type ,int x,int y)
        {
            pos = new Position(x, y);
            Type = type;
        }
        public void Draw()
        {
            // 跳过屏幕上方尚未进入可见区的格子
            if (pos.y < 0) 
                return;

            // 可选：额外边界保护，避免越界
            if (pos.x < 0 || pos.x >= Console.BufferWidth || pos.y >= Console.BufferHeight) 
                return;

            Console.SetCursorPosition(pos.x, pos.y);

            // 根据格子的类型设置不同的颜色
            switch (Type)
            {
                case BrickType.Wall:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case BrickType.I:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case BrickType.O:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case BrickType.T:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;

                case BrickType.L:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;

                case BrickType.J:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

                case BrickType.S:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case BrickType.Z:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            Console.Write('#');
        }
        //格子消除，方便方块进行擦除
        public void Clear()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.Write(' ');
        }
    }
}