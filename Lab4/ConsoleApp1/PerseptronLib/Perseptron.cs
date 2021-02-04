using System;
using System.Collections.Generic;

namespace PerseptronLib
{
    public class Perseptron
    {
        private int _classCount;
        private int _instanseCount;
        private int _signCount;
        private List<PerseptronClass> _classes = new List<PerseptronClass>();
        
        public void Evaluate(out string classes, out string functions)
        {
            classes = this.ToString();
            
            functions = "2";
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < _classCount; i++)
            {
                result += "Класс " + (i+1) + " \n\r" + _classes[i].ToString();

            }
            return result;
        }

        public Perseptron(int classCount, int instanseCount, int signCount)
        {
            _classCount = classCount;
            _instanseCount = instanseCount;
            _signCount = signCount;

            for (int i = 0; i < classCount; i++)
            {
                PerseptronClass perseptronClass = new PerseptronClass(instanseCount, signCount);
                _classes.Add(perseptronClass);
            }

        }
    }

    public class PerseptronClass
    {
        public List<PerseptronInstanse> Instanses = new List<PerseptronInstanse>();

        public PerseptronClass(int countOfInstanse, int countOfSign)
        {
            for (int i = 0; i < countOfInstanse; i++)
            {
                PerseptronInstanse instanse = new PerseptronInstanse(countOfSign);
                Instanses.Add(instanse);
            }
        }

        public override string ToString()
        {
            string result = "";

            for (int i = 0; i < Instanses.Count; i++)
            {
                result += "\tОбраз " + (i+1) + Instanses[i].ToString();
            }
            return result;
        }
    }

    public class PerseptronInstanse
    {
        public List<int> Signs = new List<int>();

        private Random _random = new Random();
        private int _range = 10;

        public PerseptronInstanse(int countOfSign)
        {
            for (int i = 0; i < countOfSign; i++)
            {
                int sign = _random.Next() % _range - _range / 2;
                Signs.Add(sign);
            }
        }

        public override string ToString()
        {
            string result = " (";

            for (int i = 0; i < Signs.Count; i++)
            {
                if (i != Signs.Count - 1)
                {
                    result += Signs[i] + ",";
                }
                else
                {
                    result += Signs[i];
                }
            }

            result += ")\n\r";
            return result;
        }
    }
    
}