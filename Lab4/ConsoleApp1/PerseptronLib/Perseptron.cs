using System;
using System.Collections.Generic;

namespace PerseptronLib
{
    public class Perseptron
    {
        private int _classCount;
        private int _instanceCount;
        private int _signCount;
        private List<PerseptronClass> _classes = new List<PerseptronClass>();
        private List<PerseptronClass> _weigher = new List<PerseptronClass>();
        private List<int> _functionResults;
        
        public void Evaluate(out string classes, out string functions)
        {
            classes = this.ToString();
            int iteration = 0;
            while (!CheckWeigher() && iteration < 1000)
            {
                iteration++;
            }

            if (iteration != 1000)
            {
                functions = GetFunctions();
            }
            else
            {
                functions = "Превышено количество итераций, функции найдены не верно\n\r";
                functions += GetFunctions();
            }
        }

        private string GetFunctions()
        {
            string result = "";
            for (int i = 0; i < _classCount; i++)
            {
                result += "d" + (i+1) + " = ";
                for (int j = 0; j < _signCount; j++)
                {
                    result += _weigher[i].Instances[0].Signs[j] + "*X" + (j + 1) + " + ";
                }

                result += _weigher[i].Instances[0].Signs[_signCount] + ";\n\r";
            }
            return result;
        }
        private bool CheckWeigher()
        {
            bool result = true;
            int classIndex = 0, instanceIndex = 0;
            for (int index = 0; index < _classCount * _instanceCount; index++)
            {
                PerseptronInstance perseptronInstance = _classes[classIndex].Instances[instanceIndex];
                for (int i = 0; i < _classCount; i++)
                {
                    int sum = 0;
                    for (int j = 0; j < _signCount + 1; j++)
                    {
                        sum += _weigher[i].Instances[0].Signs[j] * perseptronInstance.Signs[j];
                    }

                    _functionResults[i] = sum;
                }
                if (!ChangeWeigher(classIndex, perseptronInstance))
                {
                    result = false;
                }

                classIndex++;
                if (classIndex == _classCount)
                {
                    classIndex = 0;
                    instanceIndex++;
                }
            }

            return result;
        }

        private bool ChangeWeigher(int classIndex, PerseptronInstance perseptronInstance)
        {
            bool result = true;
            for (int i = 0; i < _classCount; i++)
            {
                if (i != classIndex)
                {
                    if (_functionResults[classIndex] <= _functionResults[i])
                    {
                        for (int j = 0; j < _signCount + 1; j++)
                        {
                            _weigher[i].Instances[0].Signs[j] -= perseptronInstance.Signs[j];
                        }
                        result = false;
                    }
                }
            }

            if (!result)
            {
                for (int j = 0; j < _signCount + 1; j++)
                {
                    _weigher[classIndex].Instances[0].Signs[j] += perseptronInstance.Signs[j];
                }
            }
            return result;
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

        public Perseptron(int classCount, int instanceCount, int signCount)
        {
            _classCount = classCount;
            _instanceCount = instanceCount;
            _signCount = signCount;

            _functionResults = new List<int>();

            for (int i = 0; i < classCount; i++)
            {
                PerseptronClass perseptronClass = new PerseptronClass(instanceCount, signCount);
                PerseptronClass weigherClass = new PerseptronClass(1,signCount + 1,true);
                _weigher.Add(weigherClass);
                _classes.Add(perseptronClass);
                _functionResults.Add(Int32.MaxValue);
            }
        }
    }

    public class PerseptronClass
    {
        public List<PerseptronInstance> Instances = new List<PerseptronInstance>();

        public PerseptronClass(int countOfInstance, int countOfSign, bool weigher = false)
        {
            for (int i = 0; i < countOfInstance; i++)
            {
                PerseptronInstance instance = new PerseptronInstance(countOfSign, weigher);
                Instances.Add(instance);
            }
        }

        public override string ToString()
        {
            string result = "";

            for (int i = 0; i < Instances.Count; i++)
            {
                result += "\tОбраз " + (i+1) + Instances[i].ToString();
            }
            return result;
        }
    }

    public class PerseptronInstance
    {
        public List<int> Signs = new List<int>();

        private Random _random = new Random();
        private int _range = 10;

        public PerseptronInstance(int countOfSign, bool weigher)
        {
            if (!weigher)
            {
                for (int i = 0; i < countOfSign; i++)
                {
                    int sign = _random.Next() % _range - _range / 2;
                    Signs.Add(sign);
                }
                Signs.Add(1);
            }
            else
            {
                for (int i = 0; i < countOfSign; i++)
                {
                    Signs.Add(0);
                }
            }
        }

        public override string ToString()
        {
            string result = " (";

            for (int i = 0; i < Signs.Count - 1; i++)
            {
                if (i != Signs.Count - 2)
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