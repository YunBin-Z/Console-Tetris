using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_进阶俄罗斯方块
{
    //打印工厂类的墙壁
    //打印方块
    //得有一个区域来实时显示分数


    // 统一绘制所有需要显示的对象（墙壁、方块、分数等）
    //渲染器的疑问：我需不需要把墙壁和方块分开方法来实现

    
    internal class Renderer
    {
        //绘制对应的对象，想渲染墙壁就把墙壁对象传进来，想渲染方块就把方块对象传进来
        //无状态渲染器。
        public static void Draw(List<GameObject> gameobject)
        {
            foreach (var obj in gameobject)
            {
                obj.Draw();
            }
        }
        public static void DrawMap(Map map)
        {
            for (int y = 0; y < map.height; y++)
            {
                for (int x = 0; x < map.width; x++)
                {
                    if (map.grid[y, x])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(x, y);
                        Console.Write('#');
                    }
                    else
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(' ');
                    }
                }
            }
        }



        //擦除对应的对象
        public static void Vanish(List<GameObject> gameobject)
        {
            foreach (var obj in gameobject)
            {
                obj.Clear();
            }
        }
        public static void printScore(Map map)
        {
            Console.SetCursorPosition(5, map.height + 3);
            Console.Write($"累计分数:{map.SCORE}");
        }
    }
}
