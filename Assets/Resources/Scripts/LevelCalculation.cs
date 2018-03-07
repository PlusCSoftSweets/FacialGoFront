using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Resources.Scripts
{
    static public class LevelCalculation {
        static public int ExpToLevel(int exp) {
            int level = 0;
            int current = 0;
            int diff = 2;
            while(true) {
                current += diff;
                if (exp < current * 50)
                    return level;
                level++; diff++;
            }
        }
    }
}
