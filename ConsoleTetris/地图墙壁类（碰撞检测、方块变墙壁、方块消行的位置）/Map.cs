using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    internal class Map
    {
       public int SCORE = 0;
        // true = 有方块（墙或静态块）
        // false = 空
       public bool[,] grid;
       BrickType Type = BrickType.Wall;

        // 地图宽高（包含墙）
        public int width;
        public int height;

        public Map(int w, int h)
        {
            width = w;
            height = h;

            grid = new bool[height, width];

            InitWalls();
        }

        /// <summary>
        /// 初始化墙壁
        /// </summary>
        private void InitWalls()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // 左墙1格 + 右墙1格 + 底墙
                    if (x == 0  || x == width - 1  || y == height - 1)
                    {
                        grid[y, x] = true;
                    }
                    else
                    {
                        grid[y, x] = false;
                    }
                }
            }
        }

        /// <summary>
        /// 碰撞检测
        /// </summary>
        public bool CollisionDetection(List<GameObject> blocks)
        {
            foreach (var block in blocks)
            {
                int x = block.pos.x;
                int y = block.pos.y;
                if (x < 0 || x >= width || y < 0 || y >= height) return true; // 或者对 y<0 采用不同策略
                if (grid[y, x]) return true;
            }
            return false;
        }
    

        /// <summary>
        /// 将动态方块固定为静态方块 ，传入要变化成动态方块的坐标列表，地图类负责把这些坐标在二维数组里对应的位置改成true，表示这些位置上有了静态方块了
        /// </summary>
        public void FixBlocks(List<GameObject> blocks)
        {
            foreach (GameObject block in blocks)
            {
                //block.Type = BrickType.Wall;
                int x = block.pos.x;
                int y = block.pos.y;

                grid[y, x] = true;
            }
        }

        /// <summary>
        /// 消除地图中的满行
        /// 检测一次是否需要消行，可以消行就消行 
        /// </summary>
        public void ClearLines()
        {
           
            // 最底可写行（底部墙上面一行）
            int writeRow = height - 2;
            // 从底往上扫描
            for (int y = height - 2; y >= 0; y--)
            {
                bool full = true;
                
                for (int x = 1; x < width - 1; x++)
                {
                    if (!grid[y, x])
                    {
                        full = false;
                        break;
                    }
                }
                // 未满行 -> 保留
                if (!full)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        grid[writeRow, x] = grid[y, x];
                    }

                    writeRow--;
                }
                else
                {
                    SCORE++;
                }
            }
            // 清空上方多余行
            for (int y = writeRow; y >= 0; y--)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    grid[y, x] = false;
                }
            }
        }
    }
}







//namespace C_进阶俄罗斯方块粗稿
//{
//    //这个类要做的事：
//    //1. 检测是否碰撞（参数是方块的坐标），只负责检测动态方块和墙壁是否发生了碰撞，具体要做的静态方块消除、动态方块移动、动态方块旋转、方块下落等逻辑不在这里负责
//    //2. 静态方块的消除，检测静态方块是否满行，如果满行了就消除掉，并且把上面的方块下落（参数是静态方块的坐标列表）
//    internal class Map
//    {
//        //我预先得先知道当前地图墙壁得坐标，才能检测动态方块和墙壁的碰撞
//        //我得用一个集合来表示当前地图上所有静态方块的坐标，
//        //我有一个问题，该用什么类型得数据去存储墙壁和静态方块的坐标呢？墙壁和静态方块得坐标应该分开存吗？
//        //因为我还要在检测是否碰撞得时候，不应该分开判断。但是在静态方块的消除的时候又不得不分开判断，所以我觉得还是分开存储比较好
//        //我不太喜欢用二维数组的行标和列标来存储坐标信息，因为坐标的x、y在表示的时候本身就不固定，很难用行标和列标来对应坐标。
//        //但是我可以这么做，二维数组不需要对应，只需要存储一个bool值，表示这个位置上有没有静态方块或者墙壁就行了，这样就不需要考虑坐标的x、y和行标、列标的关系
//        //这样会衍生一个问题，如何进行方块的消除，我要不要这样，再用一个二维数组来存储游戏区域的行标和列标，二维数组还是存bool值，当这个代表游戏区域内的二维数组的某一行满了，就把这个二维数组的这一行清空，
//        //（并且把上面的行往下移动，这样就可以实现方块的消除了）这个是方块类要做的消除后的逻辑，不要放在地图类做比较好
//        //我产生了一个新的问题，这两个二维数组是放在类的方法内当地变量，还是放在类的成员变量里？
//        //我觉得应该放在成员变量里，因为它们是地图类的核心数据结构，地图类的所有方法都要用到它们，所以放在成员变量里比较好，因为检测的时候频繁创建也挺不好的

//        //第一个二维数组，用于碰撞检测，大小应该等同于包含墙壁在内的游戏范围，我认为这个数组的大小应该在地图类的构造函数里传入，这样就可以根据传入的大小来创建这个二维数组了
//        public bool[,] collisionMap;

