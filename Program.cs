using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomokuIA
{
    struct move
    {
        public int x;
        public int y;
    }

    class minmax
    {
        public struct situation
        {
            public int x;
            public int y;
            public int consecutive;
            public int opened;
            public int score;
        }
        Random rand = new Random();
        int [,]_board = null;
        int _width;
        int _height;
        move _turn;
        bool current_turn;
        public situation init_situation()
        {
            situation sit = new situation();

            sit.x = 0;
            sit.y = 0;
            sit.consecutive = 0;
            sit.opened = 0;
            sit.score = 0;
            return (sit);
        }

        public int calcul_score(situation sit)
        {
            if (sit.opened == 0 && sit.consecutive < 5)
                return 0;
            switch (sit.consecutive)
            {
                case 4:
                    switch (sit.opened)
                    {
                        case 1:
                            if (current_turn)
                                return 1000000;
                            return 50;
                        case 2:
                            if (current_turn)
                                return 1000000;
                            return 5000;
                    }
                    return 0;
                case 3:
                    switch (sit.opened)
                    {
                        case 1:
                            if (current_turn)
                                return 7;
                            return 5;
                        case 2:
                            if (current_turn)
                                return 10000;
                            return 50;
                    }
                    return 0;
                case 2:
                    switch (sit.opened)
                    {
                        case 1:
                            return 2;
                        case 2:
                            return 5;
                    }
                    return 0;
                case 1:
                    switch (sit.opened)
                    {
                        case 1:
                            return 1;
                        case 2:
                            return 2;
                    }
                    return 0;
                default:
                    return 2000000;
            }
        }

        public situation AnalyzeCase(situation sit, int to)
        {
            if (_board[sit.y, sit.x] == to)
                sit.consecutive += 1;
            else if (_board[sit.y, sit.x] == 0 && sit.consecutive > 0)
            {
                sit.opened += 1;
                sit.score += calcul_score(sit);
                sit.consecutive = 0;
                sit.opened = 0;
            }
            else if (_board[sit.y, sit.x] == 0)
                sit.opened = 1;
            else if (sit.consecutive > 0)
            {
                sit.score += calcul_score(sit);
                sit.consecutive = 0;
                sit.opened = 0;
            }
            else
                sit.opened = 0;
            return (sit);
        }

        public situation end_line(situation sit)
        {
            if (sit.consecutive > 0)
                sit.score += calcul_score(sit);
            sit.opened = 0;
            sit.consecutive = 0;
            return (sit);
        }

        public int AnalyzeHorizontal(int to)
        {
            situation sit = init_situation();
            sit.x = -1;
            sit.y = -1;

            while (++sit.y < _height)
            {
                while (++sit.x < _width)
                    sit = AnalyzeCase(sit, to);
                sit.x = -1;
                sit = end_line(sit);
            }
            return (sit.score);
        }

        public int AnalyzeVertical(int to)
        {
            situation sit = init_situation();
            sit.x = -1;
            sit.y = -1;

            while (++sit.x < _width)
            {
                while (++sit.y < _height)
                    sit = AnalyzeCase(sit, to);
                sit.y = -1;
                sit = end_line(sit);
            }
            return (sit.score);
        }

        public int AnalyzeLeftDiagonal(int to)
        {
            situation sit = init_situation();
            sit.y = _height - 1;
            int tmp;

            while (sit.y >= 0)
            {
                tmp = sit.y;
                while (sit.x < _width && sit.y < _height)
                {
                    sit = AnalyzeCase(sit, to);
                    sit.x += 1;
                    sit.y += 1;
                }
                sit = end_line(sit);
                sit.x = 0;
                sit.y = tmp - 1;
            }

            sit.x = 1;
            sit.y = 0;
            while (sit.x < _width)
            {
                tmp = sit.x;
                while (sit.y < _height && sit.x < _width)
                {
                    sit = AnalyzeCase(sit, to);
                    sit.x += 1;
                    sit.y += 1;
                }
                sit = end_line(sit);
                sit.x = tmp + 1;
                sit.y = 0;
            }
            return (sit.score);
        }

        public int AnalyzeRightDiagonal(int to)
        {
            situation sit = init_situation();
            sit.y = _height - 1;
            sit.x = _width - 1;
            int tmp;

            while (sit.y >= 0)
            {
                tmp = sit.y;
                while (sit.x >= 0 && sit.y < _height)
                {
                    sit = AnalyzeCase(sit, to);
                    sit.x -= 1;
                    sit.y += 1;
                }
                sit = end_line(sit);
                sit.x = _width;
                sit.y = tmp - 1;
            }

            sit.x = _width - 2;
            sit.y = 0;
            while (sit.x >= 0)
            {
                tmp = sit.x;
                while (sit.y < _height && sit.x >= 0)
                {
                    sit = AnalyzeCase(sit, to);
                    sit.x -= 1;
                    sit.y += 1;
                }
                sit = end_line(sit);
                sit.x = tmp - 1;
                sit.y = 0;
            }
            return (sit.score);
        }

        int Eval()
        {
            int score = 0;
            int score2 = 0;

         
               score += AnalyzeHorizontal(1);
            score += AnalyzeVertical(1);
            score += AnalyzeLeftDiagonal(1);
            score += AnalyzeRightDiagonal(1);
            current_turn = !current_turn;
            score2 += AnalyzeRightDiagonal(2);
            score2 += AnalyzeHorizontal(2);
            score2 += AnalyzeVertical(2);
            score2 += AnalyzeLeftDiagonal(2);
            current_turn = !current_turn;
            return (score - score2);
        }
        move CreateMove(int _y, int _x)
        {
            move r;
            r.x = _x;
            r.y = _y;
            return r;
        }
        List<move> elagage()
        {
            move[] around = new move[8];
            around[0].x = 0;
            around[0].y = 1;

            around[1].x = 0;
            around[1].y = -1;
            around[2].x = 1;
            around[2].y = 0;
            around[3].x = -1;
            around[3].y = 0;

            around[4].x = -1;
            around[4].y = -1;

            around[5].x = -1;
            around[5].y = 1;

            around[6].x = 1;
            around[6].y = -1;

            around[7].x = 1;
            around[7].y = 1;

            List<move> ret = new List<move>();
            int i, j;
            i = j = 0;
            while (i < _height)
            {
                j = 0;
                while (j < _width)
                {
                    if (_board[i, j] != 0)
                    {
                        int k = -1;
                        while (++k <= 7)
                        {
                            if (i + around[k].y >= 0 && j + around[k].x >= 0 &&
                                i + around[k].y < _height && j + around[k].x < _width &&
                                _board[i + around[k].y, j + around[k].x] == 0)
                                  ret.Add(CreateMove(i + around[k].y, j + around[k].x));

                        }
                    }
                    ++j;
                }
                ++i;
            }
            List<move> r = new List<move>();
            foreach(move m in ret)
            {
                if (_board[m.y, m.x] == 0)
                    r.Add(m);

            }

            return r;
        }

        int Max(int deep)
        {
            current_turn = true;
            if (deep == 0)
                return (Eval());
            List<move> toTest = elagage();
            int tmp;
            int max = -10000000;
            move best;
            best.x = 0;
            best.y = 0;
            foreach(move m in toTest)
            {
                _board[m.y, m.x] = 1;

                tmp = Min(deep - 1);
                if (tmp > max)
                {
                    best.x = m.x;
                    best.y = m.y;
                    max = tmp;
                }
                _board[m.y, m.x] = 0;
            }
            return (max);
        }
        int Min(int deep)
        {
            current_turn = false;

            if (deep == 0)
                return (Eval());
            List<move> toTest = elagage();

            int tmp;
            int max = 100000000;

            move best;
            best.x = 0;
            best.y = 0;
            foreach (move m in toTest)
            {
                _board[m.y, m.x] = 2;
                tmp = Max(deep - 1);

                if (tmp < max)
                {
                    best.x = m.x;
                    best.y = m.y;
                    max = tmp;
                }
                _board[m.y, m.x] = 0;
            }
            return (max);
        }


        int Pattern(int i, int j)
        {
            move[] pattern = new move[3];
            pattern[1].x = pattern[2].y = pattern[0].x = pattern[0].y = 1;
            pattern[1].y = pattern[2].x = 0;
            int type = _board[i, j];
            int n = 1;
            int k = 0;
            while (k < 3)
            {
                n = 1;
                while (n < 6)
                {
                    if (j + (pattern[n].x * n) >= _height || i + (pattern[n].y * n) >= _width ||
                        _board[i + pattern[n].y * n, j + pattern[n].x * n] != type)
                    {
                        break;
                    }
                        if (n == 5)
                        return (0);
                    n++;
                }
                k++;
            }
            return (-1);
        }
        int Winner()
        {
            int i = 0;
            int j = 0;
            while (i < _width)
            {
                j = 0;
                while (j < _height)
                {
                    if (_board[i, j] != 0)
                    {
                        if (Pattern(i, j) == 0)
                        {
                            return (0);
                        }
                    }
                    ++j;
                }
                ++i;
            }
            return (-1);
        }

        public minmax(int [,]tab, int w, int h)
        {

            _board = tab;
            _height = h;
            _width = w;
            current_turn = true;
            int deep = 2;
            move best;
            best.x = 0;
            best.y = 0;
            List<move> toTest = elagage();
            int tmp;
            int max = -100000000;
            foreach (move m in toTest)
            {
                _board[m.y, m.x] = 1;
                tmp = Min(deep - 1);

                if (tmp > max)
                {
                    best.x = m.y;
                    best.y = m.x;
                    max = tmp;
                }
                _board[m.y, m.x] = 0;
            }
            _turn = best;
        }
        public move getBestMove()
        {
            return _turn;
        }
    }
    class Program
    {
    }
}