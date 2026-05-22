using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    // GameObjectFactory 只生成GameObject列表（墙壁 / 方块）
    // 工厂类，只负责生成方块、墙壁的坐标数据，不负责绘制和管理
    internal class GameObjectFactory
    {
        // 生成想要的大小的固定墙壁（返回墙壁的GameObject列表）
        public static List<GameObject> CreateWalls(int mapWidth, int mapHeight)
        {
            var walls = new List<GameObject>();

            // 左右墙
            for (int y = 0; y < mapHeight; y++)
            {
                walls.Add(new GameObject(BrickType.Wall, 0, y));
                walls.Add(new GameObject(BrickType.Wall, mapWidth - 1, y));
            }

            // 底墙,mapWidth-1是因为，当取到mapWidth-2时，一个墙壁占横着占两个单位
            for (int x = 1; x < mapWidth-1; x ++)
            {
                walls.Add(new GameObject(BrickType.Wall, x, mapHeight - 1));
            }
            
            return walls;
        }

        /// <summary>
        /// 七种方块的相对坐标数据（以 pivot 为原点），用字典的键来存储形状，值来存储相对坐标
        /// </summary>
        private static readonly Dictionary<BrickType, Position[]> shapeData
            = new Dictionary<BrickType, Position[]>
        {
            {
                BrickType.I,
                new Position[]
                {
                    new Position(0, -1),
                    new Position(0, 0),
                    new Position(0, -1),
                    new Position(0, 1)
                }
            },

            {
                BrickType.O,
                new Position[]
                {
                   
                    new Position(1, 0),
                     new Position(0, 0),
                    new Position(0, 1),
                    new Position(1, 1)
                }
            },

            {
                BrickType.T,
                new Position[]
                {
                    new Position(-1, 0),
                    new Position(0, 0),
                    new Position(1, 0),
                    new Position(0, 1)
                }
            },

            {
                BrickType.L,
                new Position[]
                {
                    new Position(0, -1),
                    new Position(0, 0),
                    new Position(0, 1),
                    new Position(1, 1)
                }
            },

            {
                BrickType.J,
                new Position[]
                {
                    new Position(0, -1),
                    new Position(0, 0),
                    new Position(0, 1),
                    new Position(-1, 1)
                }
            },

            {
                BrickType.S,
                new Position[]
                {
                    
                    new Position(1, 0),
                    new Position(0, 0),
                    new Position(-1, 1),
                    new Position(0, 1)
                }
            },

            {
                BrickType.Z,
                new Position[]
                {
                    new Position(-1, 0),
                    new Position(0, 0),
                    new Position(0, 1),
                    new Position(1, 1)
                }
            }
        };

        //生成一个对应方块的GameObject的list，里面存坐标和类型，随机就生成随机的枚举值即可
        public static List<GameObject> CreateTetromino(BrickType type, Position pivotPos)
    {
        var result = new List<GameObject>();

        // 防止误传 Wall
        if (type == BrickType.Wall)
            throw new Exception("Wall 不是可生成的方块类型");

            //type这个是方块的枚举参数，shapeData[type]表示获取这个方块类型的相对坐标数组。
            var relativePositions = shapeData[type];//

            //遍历这个数组，把每个相对坐标都加上pivot即是真实坐标
        foreach (var relativePos in relativePositions)
        {
            Position worldPos = pivotPos + relativePos;
            result.Add(new GameObject(type,worldPos));
        }

        return result;
    }

  };
}