//        //第二个二维数组，用于方块消除，大小应该等同于不包含墙壁的游戏范围，这样就可以根据传入的大小来创建这个二维数组了
//        public bool[,] collisionMap2;

//        //因为需要二维数组的大小来界定判断的范围，所以我还需要两个成员变量来存储游戏范围的宽度和高度，这样就可以在方法里用这两个变量来界定判断的范围了
//        public int map_h;
//        public int map_w;

//        //如何把这两个二维数组正确初始化呢？这个逻辑该怎么办？
//        //这两个数组的具体值都赋值一个默认值吧，就是墙壁都true，其他位置都false，等到生成静态方块的时候，再把对应位置的值改成true就行了，这样就可以正确地初始化了

//        public Map(int width, int height)
//        {
//            //目标：给两个二维数组赋值，在墙壁的部分赋值true，在其他位置赋值false
//            //第一个二维数组的大小应该等同于包含墙壁在内的游戏范围，所以它的宽度应该是传入的宽度，长度应该是传入的高度
//            //第二个二维数组的大小应该等同于不包含墙壁的游戏范围，所以它的宽度应该是传入的宽度-2，长度应该是传入的高度-1
//            map_w = width;
//            map_h = height;
//            collisionMap = new bool[map_h,map_w];
//            collisionMap2 = new bool[map_h - 1,map_w - 4];
//            //第一个数组在墙壁的部分赋值true，在其他位置赋值false
//            //第二个数组因为排除了墙壁，所以默认全部位置都赋值false就行了

//            //出现了一个问题，因为格子横着占两个单位，竖着占一个单位，
//            //二维数组的列标是连续的，但是格子占的横向单位是两个，所以我在给第一个二维数组赋值的时候，得把每一行的第0、1列和最后两列都赋值true，外带还有最底下的墙壁赋值为true=
//            //特殊情况直接用if来判断，如果列标是0、1或者最后两列，或者行标是最后一行，就赋值true，否则赋值false，这样就可以正确地初始化了
//            for (int i = 0; i < map_h; i++)
//            {
//                for (int j = 0; j < map_w; j++)
//                {
//                    //j是列标，i是行标，行标和列标都是从0开始，最大取到Max-1，所以最后两列的列标是map_w-2和map_w-1，最后一行的行标是map_h -1
//                    if (j == 0 || j == 1 || j == map_w - 2 || j == map_w - 1 || i == map_h - 1)
//                    {
//                        collisionMap[i, j] = true;
//                    }
//                    else
//                    {
//                        collisionMap[i, j] = false;
//                    }
//                }
//            }
//            //第二个数组因为排除了墙壁，所以默认全部位置都赋值false就行了
//            for (int i = 0; i < map_h - 1; i++)
//            {
//                for (int j = 0; j < map_w - 4; j++)
//                {
//                    collisionMap2[i, j] = false;
//                }
//            }


//        }

//        //得有一个逻辑就是动态方块变成静态方块的时候，必须通知这两个二维数组，把对应位置的值改成true，这样就可以正确地维护这两个二维数组了
//        //这里我在想这个方块落地变静态方块之后给二维数组传值的步骤放在哪里比较合适，如果放在地图类里，那就省去了沟通Map类的步骤了，可以直接在地图类里把对应位置的值改成true了，这样就比较方便了


//        //如果想吧方块落地变静态方块的给二维数组传值的步骤放在地图类，得写在方块和墙壁碰撞检测的方法里，
//        //我有点担心一个事情，这样会不会导致碰撞检测变得不纯粹，这里游戏得方块类需要多次用到碰撞检测的逻辑，如果加上给二维数组传值的工作，代码这块复用的部分有点冗余
//        //还是把方块落地变静态方块之后给二维数组传值的步骤放在方块类里比较好

