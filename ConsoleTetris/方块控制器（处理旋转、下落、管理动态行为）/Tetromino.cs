using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    /// <summary>
    /// Tetromino 管理单个方块（多个GameObject）的动态行为 不生成对象、不负责全局绘制
    /// </summary>
    /// 职责如下：
    //1 管理当前方块的4个格子
    //2 控制移动
    //3 控制旋转
    //4 提供坐标给 Map 检测碰撞
    //5 提供绘制

    //MoveLeft()
    //MoveRight()
    //MoveDown()
    //Rotate()
    //GetBlocks()
    //Draw()
    internal class Tetromino
    {
        List<GameObject> blocks;

        public Tetromino(List<GameObject> a)
        {
            blocks = a;
        }
        public List<GameObject> GetBlocks()
        {
            return blocks;
        }


        //方块都往左跑，要怎么做呢？
        //先检测能不能往左，如果可以那就把方块的x坐标-2，y不变

        public bool MoveLeft(Map map)
        {
            //1.先检测能不能往左（先假设位置往左，看看会不会碰撞，调用已经写好的碰撞检测方法）
            foreach (var block in blocks)
            {
                block.pos.x -= 1;
            }
            //如果真的碰撞了，那就把方块的x坐标+2，回到原来的位置
            if (map.CollisionDetection(blocks))
            {
                foreach (var block in blocks)
                {
                    block.pos.x += 1;
                }
                return false;
            }
            return true;
        }
        public bool MoveRight(Map map)
        {
            //1.先检测能不能往右（先假设位置往右，看看会不会碰撞，调用已经写好的碰撞检测方法）
            foreach (var block in blocks)
            {
                block.pos.x += 1;
            }
            //如果真的碰撞了，那就把方块的x坐标-2，回到原来的位置
            if (map.CollisionDetection(blocks))
            {
                foreach (var block in blocks)
                {
                    block.pos.x -= 1;
                }
                return false;
            }
            return true;
        }
        //快速下落的实现思路，多线程的线程休眠速度快一些
        public bool MoveDown(Map map)
        {
            //1.先检测能不能往下（先假设位置往下，看看会不会碰撞，调用已经写好的碰撞检测方法）
            foreach (var block in blocks)
            {
                block.pos.y += 1;
            }
            //
            if (map.CollisionDetection(blocks))
            {
                //如果真的碰撞了，那就把方块的y坐标-1，回到原来的位置
                foreach (var block in blocks)
                {
                    block.pos.y -= 1;
                }
                //如果不能下落了，返回false
                return false;
                //动态方块变成静态方块（这个逻辑不应该放在这里，应该放在游戏主循环里）
                //    map.FixBlocks(blocks);
            }
            return true;
        }
        public void Rotate(Map map)
        {
            //以第二个格子为中心，把当前坐标切换成以pivot为原点的相对坐标
            GameObject pivot = blocks[1];

            //创建TestPos的目的是为了存储试算的结果，如果试算的结果没有碰撞，那就把TestPos的坐标赋值给blocks，如果试算的结果有碰撞，那就不修改blocks的坐标
            //这样通过试算就避免了直接对blocks的坐标修改，避免了修改了blocks的坐标之后又要修改回去的麻烦
            List<Position> TestPos = new List<Position>();
            int temp = 0;
            foreach (var block in blocks)
            {
                //计算相对坐标
                int relativeX = block.pos.x - pivot.pos.x;
                int relativeY = block.pos.y - pivot.pos.y;
                //相对坐标顺时针旋转90度，新的相对坐标是（relativeY, -relativeX）
                //再定义两个变量来存储旋转后的相对坐标
                int rotX = relativeY;
                int rotY = -relativeX;

                //把旋转后的相对坐标转换成新的绝对坐标
                //试算结果存储在TestPos里，如果通过碰撞测试，就把TestPos的坐标应用到blocks
                int newX = pivot.pos.x + rotX;
                int newY = pivot.pos.y + rotY;
                TestPos.Add(new Position(newX, newY));
            }

            //为了应用检测碰撞的方法，临时创建一个 List<GameObject> test对象，用于检验测试
            List<GameObject> test = new List<GameObject>();

            //给test对象正确的初始化
            for (int i = 0; i < blocks.Count; i++)
            {
                test.Add(new GameObject(blocks[i].Type, TestPos[i]));
            }
            //检测旋转后的坐标是否碰撞，如果碰撞了就不修改blocks的坐标，如果没有碰撞就把TestPos的坐标赋值给blocks
            if (!map.CollisionDetection(test))
            {
                for (int i = 0; i < blocks.Count; i++)
                { 
                    blocks[i].pos = TestPos[i];
                }
            }
        }

       public bool CanSpawn(Map map)
        {
            if (map.CollisionDetection(blocks))
            { return false; }
            return true;
        }
        public void Draw()
        {
            foreach (var block in blocks)
            {
                block.Draw();
            }
        }

        public void Clear()
        {
            foreach (var block in blocks)
            {
                block?.Clear();
            }
        }
    }
}
 