//        /// <summary>
//        /// 这个函数用来检测动态方块和墙壁（包括静态方块）的碰撞，参数是一个动态方块的坐标列表，返回值是一个bool值，表示是否发生了碰撞，如果发生了碰撞就返回true，否则返回false
//        /// 我这里的参数用List<GameObject>而不用List<Position>是担心到时候给参数赋值的时候不太方便，
//        /// 如果我拿到了List<GameObject>，想要赋值List<Position>，得先创建一个List<Position>，得写一个for循环然后再把List<GameObject>里的每个GameObject的pos属性赋值给List<Position>
//        /// <returns></returns>
//        public bool Collision_Detection(List<GameObject> dynamicBlock)
//        {
//            //查看这个动态方块的坐标列表里的每个坐标，在第一个二维数组里对应的位置的值，
//            //如果有一个位置的值是true，就说明发生了碰撞了，就返回true，否则就继续判断，直到判断完了这个动态方块的所有坐标，如果都没有发生碰撞了，就返回false
//            for (int i = 0; i < dynamicBlock.Count; i++)
//            {
//                //我在想一个问题，动态方块的坐标系和collisionMap的坐标系是一样的吗？如果不一样的话，我在判断的时候就得把动态方块的坐标转换成collisionMap的坐标了，这样就比较麻烦了
//                //我发现了，二维数组是[行标，列标]
//                //但是坐标结构体的(X,Y)是(列标,行标)，这两个坐标系是相反的，所以我在判断的时候得把动态方块的坐标的x和y交换一下，这样就可以正确地判断了\
//                //坐标中的(1，0)等同于二维数组中的[0,1]，二维数组中[1,0]代表第二行第一列，而不是坐标里的(0,1)，所以我在判断的时候得把动态方块的坐标的x和y交换一下
//                if (collisionMap[
//                    dynamicBlock[i].pos.y,
//                    dynamicBlock[i].pos.x]
//                    == true)
//                {
//                    return true;
//                }
//            }
//            return false;
//        }
//        //我在想一个事情，这个消除了逻辑该怎么做，我除了得把二维数组的这一行的值给改成false，我还得做什么？
//        //上面的行往下移动这个怎么做？具体移动多少行
//        //问ai了，ai提醒我一个事情，俄罗斯方块存在，不连续消行、连续消行的情况，
//        //如果我用计算下落距离的思路来做的话，这么做很麻烦，第一就是不连续消行，因为存在上排和下排同时消除的情况，保留的块的位置不计算放在哪里

//        //静态方块的消除函数
//        public void ClearLines()
//        {
//            //这个函数的逻辑是，先检测第二个二维数组里是否有满行了，如果有满行了，就把这个满行的行标记录下来，然后把这个满行的这一行清空（把这一行的值都改成false），然后把上面的行往下移动（把上面行的值往下移动一行，这样就可以实现方块的消除了）
//            //我在想一个问题，第二个二维数组里存储的是不包含墙壁的游戏范围，所以它的行标和列标是从0开始的，
//            //但是在地图类里，游戏范围是包含墙壁的，所以它的行标和列标是从0开始的，但是它的最大行标和列标是比第二个二维数组的大2，
//            //所以我在判断的时候得把第二个二维数组里的行标和列标加上2，这样就可以正确地判断了
//            //for (int i = 0; i < maph - 4; i++)
//            //{
//            //    bool isFull = true;
//            //    for (int j = 0; j < mapw - 1; j++)
//            //    {
//            //        if (collisionMap2[i, j] == false)
//            //        {
//            //            isFull = false;
//            //            break;
//            //        }
//            //    }
//            //    if (isFull)
//            //    {
//            //        //把这个满行的这一行清空（把这一行的值都改成false）
//            //        for (int j = 0; j < mapw - 1; j++)
//            //        {
//            //            collisionMap2[i, j] = false;
//            //        }
//            //        //把上面的行往下移动（把上面行的值往下移动一行，这样就可以实现方块的消除了）
//            //        for (int k = i - 1; k >= 0; k--)
//            //        {
//            //            for (int j = 0; j < mapw - 1; j++)
//            //            {
//            //                collisionMap2[k + 1, j] = collisionMap2[k, j];
//            //            }
//            //        }
//            //        //把最上面的一行清空（把这一行的值都改成false）
//            //        for (int j = 0; j < mapw - 1; j++)
//            //        {
//            //            collisionMap2[0, j] = false;
//            //        }
//            //    }

//            int cleared = 0;//cleared用来记录消除了多少行了，消除的行数越多，得分就越高了

//            //writeRow：下一行应该写到哪里 ，writeRow = 5  意思是：下一行要写到第5行

//            //从底往上扫描 ,writeRow的初始值对应的大小是游戏区域的最后一行的行标了
//            int writeRow = height - 1;

//            for (int y = height - 1; y >= 0; y--)
//            {
//                bool full = true;

//                for (int x = 0; x < width; x++)
//                {
//                    if (!grid[x, y])  //只要这一行有一个列得bool是空的，就说明这一行不满了，就把full标记改成false，然后跳出循环，继续扫描下一行
//                    {
//                        full = false;
//                        break;
//                    }
//                }
//                //只有未满的行，它对应的full是false，因此才会进入这里的逻辑，保留未满的行 
//                if (!full)
//                {
//                    // 把这一行中每一列的内容都复制粘贴到writeRow这里去，把这一行复制到 writeRow
//                    for (int x = 0; x < width; x++)
//                    {
//                        grid[writeRow, x] = grid[y, x];
//                    }

//                    writeRow--;
//                }
//                else
//                {
//                    cleared++;
//                }
//            }
//            //到这里，writeRow的值是最后一个未满行的上面一行的行标了，writeRow+1才是最后一个未满行的行标了，所以从writeRow+1开始往下的行都是多余的行了，都要清空掉了
//            // 重新排列
//            // 把上面多余的行清空
//            for (int y = writeRow; y >= 0; y--)
//            {
//                for (int x = 0; x < width; x++)
//                {
//                    grid[x, y] = false;
//                }
//            }

//            return cleared;



//    }
//    }
//}